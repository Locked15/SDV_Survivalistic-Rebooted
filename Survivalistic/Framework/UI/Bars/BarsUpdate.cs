namespace Survivalistic_Rebooted.Framework.UI.Bars
{
    public static class BarsUpdate
    {
        public static void CalculatePercentage()
        {
            BarsProperties.HungerPercentage = ModEntry.Data.ActualHunger / ModEntry.Data.MaxHunger * 168;
            BarsProperties.ThirstPercentage = ModEntry.Data.ActualThirst / ModEntry.Data.MaxThirst * 168;
        }
    }
}
