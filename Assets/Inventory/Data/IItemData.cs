using UnityEngine;

public interface IItemData
{
    string ItemName { get; }
    int MaxStackSize { get; }
    Sprite Icon { get; }
    GameObject Prefab { get; }
}
