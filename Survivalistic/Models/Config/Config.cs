using System.Collections.Generic;
using System.Linq;

namespace Survivalistic_Rebooted.Models.Config
{
    public class Config
    {
        public static List<string> PossibleBarLocations { get; } = new()
        {
            "bottom-right",
            "bottom-left",
            "middle-right",
            "middle-left",
            "top-left",
            "top-right",
            "custom"
        };

        // MULTIPLIERS
        public float PassiveHungerMultiplier { get; set; } = 1f;

        public float PassiveThirstMultiplier { get; set; } = 0.5f;

        public float HungerOnActionMultiplier { get; set; } = 1f;

        public float ThirstOnActionMultiplier { get; set; } = 1f;

        // BARS POSITION
        public string BarsPosition { get; set; } = PossibleBarLocations.First();

        public int BarsCustomX { get; set; } = 0;

        public int BarsCustomY { get; set; } = 0;

        // BARS COLOR
        public bool UseDynamicHungerBarColor { get; set; } = true;

        public bool UseDynamicThirstBarColor { get; set; } = true;

        // COMPATIBILITY SETTINGS
        public bool ApplyPropertiesToNonRecognizedFood { get; set; } = true;

        // GAMEPLAY SETTINGS
        public bool DecreaseValuesAfterSleep { get; set; }

        public int FoodDecreaseAfterSleep { get; set; }

        public int ThirstDecreaseAfterSleep { get; set; }

        // TOOLS CONSUMPTION RATE
        public Needs AxeConsumption { get; set; } = (0.50F, 0.25F);

        public Needs PickAxeConsumption { get; set; } = (0.50F, 0.25F);

        public Needs HoeConsumption { get; set; } = (0.50F, 0.25F);

        public Needs ScytheConsumption { get; set; } = (0.10F, 0.20F);

        public Needs FishingRodConsumption { get; set; } = (0.15F, 0.30F);

        public Needs WateringCanConsumption { get; set; } = (0.10F, 0.20F);

        public Needs ShearsConsumption { get; set; } = (0.15F, 0.30F);

        public Needs MilkPailConsumption { get; set; } = (0.15F, 0.30F);

        // MISCELLANEOUS
        public bool ShowPopUpWithRestorationValues { get; set; } = true;
    }
}
