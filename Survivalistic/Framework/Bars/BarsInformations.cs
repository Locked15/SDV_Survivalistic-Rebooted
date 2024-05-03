using Framework.Common;
using Microsoft.Xna.Framework;
using System;

namespace Survivalistic_Rebooted.Framework.Bars
{
    public class BarsInformations
    {
        public static float HungerPercentage { get; set; }

        public static float ThirstPercentage { get; set; }

        private static Color _hungerBarColor = new(1, .7f, 0);

        private static Color _thirstBarColor = new(0, .7f, 1);

        public static Color GetHungerBarColorWithOffset()
        {
            double maxHunger = ModEntry.Data.MaxHunger * 1.0;
            double currentHunger = ModEntry.Data.ActualHunger * 1.0;
            double offset = currentHunger / maxHunger;

            Color color = _hungerBarColor;
            if (ModEntry.Config.UseDynamicHungerBarColor)
            {
                unchecked
                {
                    color = ApplyColorOffset(offset, color);
                }
            }

            return color;
        }

        public static Color GetThirstBarColorWithOffset()
        {
            Helper.NormalizeStatus();

            double maxThirsty = ModEntry.Data.MaxThirst * 1.0;
            double currentThirsty = ModEntry.Data.ActualThirst * 1.0;
            double offset = currentThirsty / maxThirsty;

            Color color = _thirstBarColor;
            if (ModEntry.Config.UseDynamicHungerBarColor)
            {
                unchecked
                {
                    color = ApplyColorOffset(offset, color);
                }
            }

            return color;
        }

        private static Color ApplyColorOffset(double offset, Color color)
        {
            color.R = Convert.ToByte(Math.Abs(offset - 1) * byte.MaxValue);
            color.G = Convert.ToByte(offset * color.G);
            color.B = Convert.ToByte(offset * color.B);

            return color;
        }
    }
}
