using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace Tools.AssetCreation
{
    public class ConstInjector
    {
        public static void AddConstScreen(string className, MonoScript constScreenNamesFile)
        {
            string filePath = AssetDatabase.GetAssetPath(constScreenNamesFile);

            string fileContent = File.ReadAllText(filePath);

            string constLine = $"        public const string {className} = \"{className}\";";

            string pattern = @"public\s+static\s+class\s+ConstScreens\s*\{([^}]*)\}";
            var match = Regex.Match(fileContent, pattern);

            if (match.Success)
            {
                int insertIndex = match.Index + match.Length - 1;

                fileContent = fileContent.Insert(insertIndex - 1, "\n" + constLine);

                fileContent = Regex.Replace(fileContent, @"\n\s*\n", "\n");

                File.WriteAllText(filePath, fileContent);

                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Failed to locate ConstScreens class in the file.");
            }
        }
        
        public static void AddConstPopup(string className, MonoScript constScreenNamesFile)
        {
            string filePath = AssetDatabase.GetAssetPath(constScreenNamesFile);

            string fileContent = File.ReadAllText(filePath);

            string constLine = $"        public const string {className} = \"{className}\";";

            string pattern = @"public\s+static\s+class\s+ConstPopups\s*\{([^}]*)\}";

            var match = Regex.Match(fileContent, pattern);

            if (match.Success)
            {
                int insertIndex = match.Index + match.Length - 1;

                fileContent = fileContent.Insert(insertIndex - 1, "\n" + constLine);

                fileContent = Regex.Replace(fileContent, @"\n\s*\n", "\n");

                File.WriteAllText(filePath, fileContent);

                AssetDatabase.Refresh();
            }
            else
            {
                Debug.LogError("Failed to locate ConstPopups class in the file.");
            }
        }
    }
}