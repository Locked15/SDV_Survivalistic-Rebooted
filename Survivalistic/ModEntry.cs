using StardewModdingAPI;
using StardewModdingAPI.Events;
using Survivalistic_Rebooted.Framework.APIs;
using Survivalistic_Rebooted.Framework.Bars;
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

            helper.Events.Display.RenderingHud += Renderer.OnRenderingHud;
            //helper.Events.Display.RenderedActiveMenu += Renderer.OnActiveMenu;
            helper.Events.GameLoop.ReturnedToTitle += OnReturnToTitle;

            helper.ConsoleCommands.Add("survivalistic_feed", "Feeds a player.\nUsage: survivalistic_feed 'food_amount' 'player_name'", Commands.Feed);
            helper.ConsoleCommands.Add("survivalistic_hydrate", "Hydrates a player.\nUsage: survivalistic_hydrate 'hydration_amount' 'player_name'", Commands.Hydrate);
            helper.ConsoleCommands.Add("survivalistic_fullness", "Set full status to a player.\nUsage: survivalistic_fullness 'player_name'", Commands.Fullness);
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

        private void OnReturnToTitle(object sender, ReturnedToTitleEventArgs e)
        {
            NetController._firstLoad = false;
        }

        private void OnUpdate(object sender, UpdateTickedEventArgs e)
        {
            BarsPosition.SetBarsPosition();

            Interaction.EatingCheck();
            Interaction.UsingToolCheck();
            Interaction.UpdateTickInformation();
            Penalty.VerifyPassOut();
        }

        private void OnTimeChanged(object sender, TimeChangedEventArgs e)
        {
            BarsUpdate.UpdateBarsInformation();
            BarsUpdate.CalculatePercentage();
            BarsWarnings.VerifyStatus();

            if (!Benefits.VerifyBenefits())
                Penalty.VerifyPenalty();

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
            if (!NetController._firstLoad) NetController.Sync();
            Interaction.Awake();
            NetController.Sync();
            Interaction.ReceiveAwakeInfo();
            BarsUpdate.CalculatePercentage();
            BarsWarnings.VerifyStatus();
        }

        private void OnPlayerConnected(object sender, PeerConnectedEventArgs e) =>
                     NetController.SyncSpecificPlayer(e.Peer.PlayerID);

        private void OnMessageReceived(object sender, ModMessageReceivedEventArgs e) =>
                     NetController.OnMessageReceived(e);
    }
}
