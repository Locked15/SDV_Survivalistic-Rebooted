namespace Survivalistic_Rebooted.Framework.UI.Bars
{
    public static class RuntimeAdditionalProperties
    {
        public static float HungerVelocity
        {
            get => ModEntry.Config.PassiveHungerMultiplier;
        }

        public static float ThirstVelocity
        {
            get => ModEntry.Config.PassiveThirstMultiplier;
        }

        public static bool IsCurrentRenderTargetedToTheRightSide = false;

        public static bool ShouldRenderNumericalHungerValuesInCurrentCase = false;

        public static bool ShouldRenderNumericalThirstValuesInCurrentCase = false;

        /// <summary>
        /// This modifier applies depending on your current status.
        /// If your stats are low, tools are much (2.5F) more expensive to use.
        /// </summary>
        public static float AdditionalPostEffectToolUseMultiplier = 0.25f;
    }
}
