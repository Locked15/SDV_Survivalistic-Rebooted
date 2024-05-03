using StardewModdingAPI;
using StardewModdingAPI.Events;
using Survivalistic_Rebooted.Framework.APIs;
using Survivalistic_Rebooted.Framework.Bars;
using Survivalistic_Rebooted.Framework.Common;
using Survivalistic_Rebooted.Framework.Common.Affection;
using Survivalistic_Rebooted.Framework.Databases;
using Survivalistic_Rebooted.Framework.Networking;
using Survivalistic_Rebooted.Framework.Rendering;
using Survivalistic_Rebooted.Models;

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
            bool result = InitializeModMenu();
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
            BarsPosition.SetBarsPosition();
            Interaction.ReceiveAwakeInfo();
            BarsUpdate.CalculatePercentage();
            BarsWarnings.VerifyStatus();
        }

        private void OnPlayerConnected(object sender, PeerConnectedEventArgs e) =>
                     NetController.SyncSpecificPlayer(e.Peer.PlayerID);

        private void OnMessageReceived(object sender, ModMessageReceivedEventArgs e) =>
                     NetController.OnMessageReceived(e);

        private bool InitializeModMenu()
        {
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");

            if (configMenu is null)
            {
                return false;
            }

            else
            {
                // Register mod.
                configMenu.Register(
                    mod: ModManifest,
                    reset: () => Config = new Config(),
                    save: () => Helper.WriteConfig(Config)
                );

                #region Multiplier settings.

                // Title.
                configMenu.AddSectionTitle(
                    mod: ModManifest,
                    text: () => Helper.Translation.Get("Setting.Multiplier.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Multiplier.Description")
                );

                // Main hunger multiplier.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Multiplier.PassiveHunger.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Multiplier.PassiveHunger.Description"),
                    getValue: () => Config.HungerMultiplier,
                    setValue: value => Config.HungerMultiplier = value,
                    min: 0.0F,
                    max: 5.0F
                );

                // Main thirst multiplier.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Multiplier.PassiveThirst.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Multiplier.PassiveThirst.Description"),
                    getValue: () => Config.ThirstMultiplier,
                    setValue: value => Config.ThirstMultiplier = value,
                    min: 0.0F,
                    max: 5.0F
                );

                // Action hunger multiplier.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Multiplier.HungerOnAction.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Multiplier.HungerOnAction.Description"),
                    getValue: () => Config.HungerActionMultiplier,
                    setValue: value => Config.HungerActionMultiplier = value,
                    min: 0.0F,
                    max: 5.0F
                );

                // Action thirst multiplier.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Multiplier.ThirstOnAction.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Multiplier.ThirstOnAction.Description"),
                    getValue: () => Config.ThirstActionMultiplier,
                    setValue: value => Config.ThirstActionMultiplier = value,
                    min: 0.0F,
                    max: 5.0F
                );
                #endregion

                #region Bars options.

                // Title.
                configMenu.AddSectionTitle(
                    mod: ModManifest,
                    text: () => Helper.Translation.Get("Setting.BarsPosition.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.BarsPosition.Description")
                );

                // Bar positioning layout.
                configMenu.AddTextOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.BarsPosition.TypeOfPosition.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.BarsPosition.TypeOfPosition.Description"),
                    getValue: () => Config.BarsPosition,
                    setValue: value => Config.BarsPosition = value
                );

                // X position setting.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.BarsPosition.AxisX.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.BarsPosition.AxisX.Description"),
                    getValue: () => Config.BarsCustomX,
                    setValue: value => Config.BarsCustomX = value
                );

                // Y position setting.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.BarsPosition.AxisY.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.BarsPosition.AxisY.Description"),
                    getValue: () => Config.BarsCustomY,
                    setValue: value => Config.BarsCustomY = value
                );
                #endregion

                #region Compatibility settings.

                // Title.
                configMenu.AddSectionTitle(
                    mod: ModManifest,
                    text: () => Helper.Translation.Get("Setting.Compatibility.Title"), //TODO: Update translation.
                    tooltip: () => Helper.Translation.Get("Setting.Compatibility.Description")
                );

                // Food support bar.
                configMenu.AddBoolOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Compatibility.NonRecognizedFood.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Compatibility.NonRecognizedFood.Description"),
                    getValue: () => Config.NonSupportedFood,
                    setValue: value => Config.NonSupportedFood = value
                );
                #endregion

                #region Sleep options.

                // Title.
                configMenu.AddSectionTitle(
                    mod: ModManifest,
                    text: () => Helper.Translation.Get("Setting.Gameplay.SleepOptions.Title")
                );

                // Main Setting.
                configMenu.AddBoolOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Gameplay.SleepOptions.SleepDecrease.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Gameplay.SleepOptions.SleepDecrease.Description"),
                    getValue: () => Config.DecreaseValuesAfterSleep,
                    setValue: value => Config.DecreaseValuesAfterSleep = value
                );

                // Hunger.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Gameplay.SleepOptions.HungerDiff.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Gameplay.SleepOptions.HungerDiff.Description"),
                    getValue: () => Config.FoodDecreaseAfterSleep,
                    setValue: value => Config.FoodDecreaseAfterSleep = value,
                    min: -100,
                    max: 100
                );

                // Thirst.
                configMenu.AddNumberOption(
                    mod: ModManifest,
                    name: () => Helper.Translation.Get("Setting.Gameplay.SleepOptions.ThirstDiff.Title"),
                    tooltip: () => Helper.Translation.Get("Setting.Gameplay.SleepOptions.ThirstDiff.Description"),
                    getValue: () => Config.ThirstDecreaseAfterSleep,
                    setValue: value => Config.ThirstDecreaseAfterSleep = value,
                    min: -100,
                    max: 100
                );
                #endregion

                return true;
            }
        }
    }
}
