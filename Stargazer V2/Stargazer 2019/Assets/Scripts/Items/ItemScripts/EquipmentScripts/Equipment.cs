public abstract class Equipment : Item
{
    public virtual void OnEquipped() { }
    public virtual void OnUnequipped() { }
    public virtual void ActivateSecondary() { }
}
