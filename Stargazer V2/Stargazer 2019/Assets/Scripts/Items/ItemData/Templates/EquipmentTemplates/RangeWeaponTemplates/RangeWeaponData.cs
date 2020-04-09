using UnityEngine;
using System;

[CreateAssetMenu(fileName = "RangeWeaponData", menuName = "Item/RangeWeaponData", order = 0)]
public class RangeWeaponData : EquipmentData
{
    [Space]
    [NonSerialized]
    public float currentReloadTime = 0.0f;

    public int maxAmmoCount = 20;
    public int currentAmmoCount = 20;
    public float damageModifier = 1.0f;
    public float maxReloadTime = 1.0f;
    public float ammoSpeed = 1.0f;
    public float ammoRange = 100.0f;

    public GameObject ammoType;
}
