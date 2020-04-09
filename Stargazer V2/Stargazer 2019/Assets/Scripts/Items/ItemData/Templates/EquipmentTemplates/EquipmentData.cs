using UnityEngine;

public class EquipmentData : ItemData
{
    [Space]
    public EquipmentType equipmentType;
}

public enum EquipmentType
{
    Weapon,
    Footwear
}
