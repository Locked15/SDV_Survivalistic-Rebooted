using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;

namespace Survivalistic_Rebooted.Framework.Bars
{
    public static class BarsPosition
    {
        public static Vector2 BarPosition;

        private static string _currentLocation;

        private static Vector2 _sizeUI;

        public static void SetBarsPosition()
        {
            if (!Context.IsWorldReady) return;

            _sizeUI = new Vector2(Game1.uiViewport.Width, Game1.uiViewport.Height);
            _currentLocation = Game1.player.currentLocation.Name;

            switch (ModEntry.Config.BarsPosition)
            {
                case "bottom-right":
                    BarPosition.X = GetXPositionForRightBottomCorner();
                    BarPosition.Y = _sizeUI.Y;

                    BarsDatabase.RightSide = true;
                    break;

                case "bottom-left":
                    BarPosition.X = 70;
                    BarPosition.Y = _sizeUI.Y;

                    BarsDatabase.RightSide = false;
                    break;

                case "middle-right":
                    BarPosition.X = _sizeUI.X - 56;
                    BarPosition.Y = (_sizeUI.Y / 2) + 75;

                    BarsDatabase.RightSide = true;
                    break;

                case "middle-left":
                    BarPosition.X = 70;
                    BarPosition.Y = (_sizeUI.Y / 2) + 75;

                    BarsDatabase.RightSide = false;
                    break;

                case "top-right":
                    BarPosition.X = _sizeUI.X - 365;
                    if (Game1.player.buffs.AppliedBuffs.Count > 0) BarPosition.Y = 325;
                    else BarPosition.Y = 290;

                    BarsDatabase.RightSide = true;
                    break;

                case "top-left":
                    BarPosition.X = 70;
                    if (CheckIfPlayerInDangerLocation()) BarPosition.Y = 320;
                    else BarPosition.Y = 260;

                    BarsDatabase.RightSide = false;
                    break;

                default:
                    BarPosition.X = ModEntry.Config.BarsCustomX;
                    BarPosition.X = ModEntry.Config.BarsCustomY;

                    BarsDatabase.RightSide = BarPosition.X >= _sizeUI.X / 2;
                    break;
            }
        }

        /// <summary>
        /// Cause right bottom corner contains a lot of dynamic bars, so I moved this logic to this function.
        /// </summary>
        /// <returns>Position on 'X' axis.</returns>
        private static float GetXPositionForRightBottomCorner()
        {
            bool inDangerous = CheckIsPlayerInDangerous();
            return inDangerous ? _sizeUI.X - 171 : _sizeUI.X - 116;
        }

        private static bool CheckIsPlayerInDangerous() =>
                            Game1.showingHealth;

        private static bool CheckIfPlayerInDangerLocation() =>
                            _currentLocation.Contains("UndergroundMine") || _currentLocation.Contains("SkullCavern") ||
                            (_currentLocation.Contains("VolcanoDungeon") && _currentLocation != "VolcanoDungeon0");
    }
}
