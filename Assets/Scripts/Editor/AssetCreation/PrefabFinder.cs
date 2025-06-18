using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Tools.AssetCreation
{
    public class PrefabFinder<T> where T : Component
    {
        public List<GameObject> FindAll()
        {
            var results = new List<GameObject>();
            string[] guids = AssetDatabase.FindAssets("t:Prefab");

            foreach (var guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                if (prefab != null && prefab.GetComponent<T>() != null)
                    results.Add(prefab);
            }
            return results;
        }
    }
}