using UnityEngine;

[CreateAssetMenu(fileName = "RocketBoot", menuName = "Item/RocketBootData", order = 0)]
public class RocketBootData : EquipmentData
{
    public float boostForce = 2.0f;
    public float maxBoostReloadTime = 1.0f; //Seconds
    public float currentReloadTime = 0.0f;
}
