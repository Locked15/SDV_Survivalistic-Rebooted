using StardewModdingAPI;
using Survivalistic_Rebooted.Framework.Common.Effects;

namespace Survivalistic_Rebooted.Framework.Common.Affection
{
    public static class Benefits
    {
        public static bool VerifyBenefits()
        {
            if (!Context.IsWorldReady)
                return false;

            var hungerSate = ProceedHungerCheck();
            var thirstSate = ProceedThirstCheck();

            return hungerSate && thirstSate;
        }

        private static bool ProceedHungerCheck()
        {
            var result = true;
            if (ModEntry.Data.ActualHunger >= 80)
            {
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.FullnessBuff));
            }
            else
            {
                result = ModEntry.Data.ActualHunger > 30;
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.FullnessBuff), true);
            }

            return result;
        }

        private static bool ProceedThirstCheck()
        {
            var result = true;
            if (ModEntry.Data.ActualThirst >= 80)
            {
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.HydratedBuff));
            }
            else
            {
                result = ModEntry.Data.ActualThirst > 30;
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.HydratedBuff), true);
            }

            return result;
        }
    }
}
