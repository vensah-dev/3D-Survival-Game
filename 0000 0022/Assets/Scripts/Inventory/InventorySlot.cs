using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public Image image;
    public Color selectColor, idleColor;

    public void select()
    {
        image.color = selectColor;
    }

    public void deselect()
    {
        image.color = idleColor;

    }

    public void OnDrop(PointerEventData eventData)
    {
        Transform parentAfterSwap;

        GameObject dropped = eventData.pointerDrag;
        InventoryItem CurrentInventoryItem = dropped.GetComponent<InventoryItem>();


        if (transform.childCount == 0)
        {
            CurrentInventoryItem.parentAfterDrag = transform;
        }
        else if (transform.childCount > 0 && CurrentInventoryItem.name != transform.GetChild(0).GetComponent<InventoryItem>().name)
        {
            Transform HoverObject = transform.GetChild(0);
            InventoryItem HoverItem = transform.GetComponent<InventoryItem>();

            parentAfterSwap = CurrentInventoryItem.parentAfterDrag;
            HoverObject.SetParent(parentAfterSwap);
            CurrentInventoryItem.parentAfterDrag = transform;
        }
        else if (CurrentInventoryItem.name == transform.GetChild(0).GetComponent<InventoryItem>().name
            && CurrentInventoryItem.item.Stackable
            && CurrentInventoryItem.item.StackSize >= CurrentInventoryItem.count + transform.GetChild(0).GetComponent<InventoryItem>().count)
        {
            Transform HoverObject = transform.GetChild(0);
            InventoryItem HoverItem = transform.GetComponent<InventoryItem>();

            HoverItem.count = HoverItem.count + CurrentInventoryItem.count;
            HoverItem.RefreshCount();

            Destroy(CurrentInventoryItem.gameObject);

        }

    }
}
