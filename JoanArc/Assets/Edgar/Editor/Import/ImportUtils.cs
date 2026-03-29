using System.IO;
using Edgar.Unity.Export;
using UnityEditor;
using UnityEngine;

namespace Edgar.Unity.Editor
{
    #if OndrejNepozitekEdgar
    public class ImportUtils
    {
        [MenuItem("Assets/Import Edgar configuration", true)]
        private static bool ValidateLoadJson()
        {
            // Check if the selected file is a JSON file
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            return path.EndsWith(".json");
        }

        [MenuItem("Assets/Import Edgar configuration")]
        private static void LoadJson()
        {
            // Get the path to the selected JSON file
            var path = AssetDatabase.GetAssetPath(Selection.activeObject);
            if (!File.Exists(path))
            {
                Debug.LogError("File does not exist: " + path);
                return;
            }

            // Read the JSON file content
            var data = File.ReadAllText(path);
            var directory = Path.GetDirectoryName(path);
            var exportDto = JsonUtility.FromJson<ExportDto>(data);

            var importRunner = new ImportRunner(exportDto, directory);
            importRunner.Run();
        }
    }
    #endif
}