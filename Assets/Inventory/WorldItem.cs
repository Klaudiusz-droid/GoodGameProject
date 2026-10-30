using UnityEngine;

public class WorldItem : MonoBehaviour
{
    [SerializeField] private ScriptableObject itemDataAsset;
    [SerializeField] private int stackCount = 1;

    public IItemData ItemData => itemDataAsset as IItemData;
    public int StackCount => stackCount;

    public void Initialize(IItemData item, int count)
    {
        itemDataAsset = item as ScriptableObject;
        stackCount = count;
    }
}
