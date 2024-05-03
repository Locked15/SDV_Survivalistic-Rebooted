using System.Collections.Generic;

namespace Survivalistic_Rebooted.Framework.Databases
{
    public static class Tools
    {
        private const string ToolConsumptionDetailStringTemplate = "{0}/{1}";

        public static Dictionary<string, string> GetToolDatabase()
        {
            return new()
            {
                {
                    "Axe",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.AxeConsumption.Hunger,
                                                             ModEntry.Config.AxeConsumption.Thirst))
                },
                {
                    "Pickaxe",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.PickAxeConsumption.Hunger,
                                                             ModEntry.Config.PickAxeConsumption.Thirst))
                },
                {
                    "Hoe",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.HoeConsumption.Hunger,
                                                             ModEntry.Config.HoeConsumption.Thirst))
                },
                {
                    "Scythe",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.HoeConsumption.Hunger,
                                                             ModEntry.Config.HoeConsumption.Thirst))
                },
                {
                    "Fishing Rod",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.FishingRodConsumption.Hunger,
                                                             ModEntry.Config.FishingRodConsumption.Thirst))
                },
                {
                    "Watering Can",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.WateringCanConsumption.Hunger,
                                                             ModEntry.Config.WateringCanConsumption.Thirst))
                },
                {
                    "Shears",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.ShearsConsumption.Hunger,
                                                             ModEntry.Config.ShearsConsumption.Thirst))
                },
                {
                    "Milk Pail",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.MilkPailConsumption.Hunger,
                                                             ModEntry.Config.MilkPailConsumption.Thirst))
                }
            };
        }

        private static string NormalizeConsumptionString(string rawString) => rawString.Replace(',', '.');
    }
}
