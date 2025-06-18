using System;
using System.IO;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tools.AssetCreation
{
    public class PrefabCreator
    { 
        public static string Create(string className, string namespacePath, GameObject prefab, string savePath)
        {
            var instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            var type = FindTypeInAssemblies($"{namespacePath}.{className}");
            var component = instance.AddComponent(type);

            AddIdToClass(component, className);

            string path = Path.Combine(savePath, $"{className}.prefab");
            PrefabUtility.SaveAsPrefabAsset(instance, path);
            Object.DestroyImmediate(instance);

            return path;
        }
        
        private static void AddIdToClass(Component component, string name)
        {
            var idField = component.GetType().GetField("_id", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
            idField.SetValue(component, name);
        }
        
        private static Type FindTypeInAssemblies(string fullClassName)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type type = assembly.GetType(fullClassName);
                if (type != null)
                    return type;
            }
            return null;
        }
    }
}