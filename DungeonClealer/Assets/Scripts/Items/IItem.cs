using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public enum Rarity
{
    common,
    uncommon,
    rare,
    mythical,
    epic,
    legendary,
    unique
}

public interface IItem
{
    int id { get; }
    string Name { get; }
    Sprite UIIcon { get; }
    float cost { get; }
    TextMeshProUGUI count { get; }
    int level { get; }
    Rarity rarity { get; }
    string description { get; }
}
