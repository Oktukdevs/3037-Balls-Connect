using UnityEditor;

namespace EditorTools.SpriteOptimizer
{
    public class SpriteOptimizer
    {
        [MenuItem("Tools/Sprite Optimizer")]
        public static void CreatePopup()
        {
            SpriteOptimizerWindow.InitWindow();
        }
    }
}