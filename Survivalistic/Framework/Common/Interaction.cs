using Framework.Common;
using StardewModdingAPI;
using StardewValley;
using Survivalistic_Rebooted.Framework.Bars;
using Survivalistic_Rebooted.Framework.Common.Affection;
using Survivalistic_Rebooted.Framework.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using SDVObject = StardewValley.Object;

namespace Survivalistic_Rebooted.Framework.Common
{
    public static class Interaction
    {
        private static bool AlreadyEating;

        private static bool AlreadyUsingTool;

        private static string ToolUsedName;

        private static bool GettingTickInformation = true;

        public static void EatingCheck()
        {
            if (!Context.IsWorldReady) return;

            var eatenObject = (Game1.player.itemToEat as StardewValley.Object);
            if (Game1.player.isEating)
            {
                AlreadyEating = true;
            }
            else
            {
                if (AlreadyEating)
                {
                    AlreadyEating = false;
                    IncreaseStatus(eatenObject);
                }
            }
        }

        public static void UsingToolCheck()
        {
            if (!Context.IsWorldReady) return;

            if (Game1.player.UsingTool)
            {
                ToolUsedName = Game1.player.CurrentTool.BaseName;
                AlreadyUsingTool = true;
            }
            else
            {
                if (AlreadyUsingTool)
                {
                    AlreadyUsingTool = false;
                    DecreaseStatus(ToolUsedName);
                }
            }
        }

        private static void IncreaseStatus(SDVObject consumedMeal)
        {
            (float _hunger, float _thirst) lastValues = (ModEntry.Data.ActualHunger, ModEntry.Data.ActualThirst);
            (int _hunger, int _thirst) restoreValues = (0, 0);

            if (Foods.FoodDatabase.TryGetValue(consumedMeal.Name, out string foodStatusString))
            {
                List<string> foodStatus = foodStatusString.Split('/').ToList();
                restoreValues = (int.Parse(foodStatus[0]), int.Parse(foodStatus[1]));
            }
            else if (ModEntry.Config.ApplyPropertiesToNonRecognizedFood)
            {
                var isDrink = CheckIsConsumedItemIsDrink(consumedMeal.GetContextTags());

                if (isDrink) restoreValues._thirst = consumedMeal.Edibility;
                if (!isDrink) restoreValues._hunger = consumedMeal.Edibility;
            }

            UpdateStats(restoreValues);
            NotifyUserAboutStatsChange(lastValues);

            Benefits.VerifyStatus();
            Penalties.VerifyStatus();
        }

        /// <summary>
        /// Checks item context tags to contain "Drinking" ones.
        /// May not always return correct result because sometimes even drinking meals don't have relevant categories (like 'Truffle Oil').
        /// </summary>
        /// <param name="contextTags">Context tags of the target item.</param>
        /// <returns>Boolean value is this meal drink or not.</returns>
        private static bool CheckIsConsumedItemIsDrink(IEnumerable<string> contextTags)
        {
            var isClearDrinkItem = contextTags.Contains("drink_item");
            var isSyrupItem = contextTags.Contains("category_syrup");

            return isClearDrinkItem || isSyrupItem;
        }

        private static void UpdateStats((int _hunger, int _thirst) restoreValues)
        {
            if (ModEntry.Data.ActualHunger < ModEntry.Data.MaxHunger) ModEntry.Data.ActualHunger += restoreValues._hunger;
            if (ModEntry.Data.ActualThirst < ModEntry.Data.MaxThirst) ModEntry.Data.ActualThirst += restoreValues._thirst;
            Helper.NormalizeStatus();
        }

        private static void NotifyUserAboutStatsChange((float _hunger, float _thirst) lastValues)
        {
            (float _hunger, float _thirst) = (ModEntry.Data.ActualHunger - lastValues._hunger,
                                              ModEntry.Data.ActualThirst - lastValues._thirst);

            if (_hunger > 1 || _thirst > 1)
            {
                var messageTemplate = _hunger > 1 ? ModEntry.Instance.Helper.Translation.Get("Info.Fullness.Restore") :
                                                    ModEntry.Instance.Helper.Translation.Get("Info.Thirst.Restore");
                var actualDiff = Math.Max(_hunger, _thirst);

                Game1.addHUDMessage(new HUDMessage(string.Format(messageTemplate, actualDiff), 4));
            }
        }

        private static void DecreaseStatus(string toolUsed)
        {
            if (Tools.GetToolDatabase().TryGetValue(toolUsed, out string toolStatusString))
            {
                List<string> toolStatus = toolStatusString.Split('/').ToList();

                if (ModEntry.Data.ActualHunger >= 0)
                    ModEntry.Data.ActualHunger -= float.Parse(toolStatus[0]) * (BarsDatabase.ToolUseMultiplier * ModEntry.Config.HungerOnActionMultiplier);

                if (ModEntry.Data.ActualThirst >= 0)
                    ModEntry.Data.ActualThirst -= float.Parse(toolStatus[1]) * (BarsDatabase.ToolUseMultiplier * ModEntry.Config.ThirstOnActionMultiplier);

                if (!Benefits.VerifyStatus())
                    Penalties.VerifyStatus();
                Helper.NormalizeStatus();
                BarsWarnings.VerifyStatus();
            }
        }

        public static void Awake()
        {
            ModEntry.Data.InitialHunger = ModEntry.Data.ActualHunger;
            ModEntry.Data.InitialThirst = ModEntry.Data.ActualThirst;
            ModEntry.Data.ActualDay = Game1.Date.DayOfMonth;
            ModEntry.Data.ActualSeason = Game1.Date.SeasonIndex;
            ModEntry.Data.ActualYear = Game1.Date.Year;
        }

        public static void ReceiveAwakeInfo()
        {
            if (Game1.IsMultiplayer)
            {
                if (ModEntry.Data.ActualDay != Game1.Date.DayOfMonth ||
                        ModEntry.Data.ActualSeason != Game1.Date.SeasonIndex ||
                        ModEntry.Data.ActualYear != Game1.Date.Year ||
                        ModEntry.Data.ActualTick < Game1.ticks)
                {
                    ModEntry.Data.ActualHunger = ModEntry.Data.InitialHunger;
                    ModEntry.Data.ActualThirst = ModEntry.Data.InitialThirst;
                }
            }
            else
            {
                ModEntry.Data.ActualHunger = ModEntry.Data.InitialHunger;
                ModEntry.Data.ActualThirst = ModEntry.Data.InitialThirst;
            }
            GettingTickInformation = false;
        }

        public static void UpdateTickInformation()
        {
            if (!GettingTickInformation)
            {
                ModEntry.Data.ActualTick = Game1.ticks;
            }
        }
    }
}
