using UnityEditor;

namespace EditorTools.ObjectRenamer
{
    public class ObjectRenamer
    {
        [MenuItem("Tools/Object Renamer")]
        public static void CreatePopup()
        {
            ObjectRenamerWindow.InitWindow();
        }
    }
}