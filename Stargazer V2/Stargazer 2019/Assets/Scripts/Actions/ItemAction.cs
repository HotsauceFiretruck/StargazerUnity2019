using UnityEngine;

public class ItemAction : MonoBehaviour
{
    //Holding angle is in degrees
    public float defaultHoldingDistance;
    public float defaultHoldingAngle;
    protected Entity ownerEntity;

    void Awake()
    {
        ownerEntity = GetComponent<Entity>();
    }

    public bool ObtainItem(Item item)
    {
        Inventory inventory = ownerEntity.GetComponent<Inventory>();
        if (!item.itemData.isStored && inventory != null)
        {
            bool success = inventory.StoreItem(item);
            if (success)
            {
                item.itemData.ownerEntity = ownerEntity;
            }
            return success;
        }
        return false;
    }

    public void HoldItem(Item item, Transform transform)
    {
        HoldItem(item, transform, defaultHoldingDistance, defaultHoldingAngle);
    }

    public void HoldItem(Item item, Transform transform, float holdingDistance, float holdingAngle)
    {
        if (item.itemData.ownerEntity == null)
        {
            item.itemData.ownerEntity = ownerEntity;
        }

        if (item.itemData.ownerEntity == ownerEntity && !item.itemData.isStored)
        {
            if (ownerEntity.currentHoldingItem != null)
            {
                Inventory inventory = ownerEntity.GetComponent<Inventory>();
                if (inventory != null)
                {
                    inventory.StoreItem(ownerEntity.currentHoldingItem);
                }
                else
                {
                    DropItem(ownerEntity.currentHoldingItem);
                }
                ownerEntity.currentHoldingItem = null;
            }

            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
                rb.useGravity = false;
                rb.detectCollisions = false;
            }

            ownerEntity.currentHoldingItem = item;

            item.transform.parent = transform;
            item.transform.localPosition = Quaternion.Euler(Vector3.right * holdingAngle) * Vector3.forward * holdingDistance;
            item.transform.eulerAngles = transform.eulerAngles;
            item.transform.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");

        }
    }

    public virtual void DropItem(Item item)
    {
        if (item.itemData.ownerEntity == ownerEntity && !item.itemData.isStored)
        {
            item.Deactivate();

            item.transform.gameObject.layer = LayerMask.NameToLayer("Item");

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
            if (item == ownerEntity.currentHoldingItem) ownerEntity.currentHoldingItem = null;
        }
    }
}
