using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FolliageActivator : MonoBehaviour
{

    private  GameObject folliageParent;
    private DistanceActivator FolliageActivatorScript;

    void Start()
    {
        folliageParent = GameObject.Find("Folliage");
        FolliageActivatorScript = folliageParent.GetComponent<DistanceActivator>();

        AddItems();

    }
    void AddItems()
    {
        FolliageActivatorScript.AddList.Add(new ActivatorItem {Item = this.gameObject});
    }

}
