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

        // GAMEPLAY SETTINGS

        public bool NonSupportedFood { get; set; }

        public bool DecreaseValuesAfterSleep { get; set; }

        public int FoodDecreaseAfterSleep { get; set; }

        public int ThirstDecreaseAfterSleep { get; set; }

        // MISCELLANEOUS

        public bool ShowPopUpWithRestorationValues { get; set; } = true;
    }
}
