using UnityEngine;
using System;

public class Inventory : MonoBehaviour
{
    ItemData[] items;
    int currentNumberOfItems = 0;
    public int maxNumberOfSlots = 4;

    public delegate void OnInventoryChange();
    public OnInventoryChange inventoryChangeCallback;

    void Start()
    {
        items = new ItemData[maxNumberOfSlots];
    }

    public bool StoreItem(Item item)
    {
        if (currentNumberOfItems < maxNumberOfSlots)
        {
            int nextEmptySlot = Array.IndexOf(items, null);
            items[nextEmptySlot] = item.itemData;
            Destroy(item.gameObject);
            currentNumberOfItems++;

            if (inventoryChangeCallback != null)
            {
                inventoryChangeCallback.Invoke();
            }

            item.itemData.isStored = true;

            return true;
        }
        return false;
    }

    public Item ReturnItem(int index)
    {
        if (items[index] != null)
        {
            ItemData itemData = items[index];
            GameObject itemObject = (GameObject)Instantiate(items[index].itemPrefab);
            Item item = itemObject.GetComponent<Item>();

            if (item != null)
            {
                DestroyImmediate(item.itemData);
                item.itemData = itemData;
            }

            items[index] = null;
            currentNumberOfItems--;

            if (inventoryChangeCallback != null)
            {
                inventoryChangeCallback.Invoke();
            }

            item.itemData.isStored = false;

            return item;
        }
        return null;
    }

    public void RemoveItem(int index)
    {
        if (items[index] != null)
        {
            DestroyImmediate(items[index]);

            items[index] = null;
            currentNumberOfItems--;

            if (inventoryChangeCallback != null)
            {
                inventoryChangeCallback.Invoke();
            }
        }
    }

    public ItemData[] GetAllItemData()
    {
        return items;
    }
}
