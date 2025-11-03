using UnityEngine;
using UnityEngine.AddressableAssets;

[CreateAssetMenu(menuName = "SOData/SOScenePrefabData")]
public class SOScenePrefabData : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public Shooting.StateType stateType;
        public AssetReferenceGameObject assetReference;
    }

    public Entry[] entries;
    public AssetReferenceGameObject GetAssetReference(Shooting.StateType type)
    {
        if (entries == null) return null;
        foreach (var e in entries)
        {
            if (e.stateType == type) return e.assetReference;
        }
        return null;
    }
}
