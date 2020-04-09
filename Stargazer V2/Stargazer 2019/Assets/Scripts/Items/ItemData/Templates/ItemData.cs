using UnityEngine;
using System;

[CreateAssetMenu(fileName = "ItemData", menuName = "Item/ItemData", order = 0)]
public class ItemData : ScriptableObject
{
    public string itemName = "Default Name";
    public int maxStackAmount = 1;
    public GameObject itemPrefab;
    public Entity ownerEntity;

    [NonSerialized]
    public bool isStored = false;
}
