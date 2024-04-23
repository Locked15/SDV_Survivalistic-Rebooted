using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewValley;
using StardewValley.GameData.Objects;
using StardewValley.ItemTypeDefinitions;
using Survivalistic_Rebooted.Framework.Bars;
using Survivalistic_Rebooted.Framework.Common.Affection;
using Survivalistic_Rebooted.Framework.Databases;
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

        private static void IncreaseStatus(SDVObject eatenFood)
        {
            (float _hunger, float _thirst) lastValues = (ModEntry.Data.ActualHunger, ModEntry.Data.ActualThirst);
            (int _hunger, int _thirst) restoreValues = (0, 0);

            if (Foods.FoodDatabase.TryGetValue(eatenFood.Name, out string foodStatusString))
            {
                List<string> foodStatus = foodStatusString.Split('/').ToList();
                restoreValues = (int.Parse(foodStatus[0]), int.Parse(foodStatus[1]));
            }
            else if (ModEntry.Config.NonSupportedFood)
            {
                // TODO: Revamp this.
                // Well, this is a temporary solution.
                var isDrink = true;

                if (isDrink) restoreValues._thirst = eatenFood.Edibility;
                if (isDrink) restoreValues._hunger = eatenFood.Edibility;
            }

            UpdateStats(restoreValues);
            NotifyUserAboutStatsChange(lastValues);

            if (!Benefits.VerifyBenefits())
                Penalty.VerifyPenalty();
        }

        private static void UpdateStats((int _hunger, int _thirst) restoreValues)
        {
            if (ModEntry.Data.ActualHunger < ModEntry.Data.MaxHunger) ModEntry.Data.ActualHunger += restoreValues._hunger;
            if (ModEntry.Data.ActualThirst < ModEntry.Data.MaxThirst) ModEntry.Data.ActualThirst += restoreValues._thirst;

            BarsInformations.NormalizeStatus();
        }

        private static void NotifyUserAboutStatsChange((float _hunger, float _thirst) lastValues)
        {
            (float _hunger, float _thirst) = (ModEntry.Data.ActualHunger - lastValues._hunger,
                                              ModEntry.Data.ActualThirst - lastValues._thirst);

            if (_hunger > 1 || _thirst > 1)
            {
                var messageTemplate = _hunger > 1 ? ModEntry.Instance.Helper.Translation.Get("info-fullness") :
                                                    ModEntry.Instance.Helper.Translation.Get("info-thirsty");
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
                    ModEntry.Data.ActualHunger -= float.Parse(toolStatus[0]) * (BarsDatabase.ToolUseMultiplier * ModEntry.Config.HungerActionMultiplier);

                if (ModEntry.Data.ActualThirst >= 0) 
                    ModEntry.Data.ActualThirst -= float.Parse(toolStatus[1]) * (BarsDatabase.ToolUseMultiplier * ModEntry.Config.ThirstActionMultiplier);

                if (!Benefits.VerifyBenefits())
                    Penalty.VerifyPenalty();

                BarsInformations.NormalizeStatus();
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
