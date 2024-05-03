using StardewModdingAPI;
using Survivalistic_Rebooted.Assets;
using System.IO;
using System.Linq;

namespace Survivalistic_Rebooted.Framework.Databases
{
    static class DBController
    {
        public static void LoadDatabases()
        {
            EdiblesDB _actualEdiblesRawDatabase = ModEntry.Instance.Helper.Data.ReadJsonFile<EdiblesDB>(Path.Combine(AssetHelper.GetDatabaseAssetsFolderPath(),
                                                                                                              string.Concat(AssetHelper.EdiblesDBConstants.BaseGameEdibleDBFileName,
                                                                                                                            AssetHelper.EdiblesDBConstants.EdiblesDBAssetFileEnding)));
            foreach (IModInfo _mod in ModEntry.Instance.Helper.ModRegistry.GetAll().ToList())
            {
                var assetFilePath = Path.Combine(AssetHelper.GetDatabaseAssetsFolderPath(true), AssetHelper.SplitModUniqueIDToAuthorAndIdentifier(_mod.Manifest.UniqueID)._author,
                                                                                                string.Concat(AssetHelper.SplitModUniqueIDToAuthorAndIdentifier(_mod.Manifest.UniqueID)._identifier,
                                                                                                              AssetHelper.EdiblesDBConstants.EdiblesDBAssetFileEnding));
                if (File.Exists(assetFilePath))
                    _actualEdiblesRawDatabase = ModEntry.Instance.Helper.Data.ReadJsonFile<EdiblesDB>(assetFilePath);
            }

            if (_actualEdiblesRawDatabase != null) AddRedEdiblesToInGameDB(_actualEdiblesRawDatabase);
        }

        private static void AddRedEdiblesToInGameDB(EdiblesDB _actualModDatabase)
        {
            for (var i = 0; i < _actualModDatabase.Edibles.Length / 2; i++)
            {
                try
                {
                    Foods.FoodDatabase.Add(_actualModDatabase.Edibles[i, 0], _actualModDatabase.Edibles[i, 1]);
                }
                catch (System.ArgumentException exception)
                {
                    ModEntry.Instance.Monitor.Log($"({_actualModDatabase.Edibles[i, 0]}) — Duplicate Entry!", LogLevel.Trace);
                    ModEntry.Instance.Monitor.Log(exception.Message, LogLevel.Trace);
                    ModEntry.Instance.Monitor.Log(exception.StackTrace, LogLevel.Trace);
                }
            }
        }
    }
}
