using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance { get; private set; }

    public SOScenePrefabData database;
    private GameObject _currentObject;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    public T LoadSceneForState<T>(Shooting.StateType type) where T : Component
    {
        UnloadCurrent();
        var assetRef = database.GetAssetReference(type);
        if (assetRef != null)
        {
            var handle = assetRef.InstantiateAsync();
            // ブロッキングで待つ（簡易実装）。必要なら非同期APIに変更してください。
            handle.WaitForCompletion();
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                var go = handle.Result;
                _currentObject = go;
                var comp = go.GetComponent<T>();
                if (comp == null)
                {
                    Debug.LogError($"Component {typeof(T)} not found on prefab for {type}");
                }
                else
                {
                    var init = comp.GetType().GetMethod("Initialize");
                    init?.Invoke(comp, null);
                }
                return comp;
            }
        }
        Debug.LogError($"Failed to instantiate AssetReference for state {type}");
        return null;
    }

    public void UnloadCurrent()
    {
        if (_currentObject != null)
        {
            Destroy(_currentObject);
            _currentObject = null;
        }
    }
}
