namespace Survivalistic_Rebooted.Framework.Bars
{
    public static class BarsDatabase
    {
        public static float HungerVelocity = ModEntry.Config.PassiveHungerMultiplier;
        public static float ThirstVelocity = ModEntry.Config.PassiveThirstMultiplier;

        public static bool RenderNumericalHunger = false;
        public static bool RenderNumericalThirst = false;

        public static bool RightSide = false;

        public static float ToolUseMultiplier = 0.25f;
    }
}
