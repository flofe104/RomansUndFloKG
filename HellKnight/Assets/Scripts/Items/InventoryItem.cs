using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : ScriptableObject
{

    public string itemName;

    [Range(0,100)]
    public int cost;

}
