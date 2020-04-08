using UnityEngine;
using System;

public class Inventory : MonoBehaviour {
    [NonSerialized]
    public GameObject[] items;
    public int maxItems = 4;
    [NonSerialized]
    public int numberOfItems = 0;
    [NonSerialized]
    public bool isUpdateSyncUI = false; //signals UI to update list and change this value in UI

    void Start() {
        items = new GameObject[maxItems];
    }

    public bool Store(GameObject item) {
        if (numberOfItems < 5 && item.transform.tag == "Equipment") {
            int nextEmptySlot = Array.IndexOf(items, null);
            items[nextEmptySlot] = item;

            item.SetActive(false);
            numberOfItems++;
            isUpdateSyncUI = true;

            return true;
        }
        return false;
    }

    public GameObject GetItem(int slotNumber) {
        GameObject item = items[slotNumber];
        if (item != null) {
            items[slotNumber] = null;
            item.SetActive(true);
            numberOfItems--;
            isUpdateSyncUI = true;
        }
        return item;
    }

}
