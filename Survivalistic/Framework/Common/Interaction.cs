using Survivalistic_Rebooted.Framework.UI.Bars;
using StardewModdingAPI;
using StardewValley;
using Survivalistic_Rebooted.Framework.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using SDVObject = StardewValley.Object;

namespace Survivalistic_Rebooted.Framework.Common
{
    public static class Interaction
    {
        private static bool GettingTickInformation = true;

        private static void NotifyUserAboutStatsChange((float _hunger, float _thirst) valuesBeforeChange)
        {
            (float _hunger, float _thirst) = (ModEntry.Data.ActualHunger - valuesBeforeChange._hunger,
                                              ModEntry.Data.ActualThirst - valuesBeforeChange._thirst);

            if (_hunger > 1 || _thirst > 1)
            {
                var messageTemplate = _hunger > 1 ? ModEntry.Instance.Helper.Translation.Get("Info.Hunger.Restore") :
                                                    ModEntry.Instance.Helper.Translation.Get("Info.Thirst.Restore");
                var actualDiff = Math.Max(_hunger, _thirst);

                Game1.addHUDMessage(new HUDMessage(string.Format(messageTemplate, actualDiff), 4));
            }
        }

        public static void UpdateTickInformation()
        {
            if (!GettingTickInformation)
            {
                ModEntry.Data.ActualTick = Game1.ticks;
            }
        }

        public static class NeedsConsumption
        {
            private static bool AlreadyUsingTool;

            private static string ToolUsedName;

            /// <summary>
            /// This is one of routine functions. 
            /// Applies on "Update" event.
            /// <br />
            /// It performs check is player using tool right away or not.
            /// If yes, in the next iteration it will apply stats (hunger / thirst) penalty for performed action and reset status of <see cref="AlreadyUsingTool"/> property.
            /// </summary>
            public static void CheckDoesPlayerUseToolAndApplyEffectsIfRelevant()
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
                        DecreasePlayerStatsByAction(ToolUsedName);

                        Helper.PerformBenefitsPenaltiesRoutineApplying();
                        Helper.NormalizeStatus();
                        BarsWarnings.VerifyStatus();
                    }
                }
            }

            private static void DecreasePlayerStatsByAction(string toolUsed)
            {
                if (Tools.GetToolDatabase().TryGetValue(toolUsed, out string toolStatusString))
                {
                    List<string> toolStatus = toolStatusString.Split('/').ToList();

                    if (ModEntry.Data.ActualHunger >= 0)
                        ModEntry.Data.ActualHunger -= float.Parse(toolStatus[0]) * (RuntimeAdditionalProperties.AdditionalPostEffectToolUseMultiplier * ModEntry.Config.HungerOnActionMultiplier);

                    if (ModEntry.Data.ActualThirst >= 0)
                        ModEntry.Data.ActualThirst -= float.Parse(toolStatus[1]) * (RuntimeAdditionalProperties.AdditionalPostEffectToolUseMultiplier * ModEntry.Config.ThirstOnActionMultiplier);
                }
            }

            /// <summary>
            /// This is one of routine functions.
            /// Applies on "TimeChanged" event.
            /// <br />
            /// This function decreases player stats (hunger / thirst) passively, on time change.
            /// This can be disabled entirely by user settings (see <see cref="Models.Config.Config.PassiveHungerMultiplier"/> and <see cref="Models.Config.Config.PassiveThirstMultiplier"/>).
            /// </summary>
            public static void ApplyPassiveStatsDecreaseIfPossibleAndApplyEffectsIfRelevant()
            {
                if (!Context.IsWorldReady) return;

                DecreseStatsByPassiveConsumptionRates();

                Helper.PerformBenefitsPenaltiesRoutineApplying();
                Helper.NormalizeStatus();
                BarsWarnings.VerifyStatus();
            }

            private static void DecreseStatsByPassiveConsumptionRates()
            {
                if (ModEntry.Data.ActualHunger > 0) ModEntry.Data.ActualHunger -= RuntimeAdditionalProperties.HungerVelocity;
                else ModEntry.Data.ActualHunger = 0;

                if (ModEntry.Data.ActualThirst > 0) ModEntry.Data.ActualThirst -= RuntimeAdditionalProperties.ThirstVelocity;
                else ModEntry.Data.ActualThirst = 0;
            }
        }

        public static class NeedsRestoration
        {
            private static bool AlreadyEating;

            public static void PerformEatingCheckAndApplyEffectsIfPossible()
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

                        (float _hunger, float _thirst) valuesBeforeRestoration = (ModEntry.Data.ActualHunger, ModEntry.Data.ActualThirst);
                        SatePlayerStats(CalculateRestorationValuesByConsumedMeal(eatenObject));
                        NotifyUserAboutStatsChange(valuesBeforeRestoration);
                    }
                }
            }

            private static (int _hungerRestore, int _thirstRestore) CalculateRestorationValuesByConsumedMeal(SDVObject consumedMeal)
            {
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

                return restoreValues;
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

            private static void SatePlayerStats((int _hunger, int _thirst) restorationValues)
            {
                if (ModEntry.Data.ActualHunger < ModEntry.Data.MaxHunger) ModEntry.Data.ActualHunger += restorationValues._hunger;
                if (ModEntry.Data.ActualThirst < ModEntry.Data.MaxThirst) ModEntry.Data.ActualThirst += restorationValues._thirst;
                Helper.NormalizeStatus();
            }
        }

        public static class AwakeManager
        {
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
        }
    }
}
