using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GrapplingGunData", menuName = "Item/GrapplingGunData", order = 0)]
public class GrapplingGunData : EquipmentData
{
    public LayerMask grappableLayer;
    public float maxDistance = 100f;
}
