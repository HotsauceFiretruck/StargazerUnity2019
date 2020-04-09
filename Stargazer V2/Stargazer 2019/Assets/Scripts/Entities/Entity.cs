using UnityEngine;
using System;
using System.Collections.Generic;

public class Entity : MonoBehaviour
{
    [NonSerialized]
    public Vector2 velocity = Vector2.zero;

    [NonSerialized]
    public Vector3 direction, position;

    [NonSerialized]
    public Dictionary<EquipmentType, Equipment> equipments =
        new Dictionary<EquipmentType, Equipment>()
        {
            {EquipmentType.Weapon, null},
            {EquipmentType.Footwear, null}
        };

    [NonSerialized]
    public Item currentHoldingItem;

    [NonSerialized]
    public float currentSpeed = 5.0f;

    public float turnSpeed = 10.0f;
    public float maxSpeed = 5.0f;

    public virtual void Death()
    {
        Destroy(this.gameObject);
    }
}
