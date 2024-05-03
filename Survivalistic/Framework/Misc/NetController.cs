using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using Survivalistic_Rebooted.Framework.Bars;
using Survivalistic_Rebooted.Framework.Common;
using Survivalistic_Rebooted.Framework.Databases;
using System.Collections.Generic;

namespace Survivalistic_Rebooted.Framework.Misc
{
    public class NetController
    {
        private static IModHelper Helper = ModEntry.Instance.Helper;

        private static IManifest Manifest = ModEntry.Instance.ModManifest;

        public static bool _firstLoad;

        public static void SyncSpecificPlayer(long player_id)
        {
            if (Context.IsMainPlayer)
            {
                Data _data = Helper.Data.ReadSaveData<Data>($"{player_id}") ?? new Data();
                float[] _multipliers = { ModEntry.Config.PassiveHungerMultiplier, ModEntry.Config.PassiveThirstMultiplier };
                SyncBody _toSend = new SyncBody(_data, Foods.FoodDatabase, _multipliers);

                Helper.Data.WriteSaveData($"{player_id}", _data);

                Debugger.Log($"Sending important Data to farmhand {player_id}.", "Trace");
                Helper.Multiplayer.SendMessage(
                    message: _toSend,
                    messageType: "SaveDataFromHost",
                    modIDs: new[] { Manifest.UniqueID },
                    playerIDs: new[] { player_id }
                );
            }
        }

        public static void SyncAllPlayers()
        {
            if (Context.IsMainPlayer)
            {
                Debugger.Log($"Sending important Data to all farmhands.", "Trace");
                foreach (Farmer farmer in Game1.getOnlineFarmers())
                {
                    Data _data = Helper.Data.ReadSaveData<Data>($"{farmer.UniqueMultiplayerID}") ?? new Data();
                    float[] _multipliers = { ModEntry.Config.PassiveHungerMultiplier, ModEntry.Config.PassiveThirstMultiplier };
                    SyncBody _toSend = new SyncBody(_data, Foods.FoodDatabase, _multipliers);

                    Helper.Data.WriteSaveData($"{farmer.UniqueMultiplayerID}", _data);

                    Debugger.Log($"Sending important Data to farmhand {farmer.UniqueMultiplayerID}.", "Trace");
                    Helper.Multiplayer.SendMessage(
                        message: _toSend,
                        messageType: "SaveDataFromHost",
                        modIDs: new[] { Manifest.UniqueID },
                        playerIDs: new[] { farmer.UniqueMultiplayerID }
                    );
                }
            }
        }

        public static void Sync()
        {
            if (Context.IsMainPlayer)
            {
                if (Game1.IsMultiplayer) Debugger.Log($"Saving host Data.", "Trace");

                Data _data = Helper.Data.ReadSaveData<Data>($"{Game1.player.UniqueMultiplayerID}") ?? new Data();
                if (!_firstLoad)
                {
                    ModEntry.Data = _data;
                    _firstLoad = true;
                }
                Helper.Data.WriteSaveData($"{Game1.player.UniqueMultiplayerID}", ModEntry.Data);

                BarsUpdate.CalculatePercentage();
            }
            else
            {
                Debugger.Log($"Sending important Data to host.", "Trace");

                Helper.Multiplayer.SendMessage(
                    message: ModEntry.Data,
                    messageType: "SaveDataToHost",
                    modIDs: new[] { Manifest.UniqueID },
                    playerIDs: new[] { Game1.MasterPlayer.UniqueMultiplayerID }
                );
            }
        }

        public static void OnMessageReceived(ModMessageReceivedEventArgs e)
        {
            if (!Context.IsMainPlayer && e.FromModID == Manifest.UniqueID && e.Type == "SaveDataFromHost")
            {
                SyncBody _body = e.ReadAs<SyncBody>();
                ModEntry.Data = _body.data;
                Foods.FoodDatabase = _body.dict;
                BarsDatabase.HungerVelocity = _body.multipliers[0];
                BarsDatabase.ThirstVelocity = _body.multipliers[1];

                Debugger.Log("Received important Data from host.", "Trace");
                BarsUpdate.CalculatePercentage();
            }

            if (Context.IsMainPlayer && e.FromModID == Manifest.UniqueID && e.Type == "SaveDataToHost")
            {
                Data _data = e.ReadAs<Data>();
                Debugger.Log($"Received important Data from player {e.FromPlayerID}.", "Trace");
                Helper.Data.WriteSaveData($"{e.FromPlayerID}", _data);
            }
        }
    }

    public class SyncBody
    {
        public Data data;
        public Dictionary<string, string> dict;
        public float[] multipliers;

        public SyncBody(Data _data, Dictionary<string, string> _dict, float[] _multipliers)
        {
            data = _data;
            dict = _dict;
            multipliers = _multipliers;
        }
    }
}
