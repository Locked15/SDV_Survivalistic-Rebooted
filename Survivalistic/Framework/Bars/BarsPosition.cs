using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;

namespace Survivalistic_Rebooted.Framework.Bars
{
    public static class BarsPosition
    {
        public static Vector2 barPosition;

        private static Vector2 sizeUI;

        private static string _currentLocation;

        public static void SetBarsPosition()
        {
            if (!Context.IsWorldReady) return;

            sizeUI = new Vector2(Game1.uiViewport.Width, Game1.uiViewport.Height);
            _currentLocation = Game1.player.currentLocation.Name;

            switch (ModEntry.Config.BarsPosition)
            {
                case "bottom-right":
                    barPosition.X = GetPositionInRightBottomCorner();
                    barPosition.Y = sizeUI.Y;
                    BarsDatabase.RightSide = true;
                    break;

                case "bottom-left":
                    barPosition.X = 70;
                    barPosition.Y = sizeUI.Y;
                    BarsDatabase.RightSide = false;
                    break;

                case "middle-right":
                    barPosition.X = sizeUI.X - 56;
                    barPosition.Y = (sizeUI.Y / 2) + 75;
                    BarsDatabase.RightSide = true;
                    break;

                case "middle-left":
                    barPosition.X = 70;
                    barPosition.Y = (sizeUI.Y / 2) + 75;
                    BarsDatabase.RightSide = false;
                    break;

                case "top-right":
                    barPosition.X = sizeUI.X - 365;
                    if (Game1.player.buffs.AppliedBuffs.Count > 0) barPosition.Y = 325;
                    else barPosition.Y = 290;
                    BarsDatabase.RightSide = true;
                    break;

                case "top-left":
                    barPosition.X = 70;
                    if (CheckCavernLevelIsVisible(_currentLocation)) barPosition.Y = 320;
                    else barPosition.Y = 260;
                    BarsDatabase.RightSide = false;
                    break;

                case "custom":
                    barPosition.X = ModEntry.Config.BarsCustomX;
                    barPosition.X = ModEntry.Config.BarsCustomY;
                    BarsDatabase.RightSide = barPosition.X >= sizeUI.X / 2;
                    break;
            }
        }

        /// <summary>
        /// Cause right bottom corner contains a lot of dynamic bars, so I moved this logic to this function.
        /// </summary>
        /// <returns>Position on 'X' axis.</returns>
        private static float GetPositionInRightBottomCorner()
        {
            #region Used variables.

            float position;

            bool inDangerous = CheckToDangerous();
            bool ultimateIsVisible = false;
            #endregion

            // Player is Safe, Ultimate isn't Visible.
            if (!inDangerous && !ultimateIsVisible)
                position = sizeUI.X - 116;

            // Player is Safe, Ultimate is Visible.
            else if (!inDangerous && ultimateIsVisible)
                position = sizeUI.X - 171;

            // Player isn't Safe, Ultimate isn't Visible.
            else if (inDangerous && !ultimateIsVisible)
                position = sizeUI.X - 171;

            // Player isn't Safe, Ultimate is Visible.
            else
                position = sizeUI.X - 226;

            return position;

        }

        private static bool CheckToDangerous() =>
                            Game1.showingHealth;

        private static bool CheckCavernLevelIsVisible(string locationName) =>
                            _currentLocation.Contains("UndergroundMine") || _currentLocation.Contains("SkullCavern") ||
                            (_currentLocation.Contains("VolcanoDungeon") && _currentLocation != "VolcanoDungeon0");
    }
}
