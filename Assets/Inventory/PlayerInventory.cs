using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private PlayerInteractor interactor;
    [SerializeField] private int slotCount = 5;

    private InventorySlot[] slots;

    public static event Action<int, InventorySlot> OnItemAdded;

    public int SlotCount => slots.Length;
    public InventorySlot GetSlot(int index) => slots[index];

    private void Awake()
    {
        slots = new InventorySlot[slotCount];
    }

    public void OnPickUpItem(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        TryPickUpTarget();
    }

    private void TryPickUpTarget()
    {
        if (interactor == null || interactor.CurrentTarget == null) return;

        WorldItem worldItem = interactor.CurrentTarget.GetComponent<WorldItem>();
        if (worldItem == null || worldItem.ItemData == null) return;

        if (!TryAddItem(worldItem.ItemData)) return;

        Destroy(worldItem.gameObject);
    }

    private bool TryAddItem(IItemData item)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].Item == item && slots[i].StackCount < item.MaxStackSize)
            {
                slots[i].StackCount++;
                OnItemAdded?.Invoke(i, slots[i]);
                return true;
            }
        }

        int freeSlotIndex = Array.FindIndex(slots, slot => slot == null);
        if (freeSlotIndex == -1) return false;

        slots[freeSlotIndex] = new InventorySlot { Item = item, StackCount = 1 };
        OnItemAdded?.Invoke(freeSlotIndex, slots[freeSlotIndex]);
        return true;
    }
}
