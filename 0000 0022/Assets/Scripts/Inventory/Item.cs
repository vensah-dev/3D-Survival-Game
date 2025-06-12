using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "new item", menuName = "Inventory/item")]
public class Item : ScriptableObject
{
    public string Name = "Item Name";
    public Sprite Icon = null;
    public bool Stackable = true;
    public int StackSize = 50;

}
