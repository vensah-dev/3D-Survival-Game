using UnityEngine;

public class Inventory : MonoBehaviour
{
    public InventorySlot[] InventorySlots;
    public GameObject inventoryItemPrefab;
    public GameObject MainInventoryGroup;

    public static Inventory instance;

    int selectedSlot = -1;

    #region
    private void Awake()
    {
        if(instance != null)
        {
            return;
        }

        instance = this;
    }
    #endregion

    private void Start()
    {
        UpdateSelectedSlot(0);
    }

    private void Update()
    {

        //Hot Bar
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            UpdateSelectedSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UpdateSelectedSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            UpdateSelectedSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            UpdateSelectedSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            UpdateSelectedSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            UpdateSelectedSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            UpdateSelectedSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            UpdateSelectedSlot(7);
        }
        if (Input.GetButtonDown("Inventory"))
        {
            MainInventoryGroup.SetActive(!MainInventoryGroup.activeSelf);
        }

    }

    void UpdateSelectedSlot(int newSlot)
    {
        if(selectedSlot >= 0)
        {
            InventorySlots[selectedSlot].deselect();

        }
        InventorySlots[newSlot].select();
        selectedSlot = newSlot;

    }

    public bool AddItem(Item item)
    {

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot CSlot = InventorySlots[i];
            InventoryItem CItem = CSlot.GetComponentInChildren<InventoryItem>();
            if (CItem != null && CItem.item == item && CItem.count < CItem.item.StackSize && item.Stackable == true)
            {
                CItem.count++;
                CItem.RefreshCount();

                return true;
            }
        }

        for (int i = 0; i < InventorySlots.Length; i++)
        {
            InventorySlot CSlot = InventorySlots[i];
            InventoryItem CItem = CSlot.GetComponentInChildren<InventoryItem>();
            if(CItem == null)
            {
                SpawnNewItem(item, CSlot);

                return true;
            }
        }

        return false;
    }

    void SpawnNewItem(Item item, InventorySlot slot)
    {
        GameObject newItemGO = Instantiate(inventoryItemPrefab, slot.transform);
        InventoryItem inventoryItem = newItemGO.GetComponent<InventoryItem>();
        inventoryItem.InitializeItem(item);
    }
}
