namespace Survivalistic_Rebooted.Framework.UI.Bars
{
    public static class BarsUpdate
    {
        public static void UpdateBarsProperties()
        {
            if (ModEntry.Data.ActualHunger > 0) ModEntry.Data.ActualHunger -= BarsDatabase.HungerVelocity;
            else ModEntry.Data.ActualHunger = 0;

            if (ModEntry.Data.ActualThirst > 0) ModEntry.Data.ActualThirst -= BarsDatabase.ThirstVelocity;
            else ModEntry.Data.ActualThirst = 0;
        }

        public static void CalculatePercentage()
        {
            BarsProperties.HungerPercentage = ModEntry.Data.ActualHunger / ModEntry.Data.MaxHunger * 168;
            BarsProperties.ThirstPercentage = ModEntry.Data.ActualThirst / ModEntry.Data.MaxThirst * 168;
        }
    }
}
