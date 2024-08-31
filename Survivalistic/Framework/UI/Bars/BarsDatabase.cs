namespace Survivalistic_Rebooted.Framework.UI.Bars
{
    public static class BarsDatabase
    {
        public static float HungerVelocity = ModEntry.Config.PassiveHungerMultiplier;
        public static float ThirstVelocity = ModEntry.Config.PassiveThirstMultiplier;

        public static bool RenderNumericalHunger = false;
        public static bool RenderNumericalThirst = false;

        public static bool RightSide = false;

        /// <summary>
        /// This modifier applies depending on your current status.
        /// If your stats are low, tools are much (2.5F) more expensive to use.
        /// </summary>
        public static float AdditionalPostEffectToolUseMultiplier = 0.25f;
    }
}
