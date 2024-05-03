using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Survivalistic_Rebooted.Framework.Bars;
using Survivalistic_Rebooted.Framework.Common;
using System;
using StardewValley.Menus;
using Survivalistic_Rebooted.Framework.Databases;
using System.Collections.Generic;
using System.Linq;

namespace Survivalistic_Rebooted.Framework.Rendering
{
    public static class Renderer
    {
        public static void OnRenderingHud(object sender, RenderingHudEventArgs e)
        {
            if (!Context.IsWorldReady || Game1.CurrentEvent != null) return;

            CheckMouseHovering();

            e.SpriteBatch.Draw(Textures.HungerSprite, new Rectangle((int)BarsPosition.barPosition.X, (int)BarsPosition.barPosition.Y - 240, Textures.HungerSprite.Width * 4, Textures.HungerSprite.Height * 4), Color.White);
            e.SpriteBatch.Draw(Textures.ThirstSprite, new Rectangle((int)BarsPosition.barPosition.X - 60, (int)BarsPosition.barPosition.Y - 240, Textures.ThirstSprite.Width * 4, Textures.ThirstSprite.Height * 4), Color.White);

            e.SpriteBatch.Draw(Textures.HungerFiller, new Vector2(BarsPosition.barPosition.X + 36, BarsPosition.barPosition.Y - 25), new Rectangle(0, 0, Textures.HungerFiller.Width * 6 * Game1.pixelZoom, (int)BarsInformations.HungerPercentage), BarsInformations.GetHungerBarColorWithOffset(), 3.138997f, new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None, 1f);
            e.SpriteBatch.Draw(Textures.ThirstFiller, new Vector2(BarsPosition.barPosition.X - 24, BarsPosition.barPosition.Y - 25), new Rectangle(0, 0, Textures.ThirstFiller.Width * 6 * Game1.pixelZoom, (int)BarsInformations.ThirstPercentage), BarsInformations.GetThirstBarColorWithOffset(), 3.138997f, new Vector2(0.5f, 0.5f), 1f, SpriteEffects.None, 1f);
        
            if (BarsDatabase.RenderNumericalHunger)
            {
                string information = $"{(int)ModEntry.Data.ActualHunger}/{(int)ModEntry.Data.MaxHunger}";
                Vector2 textSize = Game1.dialogueFont.MeasureString(information);
                Vector2 textPosition;
                if (BarsDatabase.RightSide) textPosition = new Vector2(-12, textSize.X);
                else textPosition = new Vector2(12 + (Textures.HungerSprite.Width * 4), 0);

                Game1.spriteBatch.DrawString(
                    Game1.dialogueFont,
                    information,
                    new Vector2(BarsPosition.barPosition.X + textPosition.X, BarsPosition.barPosition.Y - 240 + ((Textures.HungerSprite.Height * 4) / 4) + 8),
                    BarsInformations.GetHungerBarColorWithOffset(),
                    0f,
                    new Vector2(textPosition.Y, 0),
                    1,
                    SpriteEffects.None,
                    0f);
            }

            if (BarsDatabase.RenderNumericalThirst)
            {
                string information = $"{(int)ModEntry.Data.ActualThirst}/{(int)ModEntry.Data.MaxThirst}";
                Vector2 text_size = Game1.dialogueFont.MeasureString(information);
                Vector2 text_position;
                if (BarsDatabase.RightSide) text_position = new Vector2(-12, text_size.X);
                else text_position = new Vector2(12 + (Textures.HungerSprite.Width * 4), 0);

                Game1.spriteBatch.DrawString(
                    Game1.dialogueFont,
                    information,
                    new Vector2(BarsPosition.barPosition.X - 60 + text_position.X, BarsPosition.barPosition.Y - 240 + ((Textures.HungerSprite.Height * 4) / 4) + 8),
                    BarsInformations.GetThirstBarColorWithOffset(),
                    0f,
                    new Vector2(text_position.Y, 0),
                    1,
                    SpriteEffects.None,
                    0f);
            }

            if (Game1.player.ActiveObject != null)
            {
                if (Foods.FoodDatabase.TryGetValue(Game1.player.ActiveObject.Name, out string food_status_string))
                {
                    Vector2 sizeUI = new(Game1.uiViewport.Width, Game1.uiViewport.Height);
                    List<string> foodStatus = food_status_string.Split('/').ToList();

                    string actualString = "";
                    if (Int32.Parse(foodStatus[0]) > 0)
                        actualString += string.Format(ModEntry.Instance.Helper.Translation.Get("info-fullness"), foodStatus[0]);
                    if (Int32.Parse(foodStatus[0]) > 0 && Int32.Parse(foodStatus[1]) > 0)
                        actualString += "\n";
                    if (Int32.Parse(foodStatus[1]) > 0)
                        actualString += string.Format(ModEntry.Instance.Helper.Translation.Get("info-thirsty"), foodStatus[1]);

                    string currentText = actualString;
                    Vector2 textSize = Game1.smallFont.MeasureString(currentText);
                    SpriteBatch b = e.SpriteBatch;
                    IClickableMenu.drawTextureBox(b, Game1.menuTexture, new Rectangle(0, 256, 60, 60), (int)(sizeUI.X / 2) - (int)(textSize.X / 2 + 25), (int)(sizeUI.Y) - 125 - (int)(textSize.Y + 25), (int)(textSize.X + 50), (int)(textSize.Y + 40), Color.White * 1, 1, false, 1);
                    Utility.drawTextWithShadow(b, currentText, Game1.smallFont, new Vector2((int)(sizeUI.X / 2) - (int)(textSize.X / 2 + 25) + 25, (int)(sizeUI.Y) - 125 - (int)(textSize.Y + 25) + 20), Game1.textColor);
                }
            }
        }

        public static void CheckMouseHovering()
        {
            Vector2 mousePosition = new Vector2(Game1.getMousePosition(true).X, Game1.getMousePosition(true).Y);

            BarsDatabase.RenderNumericalHunger = mousePosition.X >= BarsPosition.barPosition.X &&
                mousePosition.X <= BarsPosition.barPosition.X + (Textures.HungerSprite.Width * 4) &&
                mousePosition.Y >= BarsPosition.barPosition.Y - 240 &&
                mousePosition.Y <= BarsPosition.barPosition.Y - 240 + (Textures.HungerSprite.Height * 4);

            BarsDatabase.RenderNumericalThirst = mousePosition.X >= BarsPosition.barPosition.X - 60 &&
                mousePosition.X <= BarsPosition.barPosition.X - 60 + (Textures.HungerSprite.Width * 4) &&
                mousePosition.Y >= BarsPosition.barPosition.Y - 240 &&
                mousePosition.Y <= BarsPosition.barPosition.Y - 240 + (Textures.HungerSprite.Height * 4);
        }
    }
}
