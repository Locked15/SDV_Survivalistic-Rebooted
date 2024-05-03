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
                                                             ModEntry.Config.AxeConsumption._hunger,
                                                             ModEntry.Config.AxeConsumption._thirst)) 
                },
                { 
                    "Pickaxe",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.PickAxeConsumption._hunger,
                                                             ModEntry.Config.PickAxeConsumption._thirst))
                },
                { 
                    "Hoe",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.HoeConsumption._hunger,
                                                             ModEntry.Config.HoeConsumption._thirst))
                },
                { 
                    "Scythe",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.HoeConsumption._hunger,
                                                             ModEntry.Config.HoeConsumption._thirst))
                },
                { 
                    "Fishing Rod",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.FishingRodConsumption._hunger,
                                                             ModEntry.Config.FishingRodConsumption._thirst))
                },
                { 
                    "Watering Can",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.WateringCanConsumption._hunger,
                                                             ModEntry.Config.WateringCanConsumption._thirst))
                },
                { 
                    "Shears",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.ShearsConsumption._hunger,
                                                             ModEntry.Config.ShearsConsumption._thirst))
                },
                { 
                    "Milk Pail",
                    NormalizeConsumptionString(string.Format(ToolConsumptionDetailStringTemplate,
                                                             ModEntry.Config.MilkPailConsumption._hunger,
                                                             ModEntry.Config.MilkPailConsumption._thirst))
                }
            };
        }

        private static string NormalizeConsumptionString(string rawString) => rawString.Replace(',', '.');
    }
}
