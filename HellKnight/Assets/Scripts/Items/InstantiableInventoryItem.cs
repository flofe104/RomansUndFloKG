using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiableInventoryItem : InventoryItem, IInstantiatableItem
{

    [Tooltip("Gameobject to instantiate when the item is equiped")]
    public GameObject prefab;

    [Tooltip("Image to display in inventory view")]
    public Sprite uiImage;

    public Vector3 equipPosition;

    public Vector3 equipEulerAngle;
                 
    public Vector3 equipScale = Vector3.one;

    public GameObject CreateInstance(Transform parent)
    {
        GameObject instance = Instantiate(prefab, parent);
        Transform t = instance.transform;
        t.localPosition = equipPosition;
        t.localEulerAngles = equipEulerAngle;
        t.localScale = equipScale;
        OnInstantiate(instance);
        return instance;
    }

    protected virtual void OnInstantiate(GameObject instance) { }

}
