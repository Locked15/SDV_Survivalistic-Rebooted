using StardewModdingAPI;
using Survivalistic_Rebooted.Framework.APIs;
using Survivalistic_Rebooted.Models.Config;

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
            AddBarsColorSettings();
            AddCompatibilitySettings();
            AddGameplaySettings();
            AddToolsConsumptionSettings();
            AddMiscellaneousSettings();

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
                getValue: () => ActualConfig.PassiveHungerMultiplier,
                setValue: value => ActualConfig.PassiveHungerMultiplier = value,
                min: 0.0F,
                max: 5.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Multiplier.PassiveThirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.PassiveThirst.Description"),
                getValue: () => ActualConfig.PassiveThirstMultiplier,
                setValue: value => ActualConfig.PassiveThirstMultiplier = value,
                min: 0.0F,
                max: 5.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Multiplier.HungerOnAction.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.HungerOnAction.Description"),
                getValue: () => ActualConfig.HungerOnActionMultiplier,
                setValue: value => ActualConfig.HungerOnActionMultiplier = value,
                min: 0.0F,
                max: 5.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Multiplier.ThirstOnAction.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Multiplier.ThirstOnAction.Description"),
                getValue: () => ActualConfig.ThirstOnActionMultiplier,
                setValue: value => ActualConfig.ThirstOnActionMultiplier = value,
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

        private void AddBarsColorSettings()
        {
            _configMenu.AddSectionTitle(
                mod: _modManifest,
                text: () => _helper.Translation.Get("Setting.BarsColor.Title")
            );

            _configMenu.AddBoolOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.BarsColor.DynamicHungerColor.Title"),
                tooltip: () => _helper.Translation.Get("Setting.BarsColor.DynamicHungerColor.Description"),
                getValue: () => ActualConfig.UseDynamicHungerBarColor,
                setValue: value => ActualConfig.UseDynamicHungerBarColor = value
            );

            _configMenu.AddBoolOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.BarsColor.DynamicThirstColor.Title"),
                tooltip: () => _helper.Translation.Get("Setting.BarsColor.DynamicThirstColor.Description"),
                getValue: () => ActualConfig.UseDynamicThirstBarColor,
                setValue: value => ActualConfig.UseDynamicThirstBarColor = value
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
                name: () => _helper.Translation.Get("Setting.Compatibility.ApplyPropertiesToNonRecognizedFood.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Compatibility.ApplyPropertiesToNonRecognizedFood.Description"),
                getValue: () => ActualConfig.ApplyPropertiesToNonRecognizedFood,
                setValue: value => ActualConfig.ApplyPropertiesToNonRecognizedFood = value
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

        private void AddToolsConsumptionSettings()
        {
            _configMenu.AddSectionTitle(
                mod: _modManifest,
                text: () => _helper.Translation.Get("Setting.ToolsConsumption.Title")
            );

            // Axe:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Axe.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Axe.Hunger.Description"),
                getValue: () => ActualConfig.AxeConsumption.Hunger,
                setValue: value => ActualConfig.AxeConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Axe.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Axe.Thirst.Description"),
                getValue: () => ActualConfig.AxeConsumption.Thirst,
                setValue: value => ActualConfig.AxeConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );

            // Pickaxe:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Pickaxe.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Pickaxe.Hunger.Description"),
                getValue: () => ActualConfig.PickAxeConsumption.Hunger,
                setValue: value => ActualConfig.PickAxeConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Pickaxe.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Pickaxe.Thirst.Description"),
                getValue: () => ActualConfig.PickAxeConsumption.Thirst,
                setValue: value => ActualConfig.PickAxeConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );

            // Hoe:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Hoe.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Hoe.Hunger.Description"),
                getValue: () => ActualConfig.HoeConsumption.Hunger,
                setValue: value => ActualConfig.HoeConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Hoe.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Hoe.Thirst.Description"),
                getValue: () => ActualConfig.HoeConsumption.Thirst,
                setValue: value => ActualConfig.HoeConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );

            // Scythe:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Scythe.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Scythe.Hunger.Description"),
                getValue: () => ActualConfig.ScytheConsumption.Hunger,
                setValue: value => ActualConfig.ScytheConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Scythe.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Scythe.Thirst.Description"),
                getValue: () => ActualConfig.ScytheConsumption.Thirst,
                setValue: value => ActualConfig.ScytheConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );

            // Fishing Rod:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.FishingRod.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.FishingRod.Hunger.Description"),
                getValue: () => ActualConfig.FishingRodConsumption.Hunger,
                setValue: value => ActualConfig.FishingRodConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.FishingRod.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.FishingRod.Thirst.Description"),
                getValue: () => ActualConfig.FishingRodConsumption.Thirst,
                setValue: value => ActualConfig.FishingRodConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );

            // Watering Can:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.WateringCan.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.WateringCan.Hunger.Description"),
                getValue: () => ActualConfig.WateringCanConsumption.Hunger,
                setValue: value => ActualConfig.WateringCanConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.WateringCan.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.WateringCan.Thirst.Description"),
                getValue: () => ActualConfig.WateringCanConsumption.Thirst,
                setValue: value => ActualConfig.WateringCanConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );

            // Shears:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Shears.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Shears.Hunger.Description"),
                getValue: () => ActualConfig.ShearsConsumption.Hunger,
                setValue: value => ActualConfig.ShearsConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.Shears.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.Shears.Thirst.Description"),
                getValue: () => ActualConfig.ShearsConsumption.Thirst,
                setValue: value => ActualConfig.ShearsConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );

            // Milk Pail:
            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.MilkPail.Hunger.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.MilkPail.Hunger.Description"),
                getValue: () => ActualConfig.MilkPailConsumption.Hunger,
                setValue: value => ActualConfig.MilkPailConsumption.Hunger = value,
                min: 0.0F,
                max: 2.0F
            );

            _configMenu.AddNumberOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.ToolsConsumption.MilkPail.Thirst.Title"),
                tooltip: () => _helper.Translation.Get("Setting.ToolsConsumption.MilkPail.Thirst.Description"),
                getValue: () => ActualConfig.MilkPailConsumption.Thirst,
                setValue: value => ActualConfig.MilkPailConsumption.Thirst = value,
                min: 0.0F,
                max: 2.0F
            );
        }

        private void AddMiscellaneousSettings()
        {
            _configMenu.AddSectionTitle(
                mod: _modManifest,
                text: () => _helper.Translation.Get("Setting.Miscellaneous.Title")
            );

            _configMenu.AddBoolOption(
                mod: _modManifest,
                name: () => _helper.Translation.Get("Setting.Miscellaneous.ShowPopUpMessages.Title"),
                tooltip: () => _helper.Translation.Get("Setting.Miscellaneous.ShowPopUpMessages.Description"),
                getValue: () => ActualConfig.ShowPopUpWithRestorationValues,
                setValue: value => ActualConfig.ShowPopUpWithRestorationValues = value
            );
        }
    }
}
