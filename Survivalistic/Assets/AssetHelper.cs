using System.IO;
using System.Linq;

namespace Survivalistic_Rebooted.Assets
{
    public class AssetHelper
    {
        private const string AssetFolderName = "Assets";

        public static string GetBarAssetsFolderPath()
        {
            return Path.Combine(AssetFolderName, BarsConstants.BarAssetsFolderName);
        }

        public static string GetDatabaseAssetsFolderPath(bool toTheCustomEdibles = false)
        {
            return Path.Combine(AssetFolderName, EdiblesDBConstants.EdiblesDBAssetsFolderName, 
                                toTheCustomEdibles ? EdiblesDBConstants.CustomEdiblesDBFolderName : string.Empty);
        }

        public static (string _author, string _identifier) SplitModUniqueIDToAuthorAndIdentifier(string modID)
        {
            var result = modID.Split('.', 2);
            return (result.FirstOrDefault(), result.ElementAtOrDefault(1));
        }

        public class BarsConstants
        {
            internal const string BarAssetsFolderName = "Bars";

            public const string HungerBarAssetFileName = "Hunger_Sprite.png";

            public const string ThirstBarAssetFileName = "Thirst_Sprite.png";
        }

        public class EdiblesDBConstants
        {
            internal const string EdiblesDBAssetsFolderName = "Databases";

            internal const string CustomEdiblesDBFolderName = "Custom";

            public const string BaseGameEdibleDBFileName = "!BaseGame";

            public const string EdiblesDBAssetFileEnding = "_Edibles.json";
        }
    }
}
