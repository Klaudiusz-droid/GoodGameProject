using System.Collections.Generic;
using UnityEngine;

public class ItemViewController : MonoBehaviour
{
    [SerializeField] private Transform viewpoint;

    private readonly Dictionary<int, GameObject> slotViews = new();
    private int activeSlotIndex = -1;

    private void OnEnable()
    {
        PlayerInventory.OnItemAdded += HandleItemAdded;
        PlayerInventory.OnActiveSlotSelected += HandleActiveSlotSelected;
    }

    private void OnDisable()
    {
        PlayerInventory.OnItemAdded -= HandleItemAdded;
        PlayerInventory.OnActiveSlotSelected -= HandleActiveSlotSelected;
    }

    private void HandleItemAdded(int slotIndex, InventorySlot slot)
    {
        if (slotViews.ContainsKey(slotIndex)) return;
        if (slot.Item.Prefab == null) return;

        GameObject view = Instantiate(slot.Item.Prefab, viewpoint);
        view.transform.localPosition = Vector3.zero;
        view.transform.localRotation = Quaternion.identity;
        view.SetActive(slotIndex == activeSlotIndex);

        slotViews[slotIndex] = view;
    }

    private void HandleActiveSlotSelected(int slotIndex)
    {
        if (slotViews.TryGetValue(activeSlotIndex, out GameObject previousView) && previousView != null)
        {
            previousView.SetActive(false);
        }

        activeSlotIndex = slotIndex;

        if (slotViews.TryGetValue(activeSlotIndex, out GameObject newView) && newView != null)
        {
            newView.SetActive(true);
        }
    }
}
