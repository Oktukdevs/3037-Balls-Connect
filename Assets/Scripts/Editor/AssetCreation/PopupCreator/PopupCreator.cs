using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using Runtime.Core.UI.Popup;
using Runtime.Game.Services.UI;
using Tools.AssetCreation;
using Tools.AssetCreation.PopupCreator;

public class PopupCreator : EditorWindow
{
    public static PopupCreator Instance;

    private const string PopupSavePath = "Assets\\ProjectAssets\\Prefabs\\UI\\Popups";
    private const string PopupDataSavePath = "Assets\\Scripts\\Runtime\\Game\\UI\\Popup\\Data";
    private const string ClassSavePath = "Assets\\Scripts\\Runtime\\Game\\UI\\Popup";
    private const string StateControllerSavePath = "Assets\\Scripts\\Runtime\\Game\\GameStates\\Game\\Popups";
    private const string PopupNamespace = "Runtime.Core.UI.Popup";

    [SerializeField] private VisualTreeAsset m_VisualTreeAsset = default;
    [SerializeField] private GameObject PopupPrefab;
    [SerializeField] private TextAsset _scriptTemplate;
    [SerializeField] private TextAsset _scriptDataTemplate;
    [SerializeField] private TextAsset _scriptStateControllerTemplate;
    [SerializeField] private UiServiceViewContainer _uiServiceViewContainer;
    [SerializeField] private MonoScript _constPopupsNamesFile;

    public UiServiceViewContainer UiServiceViewContainer => _uiServiceViewContainer;
    
    private List<PopupData> _popupsDataList = new();

    private ListView _popupDataListView;
    private ListView _prefabsListView;

    private List<GameObject> _prefabs = new();

    private bool _createdNewPopup = false;

    [SerializeField] private List<PopupData> _validPopups = new();

    public List<string> CachedAssetPaths;

    [MenuItem("Tools/Popup Creator")]
    public static void ShowExample()
    {
        PopupCreator wnd = GetWindow<PopupCreator>();
        wnd.titleContent = new GUIContent("Popup Creator");
    }

    public void CreateGUI()
    {
        _createdNewPopup = false;

        rootVisualElement.Add(m_VisualTreeAsset.Instantiate());
        FindFields();
    }

    private void OnEnable()
    {
        Instance = this;
        AssemblyReloadEvents.afterAssemblyReload += ProcessAssemblyReload;
    }

    private void OnDisable()
    {
        Instance = null;
        AssemblyReloadEvents.afterAssemblyReload -= ProcessAssemblyReload;
    }
    
    private void FindFields()
    {
        _popupsDataList = new();
        _popupsDataList.Add(new());

        _popupDataListView = rootVisualElement.Q<ListView>("PopupNamesList");
        _prefabsListView = rootVisualElement.Q<ListView>("PrefabsList");

        _prefabs = new PrefabFinder<BasePopup>().FindAll();

        _prefabsListView.makeItem = () => new ObjectField();
        _prefabsListView.bindItem = BindGOItem;
        _prefabsListView.itemsSource = _prefabs;

        _popupDataListView.itemsSource = _popupsDataList;
        _popupDataListView.makeItem = () => new CustomPopupDataElement();
        _popupDataListView.bindItem = BindPopupDataElement;

        rootVisualElement.Q<UnityEngine.UIElements.Button>("CreatePopupsButton").clicked += CreatePopup;
    }
    
    private void BindPopupDataElement(VisualElement visualElement, int index)
    {
        var customElement = (CustomPopupDataElement)visualElement;
        
        if(_popupsDataList[index] == null)
            _popupsDataList[index] = new();
        
        customElement.TextField.value = _popupsDataList[index].Name;
        customElement.DataToggle.value = _popupsDataList[index].CreateData;

        customElement.TextField.RegisterValueChangedCallback(evt =>
        {
            _popupsDataList[index].Name = evt.newValue;
        });

        customElement.DataToggle.RegisterValueChangedCallback(evt =>
        {
            _popupsDataList[index].CreateData = evt.newValue;
        });
        
        customElement.StateToggle.RegisterValueChangedCallback(evt =>
        {
            _popupsDataList[index].CreateStateController = evt.newValue;
        });
    }

    private void BindGOItem(VisualElement element, int index)
    {
        var objectField = (ObjectField)element;
        objectField.value = _prefabs[index];
    }

    private void CreatePopup()
    {
        _validPopups = new ();

        for (int i = 0; i < _popupsDataList.Count; i++)
        {
            if (ClassNameValidator.IsValid(_popupsDataList[i].Name))
                _validPopups.Add(_popupsDataList[i]);
        }

        for(int i = 0; i < _validPopups.Count; i++)
        {
            string className = _validPopups[i].Name;
            
            AddConstToFile(className);
            CreatePopupClass(className);
            
            if (_validPopups[i].CreateData)
                CreatePopupDataClass(className);
            
            if(_validPopups[i].CreateStateController)
                ScriptGenerator.Generate(className, _scriptStateControllerTemplate.text, StateControllerSavePath, "StateController");
        }
    }

    private void CreatePopupClass(string className)
    {
        ScriptGenerator.Generate(className, _scriptTemplate.text, ClassSavePath);
        _createdNewPopup = true;
    }
    
    private void CreatePopupDataClass(string className) => ScriptGenerator.Generate(className, _scriptDataTemplate.text, PopupDataSavePath);

    private void ProcessAssemblyReload()
    {
        if (!_createdNewPopup)
            return;

        _createdNewPopup = false;

        for(int i = 0; i < _validPopups.Count;i++) 
            CreateNewPrefab(_validPopups[i].Name);
    }

    private void CreateNewPrefab(string name) => CachedAssetPaths.Add(PrefabCreator.Create(name, PopupNamespace, PopupPrefab, PopupSavePath));

    private void AddConstToFile(string className) => ConstInjector.AddConstPopup(className, _constPopupsNamesFile);

    public class PopupSaverPostProcessor : AssetPostprocessor
    {
        public static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (Instance == null)
                return;

            if (Instance.CachedAssetPaths == null)
                return;

            List<string> cachedPaths = Instance.CachedAssetPaths;
            for (int i = 0; i < cachedPaths.Count; i++)
            {
                string targetPath = cachedPaths[i].Replace("\\", "/");
                for (int j = 0; j < importedAssets.Length; j++)
                {
                    if (importedAssets[j] == targetPath)
                    {
                        GameObject go = AssetDatabase.LoadAssetAtPath(targetPath, typeof(GameObject)) as GameObject;
                        UIRegistrationManager.RegisterPopup(Instance.UiServiceViewContainer, go);
                    }
                }
            }
        }
    }
}
