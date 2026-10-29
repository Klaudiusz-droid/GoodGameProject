using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ScriptableObject itemDataAsset;

    public IItemData ItemData => itemDataAsset as IItemData;
}
