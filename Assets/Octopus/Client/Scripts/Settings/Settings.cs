using UnityEngine;

[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
public class Settings : ScriptableObject
{
    // ================= GENERAL CONFIG =================
    [Header("Config: General")]
    [SerializeField] private string AttributionUrl;

    // ================= INFO =================
    [Header("Info")]
    [SerializeField] private bool isDebug;
    [SerializeField] private int apiVersion = 14;
    [SerializeField] private string codeVersion = "TM_APPS";
    
    // ================= SINGLETON INSTANCE =================
    private static Settings _instance;
    public static Settings Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = Resources.Load<Settings>("Settings");

                if (_instance == null)
                {
                    Debug.LogError("Settings не знайдено в Resources!");
                }
            }
            return _instance;
        }
    }

    // ================= PUBLIC METHODS =================
    public static string GetDomain() => Instance.AttributionUrl;
    
    public static bool IsDebug() => Instance.isDebug;

    public static int ApiVersion() => Instance.apiVersion;

    public static string CodeVersion() => Instance.codeVersion;

    public static string GetAttributionUrl() => "https://" + Instance.AttributionUrl;
}
