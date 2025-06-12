using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public Item Item;

    void OnCollisionEnter(Collision col)
    {

        if (col.gameObject.tag == "BEAN")
        {
            GetPickedUp();
        }
    }

    public void GetPickedUp()
    {
        bool destroy = Inventory.instance.AddItem(Item);

        if (destroy)
        {
            Destroy(gameObject);
        }

    }
}
