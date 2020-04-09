using UnityEngine;

public class EquipAction : ItemAction
{
    public void EquipItem(Equipment item, Transform transform)
    {
        if (item.itemData.ownerEntity == null)
        {
            item.itemData.ownerEntity = ownerEntity;
        }

        if (item.itemData.ownerEntity == ownerEntity && !item.itemData.isStored && item != null)
        {
            EquipmentData data = item.itemData as EquipmentData;
            if (data != null)
            {
                if (ownerEntity.equipments[data.equipmentType] != null)
                {
                    UnequipItem(ownerEntity.equipments[data.equipmentType]);
                }

                if (data.equipmentType == EquipmentType.Weapon)
                {
                    ownerEntity.equipments[EquipmentType.Weapon] = item;
                    HoldItem(item, transform);
                }
                else if (data.equipmentType == EquipmentType.Footwear)
                {
                    ownerEntity.equipments[EquipmentType.Footwear] = item;
                    item.transform.parent = transform;
                    item.transform.localPosition = Vector3.zero;

                    MeshRenderer[] renders = item.GetComponentsInChildren<MeshRenderer>();

                    foreach (MeshRenderer r in renders)
                    {
                        r.enabled = false;
                    }

                    Rigidbody rb = item.GetComponent<Rigidbody>();
                    if (rb != null)
                    {
                        rb.isKinematic = true;
                        rb.useGravity = false;
                        rb.detectCollisions = false;
                    }
                }

                item.OnEquipped();
            }
        }
    }

    public void UnequipItem(Equipment item)
    {
        if (item.itemData.ownerEntity == ownerEntity && !item.itemData.isStored && item != null)
        {
            EquipmentData data = item.itemData as EquipmentData;
            if (data != null)
            {
                item.Deactivate();
                item.OnUnequipped();

                if (ownerEntity.equipments[data.equipmentType] == item)
                {
                    ownerEntity.equipments[data.equipmentType] = null;
                }

                if (ownerEntity.currentHoldingItem == item)
                {
                    ownerEntity.currentHoldingItem = null;
                }

                //Try to store the item. If fail, then drop the item.
                Inventory inventory = ownerEntity.GetComponent<Inventory>();
                if (inventory != null)
                {
                    bool success = inventory.StoreItem(item);

                    if (!success)
                    {
                        DropItem(item);
                    }
                }
                else
                {
                    DropItem(item);
                }
            }
        }
    }

    public override void DropItem(Item item)
    {
        if (item.itemData.ownerEntity == ownerEntity && !item.itemData.isStored && item != null)
        {
            EquipmentData data = (EquipmentData)item.itemData;
            if (data != null)
            {
                Equipment equipment = (Equipment)item;
                equipment.OnUnequipped();

                item.Deactivate();

                if (ownerEntity.equipments[data.equipmentType] == item) ownerEntity.equipments[data.equipmentType] = null;
                if (ownerEntity.currentHoldingItem == item) ownerEntity.currentHoldingItem = null;

                item.transform.gameObject.layer = LayerMask.NameToLayer("Item");

                MeshRenderer[] renders = item.GetComponentsInChildren<MeshRenderer>();

                foreach (MeshRenderer r in renders)
                {
                    r.enabled = true;
                }

                Rigidbody rb = item.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = false;
                    rb.useGravity = true;
                    rb.detectCollisions = true;

                    rb.AddForce(item.transform.rotation * Vector3.forward * 4, ForceMode.Impulse);
                }

                item.itemData.ownerEntity = null;
                item.transform.parent = null;

            }
        }
    }
}
