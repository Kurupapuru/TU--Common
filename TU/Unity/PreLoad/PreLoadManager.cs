using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Configuration/PreLoad", fileName = "PreLoad Configuration")]
public class PreLoadManager : ScriptableObject
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void PreLoad()
    {
        var preLoadManager = Resources.Load<PreLoadManager>("PreLoad Configuration");
        if (preLoadManager == null) return;
        foreach (var prefabPath in preLoadManager.prefabsPaths)
        {
            var prefab = Resources.Load<GameObject>(prefabPath.path);
            Instantiate(prefab);
        }
    }
    
    
    // Configuration
    public ResourcePathGameObject[] prefabsPaths;
}