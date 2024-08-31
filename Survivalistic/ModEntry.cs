using Survivalistic_Rebooted.Framework.UI;
using Survivalistic_Rebooted.Framework.UI.Bars;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using Survivalistic_Rebooted.Framework.APIs;
using Survivalistic_Rebooted.Framework.Common;
using Survivalistic_Rebooted.Framework.Common.Affection;
using Survivalistic_Rebooted.Framework.Databases;
using Survivalistic_Rebooted.Framework.Misc;
using Survivalistic_Rebooted.Models.Config;

namespace Survivalistic_Rebooted
{
    public class ModEntry : Mod
    {
        public static ModEntry Instance { get; private set; }

        public static Data Data { get; set; }

        public static Config Config { get; private set; }

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            Config = Helper.ReadConfig<Config>();

            Textures.LoadTextures();

            helper.Events.GameLoop.GameLaunched += OnGameLaunch;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.UpdateTicked += OnUpdate;
            helper.Events.GameLoop.TimeChanged += OnTimeChanged;
            helper.Events.GameLoop.SaveLoaded += OnSaveLoaded;
            helper.Events.GameLoop.DayStarted += OnDayStarted;

            helper.Events.Multiplayer.PeerConnected += OnPlayerConnected;
            helper.Events.Multiplayer.ModMessageReceived += OnMessageReceived;

            helper.Events.Display.RenderingHud += RenderMaster.OnRenderingHud;
            //helper.Events.Display.RenderedActiveMenu += RenderMaster.OnActiveMenu;
            helper.Events.GameLoop.ReturnedToTitle += OnReturnToTitle;

            helper.ConsoleCommands.Add("survivalistic_restore_hunger", "Feeds a player.\nUsage: survivalistic_restore_hunger 'food_amount' 'player_name'", Commands.RestoreHunger);
            helper.ConsoleCommands.Add("survivalistic_restore_thirst", "Hydrates a player.\nUsage: survivalistic_restore_thirst 'hydration_amount' 'player_name'", Commands.RestoreThirst);
            helper.ConsoleCommands.Add("survivalistic_sate", "Set full status to a player.\nUsage: survivalistic_sate 'player_name'", Commands.Sate);
            helper.ConsoleCommands.Add("survivalistic_forcesync", "Forces the synchronization in multiplayer to all players.\nUsage: survivalistic_forcesync", Commands.ForceSync);

            DBController.LoadDatabases();
        }

        private void OnGameLaunch(object sender, GameLaunchedEventArgs e)
        {
            bool result = new ConfigMenuInitializer(ModManifest, Helper, Config,
                                                    Instance.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu")).InitializeModMenu();
            string message = result ? "Generic Mod Menu successfully loaded for this mod!" :
                                      "Generic Mod Menu isn't found... skip.";

            Monitor.Log(message, LogLevel.Info);
        }

        private void OnReturnToTitle(object sender, ReturnedToTitleEventArgs e) => 
                NetController.IsFirstLoad = false;

        private void OnUpdate(object sender, UpdateTickedEventArgs e)
        {
            BarsPosition.SetBarsPosition();

            Interaction.NeedsRestoration.PerformEatingCheckAndApplyEffectsIfPossible();
            Interaction.NeedsConsumption.CheckDoesPlayerUseToolAndApplyEffectsIfRelevant();
            Interaction.UpdateTickInformation();
            Penalties.VerifyPassOut();
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            Interaction.NeedsConsumption.ApplyPassiveStatsDecreaseIfPossibleAndApplyEffectsIfRelevant();
            BarsUpdate.CalculatePercentage();

            NetController.Sync();
        }

        private void OnDayStarted(object sender, DayStartedEventArgs e)
        {
            Data.ActualHunger -= Config.FoodDecreaseAfterSleep;
            Data.ActualThirst -= Config.ThirstDecreaseAfterSleep;

            OnUpdate(default, default);
        }

        private void OnSaveLoaded(object sender, SaveLoadedEventArgs e)
        {
            if (NetController.IsFirstLoad) NetController.Sync();

            Interaction.AwakeManager.Awake();
            Interaction.AwakeManager.ReceiveAwakeInfo();

            NetController.Sync();
            BarsUpdate.CalculatePercentage();
            BarsWarnings.VerifyStatus();
        }

        private void OnPlayerConnected(object sender, PeerConnectedEventArgs e) =>
                     NetController.SyncSpecificPlayer(e.Peer.PlayerID);

        private void OnMessageReceived(object sender, ModMessageReceivedEventArgs e) =>
                     NetController.OnMessageReceived(e);
    }
}
