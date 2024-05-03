using Survivalistic_Rebooted;
using Survivalistic_Rebooted.Framework.Bars;

namespace Framework.Common
{
    internal static class Helper
    {

        public static void NormalizeStatus()
        {
            if (ModEntry.Data.ActualHunger < 0) ModEntry.Data.ActualHunger = 0;
            if (ModEntry.Data.ActualThirst < 0) ModEntry.Data.ActualThirst = 0;

            if (ModEntry.Data.ActualHunger > ModEntry.Data.MaxHunger) ModEntry.Data.ActualHunger = ModEntry.Data.MaxHunger;
            if (ModEntry.Data.ActualThirst > ModEntry.Data.MaxThirst) ModEntry.Data.ActualThirst = ModEntry.Data.MaxThirst;

            BarsUpdate.CalculatePercentage();
        }

        public static void ResetStatus()
        {
            ModEntry.Data.ActualHunger = ModEntry.Data.MaxHunger;
            ModEntry.Data.ActualThirst = ModEntry.Data.MaxThirst;

            BarsUpdate.CalculatePercentage();
        }
    }
}