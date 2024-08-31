using Survivalistic_Rebooted.Framework.UI.Bars;
using StardewModdingAPI;
using StardewValley;
using Survivalistic_Rebooted.Framework.Misc;

namespace Survivalistic_Rebooted.Framework.Common
{
    public static class Commands
    {
        private const string _errorPermission = "You aren't the host!";
        private const string _errorPlayerIsNotFound = "Player not found!";
        private const string _errorCommandIsWrong = "Command missing arguments!\nPlease check the command usage with:";
        private const string _errorMultiplayer = "That command only works on multiplayer!";

        public static void RestoreHunger(string command, string[] args)
        {
            if (!Context.IsWorldReady) return;
            if (Context.IsMultiplayer)
            {
                if (Context.IsMainPlayer)
                {
                    if (args.Length > 0)
                    {
                        bool isPlayerChecked = false;
                        foreach (Farmer farmer in Game1.getAllFarmers())
                        {
                            if (farmer.displayName == args[1])
                            {
                                Data data = ModEntry.Instance.Helper.Data.ReadSaveData<Data>($"{farmer.UniqueMultiplayerID}");
                                data.ActualHunger = data.MaxHunger;
                                ModEntry.Instance.Helper.Data.WriteSaveData($"{farmer.UniqueMultiplayerID}", data);

                                if (Context.IsMainPlayer)
                                {
                                    ModEntry.Data.ActualHunger = data.ActualHunger;
                                    BarsUpdate.CalculatePercentage();
                                }
                                else
                                {
                                    NetController.SyncSpecificPlayer(farmer.UniqueMultiplayerID);
                                }

                                Debugger.Log($"Feeding player {farmer.displayName}.", "Info");
                                isPlayerChecked = true;
                                break;
                            }
                        }
                        if (!isPlayerChecked) Debugger.Log(_errorPlayerIsNotFound, "Error");
                    }
                    else Debugger.Log($"{_errorCommandIsWrong} 'help {command}'", "Error");
                }
                else Debugger.Log(_errorPermission, "Error");
            }
            else
            {
                ModEntry.Data.ActualHunger += int.TryParse(args[0], out int fullnessValue) ? fullnessValue : 0;
                Debugger.Log("Feeding the player.", "Info");
            }
        }

        public static void RestoreThirst(string command, string[] args)
        {
            if (!Context.IsWorldReady) return;
            if (Context.IsMultiplayer)
            {
                if (Context.IsMainPlayer)
                {
                    if (args.Length > 0)
                    {
                        bool isPlayerChecked = false;
                        foreach (Farmer farmer in Game1.getAllFarmers())
                        {
                            if (farmer.displayName == args[1])
                            {
                                Data _data = ModEntry.Instance.Helper.Data.ReadSaveData<Data>($"{farmer.UniqueMultiplayerID}");
                                _data.ActualThirst = _data.MaxThirst;
                                ModEntry.Instance.Helper.Data.WriteSaveData($"{farmer.UniqueMultiplayerID}", _data);

                                if (Context.IsMainPlayer)
                                {
                                    ModEntry.Data.ActualThirst = _data.ActualThirst;
                                    BarsUpdate.CalculatePercentage();
                                }
                                else
                                {
                                    NetController.SyncSpecificPlayer(farmer.UniqueMultiplayerID);
                                }

                                Debugger.Log($"Hydrating player {farmer.displayName}.", "Info");
                                isPlayerChecked = true;
                                break;
                            }
                        }
                        if (!isPlayerChecked) Debugger.Log(_errorPlayerIsNotFound, "Error");
                    }
                    else Debugger.Log($"{_errorCommandIsWrong} 'help {command}'", "Error");
                }
                else Debugger.Log(_errorPermission, "Error");
            }
            else
            {
                ModEntry.Data.ActualThirst += int.TryParse(args[0], out int hydrationValue) ? hydrationValue : 0;
                Debugger.Log("Hydrating the player.", "Info");
            }
        }

        public static void Sate(string command, string[] args)
        {
            if (!Context.IsWorldReady) return;
            if (Context.IsMultiplayer)
            {
                if (Context.IsMainPlayer)
                {
                    if (args.Length > 0)
                    {
                        bool isPlayerChecked = false;
                        foreach (Farmer farmer in Game1.getAllFarmers())
                        {
                            if (farmer.displayName == args[0])
                            {
                                Data data = ModEntry.Instance.Helper.Data.ReadSaveData<Data>($"{farmer.UniqueMultiplayerID}");
                                data.ActualHunger = data.MaxHunger;
                                data.ActualThirst = data.MaxThirst;
                                ModEntry.Instance.Helper.Data.WriteSaveData($"{farmer.UniqueMultiplayerID}", data);

                                if (Context.IsMainPlayer)
                                {
                                    ModEntry.Data.ActualHunger = data.MaxHunger;
                                    ModEntry.Data.ActualThirst = data.ActualThirst;
                                    BarsUpdate.CalculatePercentage();
                                }
                                else
                                {
                                    NetController.SyncSpecificPlayer(farmer.UniqueMultiplayerID);
                                }

                                Debugger.Log($"Setting full status to the player {farmer.displayName}.", "Info");
                                isPlayerChecked = true;
                                break;
                            }
                        }
                        if (!isPlayerChecked) Debugger.Log(_errorPlayerIsNotFound, "Error");
                    }
                    else
                    {
                        Debugger.Log($"{_errorCommandIsWrong} 'help {command}'", "Error");
                    }
                }
                else
                {
                    Debugger.Log(_errorPermission, "Error");
                }
            }
            else
            {
                ModEntry.Data.ActualHunger = ModEntry.Data.MaxHunger;
                ModEntry.Data.ActualThirst = ModEntry.Data.MaxThirst;
                Debugger.Log("Setting full status to the player.", "info");
            }
        }

        public static void ForceSync(string command, string[] args)
        {
            if (!Context.IsWorldReady) return;
            if (Context.IsMultiplayer)
            {
                if (Context.IsMainPlayer)
                {
                    NetController.SyncAllPlayers();
                    NetController.Sync();
                    Debugger.Log($"All players are now synchronized!", "Info");
                }
                else Debugger.Log(_errorPermission, "Error");
            }
            else Debugger.Log(_errorMultiplayer, "Info");
        }
    }
}
