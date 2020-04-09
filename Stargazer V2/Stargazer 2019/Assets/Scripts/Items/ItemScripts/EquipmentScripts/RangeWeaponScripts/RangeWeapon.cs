using UnityEngine;

public abstract class RangeWeapon : Equipment
{
    public int GetAmmoCount()
    {
        RangeWeaponData data = (RangeWeaponData)itemData;

        return data.currentAmmoCount;
    }

    //If the number of ammo given exceed the max ammo count, returns the amount of ammo left over.
    public int AddAmmo(int numberOfAmmo)
    {
        RangeWeaponData data = (RangeWeaponData)itemData;

        int ammoToAdd = Mathf.Clamp(numberOfAmmo, 0, data.maxAmmoCount - data.currentAmmoCount);
        data.currentAmmoCount += ammoToAdd;
        return numberOfAmmo - ammoToAdd;
    }
}
