using Survivalistic_Rebooted.Framework.Common.Affection;
using Survivalistic_Rebooted.Framework.UI.Bars;

namespace Survivalistic_Rebooted.Framework.Common
{
    internal static class Helper
    {
        public static void PerformBenefitsPenaltiesRoutineApplying()
        {
            if (!Benefits.VerifyStatus())
                Penalties.VerifyStatus();
        }

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