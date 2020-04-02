using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : Item
{
    public Characteristics characteristics;
    public Characteristics requiredCharacteristics;
    public enum EquipmentType { 
        helm,
        armor,
        pants,
        boots,
        gloves,
        belt,
        jewel,
        ring,
        weapon,
        shield
    }
    public EquipmentType equipmentType;
}
