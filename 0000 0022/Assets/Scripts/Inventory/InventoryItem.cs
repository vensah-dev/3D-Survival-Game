using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;
using TMPro;


public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image icon;
    public TextMeshProUGUI countText;

    public Item item;
    public int count = 1;
    [HideInInspector] public Transform parentAfterDrag;

    private void Start()
    {
        InitializeItem(item);
    }

    public void InitializeItem(Item newItem)
    {
        item = newItem;
        icon.sprite = newItem.Icon;
        RefreshCount();
    }

    public void RefreshCount()
    {
        countText.text = count.ToString();
        bool TextActive = count > 1;
        countText.gameObject.SetActive(TextActive);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        icon.raycastTarget = false;

        parentAfterDrag = transform.parent;
        transform.SetParent(transform.root);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        icon.raycastTarget = true;

        transform.SetParent(parentAfterDrag);
    }

}
