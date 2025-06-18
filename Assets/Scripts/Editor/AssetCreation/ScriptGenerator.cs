using System.IO;
using UnityEditor;

namespace Tools.AssetCreation
{
    public class ScriptGenerator
    {
        public static void Generate(string name, string templateText, string classSavePath, string classNamePostfix = "")
        {
            string content = templateText.Replace("#NAME#", name);
            string classFilePath = Path.Combine(classSavePath, name + classNamePostfix + ".cs");
            Directory.CreateDirectory(classSavePath);
            File.WriteAllText(classFilePath, content);
            AssetDatabase.Refresh();
        }
    }
}