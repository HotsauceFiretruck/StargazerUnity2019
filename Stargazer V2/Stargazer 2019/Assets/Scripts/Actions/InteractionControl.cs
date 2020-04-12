using UnityEngine;

public class InteractionControl : MonoBehaviour
{
    EquipAction itemAction;
    Entity ownerEntity;
    Inventory inventory;

    public Transform orientation;

    void Start()
    {
        itemAction = GetComponent<EquipAction>();
        ownerEntity = GetComponent<Entity>();
        inventory = GetComponent<Inventory>();
    }

    void WorldInteraction()
    {
        //Obtain Item
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(orientation.transform.position, orientation.transform.forward);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo, 5))
            {
                if (hitInfo.transform.tag == "Item")
                {
                    Item item = hitInfo.transform.GetComponent<Item>();
                    itemAction.ObtainItem(item);
                }
            }
        }

        //Drop Item
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Ray ray = new Ray(orientation.position, orientation.forward);
            RaycastHit hitInfo;

            if (!Physics.Raycast(ray, out hitInfo, 3) && ownerEntity.currentHoldingItem != null)
            {
                this.itemAction.DropItem(ownerEntity.currentHoldingItem);
            }
        }

        //Activate current holding item
        if (Input.GetMouseButton(0) && ownerEntity.currentHoldingItem != null)
        {
            ownerEntity.currentHoldingItem.Activate();
        }

        //Activate Secondary (Only Apply to Weapons)
        if (Input.GetMouseButton(2) && ownerEntity.equipments[EquipmentType.Weapon] != null)
        {
            ownerEntity.equipments[EquipmentType.Weapon].ActivateSecondary();
        }
    }

    void InventoryControl()
    {
        if (inventory != null)
        {
            for (int i = 1; i < inventory.maxNumberOfSlots + 1; i++)
            {
                if (Input.GetKeyDown("" + i))
                {
                    Item item = inventory.ReturnItem(i - 1);
                    if (item != null)
                    {
                        if (item.GetType().IsSubclassOf(typeof(Equipment)))
                        {
                            //If item is an equipment...
                            itemAction.EquipItem((Equipment)item, orientation);
                        }
                        else
                        {
                            //If item is a normal item...
                            itemAction.HoldItem(item, orientation);
                        }
                    }
                }
            }
        }
    }

    void Update()
    {
        if (!PauseMenu.IsGamePaused())
        {
            WorldInteraction();
            InventoryControl();
        }
    }
}
