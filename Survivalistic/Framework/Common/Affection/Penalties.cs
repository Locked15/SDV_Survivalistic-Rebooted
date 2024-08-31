using Survivalistic_Rebooted.Framework.UI.Bars;
using StardewModdingAPI;
using StardewValley;
using Survivalistic_Rebooted.Framework.Common.Effects;
using Survivalistic_Rebooted.Framework.Misc;

namespace Survivalistic_Rebooted.Framework.Common.Affection
{
    public static class Penalties
    {
        private static bool _alreadyCheckedFaint;

        public static void VerifyStatus()
        {
            if (!Context.IsWorldReady) return;

            if (ModEntry.Data.ActualHunger <= 15 && ModEntry.Data.ActualHunger > 0) BarsDatabase.AdditionalPostEffectToolUseMultiplier = 1.5f;
            else if (ModEntry.Data.ActualHunger <= 0) BarsDatabase.AdditionalPostEffectToolUseMultiplier = 2.5f;
            else BarsDatabase.AdditionalPostEffectToolUseMultiplier = 1;

            if (ModEntry.Data.ActualThirst <= 15 && ModEntry.Data.ActualThirst > 0) BarsDatabase.AdditionalPostEffectToolUseMultiplier = 1.5f;
            else if (ModEntry.Data.ActualThirst <= 0) BarsDatabase.AdditionalPostEffectToolUseMultiplier = 2.5f;
            else BarsDatabase.AdditionalPostEffectToolUseMultiplier = 1;

            CheckValuesAndDealDamageIfReady();
        }

        public static void CheckValuesAndDealDamageIfReady()
        {
            if (!Context.IsWorldReady) return;

            bool applyingHealthDamage = ProceedHungerWork() | ProceedThirstWork();
            TryToProceedExhaustedPenalties(applyingHealthDamage);
        }

        private static bool ProceedHungerWork()
        {
            var applyHPDamage = false;
            if (ModEntry.Data.ActualHunger <= 10)
            {
                applyHPDamage = ApplyDamageToStaminaOrHP();
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.HungerDeBuff));
            }
            else
            {
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.HungerDeBuff), true);
            }

            return applyHPDamage;
        }

        private static bool ProceedThirstWork()
        {
            var applyHPDamage = false;
            if (ModEntry.Data.ActualThirst <= 10)
            {
                applyHPDamage = ApplyDamageToStaminaOrHP();
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.ThirstDeBuff));
            }
            else
            {
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.ThirstDeBuff), true);
            }

            return applyHPDamage;
        }

        private static void TryToProceedExhaustedPenalties(bool applyingHealthDamage)
        {
            if (applyingHealthDamage)
            {
                Game1.player.checkForExhaustion(Game1.player.Stamina);
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.FaintDeBuff));
            }
            else
            {
                Buffs.CallUpdateSettingBuff(BuffsHelper.GetBuffIDByCode(Models.SurvivalisticBuffs.Codes.FaintDeBuff), true);
            }
        }

        private static bool ApplyDamageToStaminaOrHP()
        {
            var applyHPDamage = false;
            if (Game1.player.stamina > 0)
            {
                Game1.player.stamina -= 15;
            }
            else
            {
                Game1.player.health -= 10;
                applyHPDamage = true;
            }

            return applyHPDamage;
        }

        public static void VerifyPassOut()
        {
            if (Game1.player.health <= 0)
            {
                if (!_alreadyCheckedFaint)
                {
                    ModEntry.Data.ActualHunger = ModEntry.Data.MaxHunger / 3;
                    ModEntry.Data.ActualThirst = ModEntry.Data.MaxThirst / 2;

                    NetController.Sync();

                    _alreadyCheckedFaint = true;
                }
            }
            else
            {
                _alreadyCheckedFaint = false;
            }
        }
    }
}
