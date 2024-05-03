using StardewValley;
using Survivalistic_Rebooted.Models;

namespace Survivalistic_Rebooted.Framework.Common.Effects
{
    public static class Buffs
    {
        public static void SetBuff(string id)
        {
            var buffToApply = SurvivalisticBuffs.GetBuffCopyByID(id);
            if (buffToApply == null || Game1.player.hasBuff(id))
                return;

            Game1.player.applyBuff(buffToApply);
        }

        public static void RemoveBuff(string id) => Game1.player.buffs.Remove(id);

        public static void CallUpdateSettingBuff(string buffID, bool remove = false)
        {
            if (!remove && !Game1.player.hasBuff(buffID))
                SetBuff(buffID);
            else if (remove && Game1.player.hasBuff(buffID))
                RemoveBuff(buffID);
        }
    }
}
