using Runtime.Core.UI.Popup;
using Runtime.Game.Services.UI;
using Runtime.Game.UI.Screen;
using UnityEditor;
using UnityEngine;

namespace Tools.AssetCreation
{
    public class UIRegistrationManager
    {
        public static void RegisterScreen(UiServiceViewContainer uiServiceViewContainer, GameObject prefabAsset)
        {
            SerializedObject serializedUiServiceViewContainer = new SerializedObject(uiServiceViewContainer);

            UiScreen uiScreenComponent = prefabAsset.GetComponent<UiScreen>();

            SerializedProperty screensPrefabProperty = serializedUiServiceViewContainer.FindProperty("_screensPrefab");

            screensPrefabProperty.arraySize++;
            screensPrefabProperty.GetArrayElementAtIndex(screensPrefabProperty.arraySize - 1).objectReferenceValue = uiScreenComponent;

            serializedUiServiceViewContainer.ApplyModifiedProperties();
        }
        
        public static void RegisterPopup(UiServiceViewContainer uiServiceViewContainer, GameObject prefabAsset)
        {
            SerializedObject serializedUiServiceViewContainer = new SerializedObject(uiServiceViewContainer);

            BasePopup basePopupComponent = prefabAsset.GetComponent<BasePopup>();

            SerializedProperty popupsPrefabProperty = serializedUiServiceViewContainer.FindProperty("_popupsPrefab");

            popupsPrefabProperty.arraySize++;
            popupsPrefabProperty.GetArrayElementAtIndex(popupsPrefabProperty.arraySize - 1).objectReferenceValue = basePopupComponent;

            serializedUiServiceViewContainer.ApplyModifiedProperties();
        }
    }
}