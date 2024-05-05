using StardewModdingAPI;
using Survivalistic_Rebooted.Framework.Misc;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Survivalistic_Rebooted.Framework.Databases
{
    static class DBController
    {
        public static void LoadDatabases()
        {
            List<EdiblesDB> rawEdiblesDBs = new()
            {
                ModEntry.Instance.Helper.Data.ReadJsonFile<EdiblesDB>(Path.Combine(AssetHelper.GetDatabaseAssetsFolderPath(),
                                                                                   string.Concat(AssetHelper.EdiblesDBConstants.BaseGameEdibleDBFileName,
                                                                                                 AssetHelper.EdiblesDBConstants.EdiblesDBAssetFileEnding)))
            };
            foreach (IModInfo _mod in ModEntry.Instance.Helper.ModRegistry.GetAll().ToList())
            {
                var assetFilePath = Path.Combine(AssetHelper.GetDatabaseAssetsFolderPath(true), AssetHelper.SplitModUniqueIDToAuthorAndIdentifier(_mod.Manifest.UniqueID)._author,
                                                                                                string.Concat(AssetHelper.SplitModUniqueIDToAuthorAndIdentifier(_mod.Manifest.UniqueID)._identifier,
                                                                                                              AssetHelper.EdiblesDBConstants.EdiblesDBAssetFileEnding));
                rawEdiblesDBs.Add(ModEntry.Instance.Helper.Data.ReadJsonFile<EdiblesDB>(assetFilePath));
            }

            var actualEdibleDBs = rawEdiblesDBs.Where(db => db != null).ToList();
            actualEdibleDBs.ForEach(db => AddRedEdiblesToInGameDB(db));
        }

        private static void AddRedEdiblesToInGameDB(EdiblesDB db)
        {
            for (var i = 0; i < db.Edibles.Length / 2; i++)
            {
                try
                {
                    Foods.FoodDatabase.Add(db.Edibles[i, 0], db.Edibles[i, 1]);
                }
                catch (System.ArgumentException exception)
                {
                    ModEntry.Instance.Monitor.Log($"({db.Edibles[i, 0]}) — Duplicate Entry!", LogLevel.Trace);
                    ModEntry.Instance.Monitor.Log(exception.Message, LogLevel.Trace);
                    ModEntry.Instance.Monitor.Log(exception.StackTrace, LogLevel.Trace);
                }
            }
        }
    }
}
