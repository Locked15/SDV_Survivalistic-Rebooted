using StardewModdingAPI;
using Survivalistic_Rebooted.Framework.APIs;
using Survivalistic_Rebooted.Models;

namespace Survivalistic_Rebooted.Framework.Misc
{
    public class ConfigMenuInitializer
    {
        public Config ActualConfig { get; private set; }

        private readonly IManifest _modManifest;

        private readonly IModHelper _helper;

        private readonly IGenericModConfigMenuApi _configMenu;

        public ConfigMenuInitializer(IManifest modManifest, IModHelper helper, Config actualConfig, IGenericModConfigMenuApi configMenuInstance)
        {
            _modManifest = modManifest;
            _helper = helper;
            ActualConfig = actualConfig;

            _configMenu = configMenuInstance;
        }

        public bool InitializeModMenu()
        {
            if (_configMenu is null) return false;

            _configMenu.Register(
                mod: _modManifest,
                reset: () => ActualConfig = new(),
                save: () => _helper.WriteConfig(ActualConfig)
            );

            AddMultiplierSettings();
            AddBarsPositioningSettings();
            AddCompatibilitySettings();
            AddGameplaySettings();

            return true;
        }

        private void AddMultiplierSettings()
        {
            _configMenu.AddSectionTitle(
                mod: _modManifest,
                text: () => _helper.Translation.Get("Setting.Multiplier.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.Description")
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Multiplier.PassiveHunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.PassiveHunger.Description"),
                getValue: () => ActualConfig.HungerMultiplier,
                setValue: value => ActualConfig.HungerMultiplier = value,
                min: 0.0F,
                max: 5.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Multiplier.PassiveThirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.PassiveThirst.Description"),
                getValue: () => ActualConfig.ThirstMultiplier,
                setValue: value => ActualConfig.ThirstMultiplier = value,
                min: 0.0F,
                max: 5.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Multiplier.HungerOnAction.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.HungerOnAction.Description"),
                getValue: () => ActualConfig.HungerActionMultiplier,
                setValue: value => ActualConfig.HungerActionMultiplier = value,
                min: 0.0F,
                max: 5.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Multiplier.ThirstOnAction.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.ThirstOnAction.Description"),
                getValue: () => ActualConfig.ThirstActionMultiplier,
                setValue: value => ActualConfig.ThirstActionMultiplier = value,
                min: 0.0F,
                max: 5.0F
            );
        }

        private void AddBarsPositioningSettings()
        {
            _configMenu.AddSectionTitle(
                mod: _modManifest,
                text: () => _helper.Translation.Get("Setting.BarsPosition.Title"),
                tooltip: () => _helper.Translation.Get("Setting.BarsPosition.Description")
            );

            _configMenu.AddTextOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.BarsPosition.TypeOfPosition.Title"),
                tooltip: () => _helper.Translation.Get("Setting.BarsPosition.TypeOfPosition.Description"),
                getValue: () => ActualConfig.BarsPosition,
                setValue: value => ActualConfig.BarsPosition = value,
                allowedValues: Config.PossibleBarLocations.ToArray()
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.BarsPosition.AxisX.Title"),
                tooltip: () => _helper.Translation.Get("Setting.BarsPosition.AxisX.Description"),
                getValue: () => ActualConfig.BarsCustomX,
                setValue: value => ActualConfig.BarsCustomX = value
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.BarsPosition.AxisY.Title"),
                tooltip: () => _helper.Translation.Get("Setting.BarsPosition.AxisY.Description"),
                getValue: () => ActualConfig.BarsCustomY,
                setValue: value => ActualConfig.BarsCustomY = value
            );
        }

        private void AddCompatibilitySettings()
        {
            _configMenu.AddSectionTitle(
                mod: _modManifest,
                text: () => _helper.Translation.Get("Setting.Compatibility.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Compatibility.Description")
            );

            _configMenu.AddBoolOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Compatibility.NonRecognizedFood.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Compatibility.NonRecognizedFood.Description"),
                getValue: () => ActualConfig.NonSupportedFood,
                setValue: value => ActualConfig.NonSupportedFood = value
            );
        }

        private void AddGameplaySettings()
        {
            _configMenu.AddSectionTitle(
                mod: _modManifest,
                text: () => _helper.Translation.Get("Setting.Gameplay.SleepOptions.Title")
            );

            _configMenu.AddBoolOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Gameplay.SleepOptions.SleepDecrease.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Gameplay.SleepOptions.SleepDecrease.Description"),
                getValue: () => ActualConfig.DecreaseValuesAfterSleep,
                setValue: value => ActualConfig.DecreaseValuesAfterSleep = value
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Gameplay.SleepOptions.HungerDiff.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Gameplay.SleepOptions.HungerDiff.Description"),
                getValue: () => ActualConfig.FoodDecreaseAfterSleep,
                setValue: value => ActualConfig.FoodDecreaseAfterSleep = value,
                min: -100,
                max: 100
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Gameplay.SleepOptions.ThirstDiff.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Gameplay.SleepOptions.ThirstDiff.Description"),
                getValue: () => ActualConfig.ThirstDecreaseAfterSleep,
                setValue: value => ActualConfig.ThirstDecreaseAfterSleep = value,
                min: -100,
                max: 100
            );
        }
    }
}
