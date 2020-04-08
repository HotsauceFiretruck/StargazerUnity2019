using UnityEngine;
using System;

public abstract class Equipment : MonoBehaviour {

    public Entity ownerEntity;

    protected int currentAmmoCount = 10;
    protected int maxAmmoCount = 10;
    [NonSerialized]
    public string id;

    public abstract void OnActivate();

    public int GetAmmoCount() {
        return currentAmmoCount;
    }

    //If the number of ammo given exceed the max ammo count, returns the amount of ammo left over.
    public int SetAmmoCount(int value) {
        int ammoToAdd = Mathf.Clamp(value, 0, maxAmmoCount - currentAmmoCount);
        currentAmmoCount += ammoToAdd;
        return value - ammoToAdd;
    }
}
