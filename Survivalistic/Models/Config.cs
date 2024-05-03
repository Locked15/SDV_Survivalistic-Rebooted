namespace Survivalistic_Rebooted.Models
{
    public class Config
    {
        // MULTIPLIERS
        public float ThirstMultiplier { get; set; } = 0.5f;

        public float HungerMultiplier { get; set; } = 1f;

        public float HungerActionMultiplier { get; set; } = 1f;

        public float ThirstActionMultiplier { get; set; } = 1f;

        // BARS POSITION
        public string BarsPosition { get; set; } = "bottom-right";

        public int BarsCustomX { get; set; } = 0;

        public int BarsCustomY { get; set; } = 0;

        // BAR COLORS
        public bool UseDynamicHungerBarColor { get; set; } = true;

        public bool UseDynamicThirstBarColor { get; set; } = true;

        // COMPATIBILITY SETTINGS
        public bool NonSupportedFood { get; set; } = true;

        // GAMEPLAY SETTINGS
        public bool DecreaseValuesAfterSleep { get; set; }

        public int FoodDecreaseAfterSleep { get; set; }

        public int ThirstDecreaseAfterSleep { get; set; }

        // TOOLS CONSUMPTION RATE
        public (float _hunger, float _thirst) AxeConsumption { get; set; } = (0.50F, 0.25F);

        public (float _hunger, float _thirst) PickAxeConsumption { get; set; } = (0.50F, 0.25F);

        public (float _hunger, float _thirst) HoeConsumption { get; set; } = (0.50F, 0.25F);

        public (float _hunger, float _thirst) ScytheConsumption { get; set; } = (0.10F, 0.20F);

        public (float _hunger, float _thirst) FishingRodConsumption { get; set; } = (0.15F, 0.30F);

        public (float _hunger, float _thirst) WateringCanConsumption { get; set; } = (0.10F, 0.20F);

        public (float _hunger, float _thirst) ShearsConsumption { get; set; } = (0.15F, 0.30F);

        public (float _hunger, float _thirst) MilkPailConsumption { get; set; } = (0.15F, 0.30F);

        // MISCELLANEOUS
        public bool ShowPopUpWithRestorationValues { get; set; } = true;
    }
}
