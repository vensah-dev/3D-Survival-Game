using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceActivator : MonoBehaviour
{
    public int MaxViewDistance = 50;

    public GameObject Player;

    public List<ActivatorItem> AddList;

    private List<ActivatorItem> ActivatorItems;

    void Start()
    {
        ActivatorItems = new List<ActivatorItem>();
        AddList = new List<ActivatorItem>();

        AddItems();
    }

    public void AddItems()
    {
        if(AddList.Count > 0)
        {
            foreach (ActivatorItem item in AddList)
            {
                if(item.Item != null)
                {
                     ActivatorItems.Add(item);
                }
            }
            AddList.Clear();
        }

        StartCoroutine("UpdateItems");

    }

    IEnumerator UpdateItems()
    {
        List<ActivatorItem> removeList = new List<ActivatorItem>();

        if(ActivatorItems.Count > 0)
        {
            foreach (ActivatorItem item in ActivatorItems)
            {
                if(item.Item == null)
                {
                    removeList.Add(item);
                }
                else
                {
                    if (Vector3.Distance(Player.transform.position, item.Item.transform.position) > MaxViewDistance)
                    {
                        item.Item.SetActive(false);

                    }
                    else
                    {
                        item.Item.SetActive(true);

                    }
                }

            }
        }

        yield return new WaitForSeconds(0.001f);

        if(removeList.Count > 0)
        {
            foreach (ActivatorItem item in removeList)
            {
                ActivatorItems.Remove(item);
            }
        }

        yield return new WaitForSeconds(0.001f);

        AddItems();


    }
}

public class ActivatorItem
{
    public GameObject Item;
}
