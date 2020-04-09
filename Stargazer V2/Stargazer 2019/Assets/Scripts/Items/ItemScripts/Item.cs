using UnityEngine;

public abstract class Item : MonoBehaviour
{
    public ItemData itemData;

    public virtual void Activate() { }
    public virtual void Deactivate() { }
}
