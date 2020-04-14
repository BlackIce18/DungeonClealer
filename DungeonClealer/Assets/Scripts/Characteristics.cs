using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Characteristics:MonoBehaviour
{
    public int hp;
    public int mp;
    public int armor; // Броня

    public int strength;
    public int dexterity;
    public int intelligence;

    public int restoreHpPerSecond;
    public int restoreMpPerSecond;

    public float damage;
    public int defence; //Защита
    public float attackDistance;
    public float accuracy; // Точность

    public float speed;
    public float speedDamage;

    public float blocking;
    public float blockingChance;

    public float criticalChance;
    public float criticalDamage;

    public float bleedingChance;
    public float poisonChance;
    public float fireChance;

    public float holyDamage;
    public float darknessDamage;
    public float lightningDamage;
    public float waterDamage;
    public float fireDamage;
    public float poisonDamage;
    public float earthDamage;

    public float holyResistance;
    public float darknessResistance;
    public float lightningResistance;
    public float waterResistance;
    public float fireResistance;
    public float poisonResistance;
    public float earthResistance;
}