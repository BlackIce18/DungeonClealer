using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

[CreateAssetMenu(menuName ="Item")]
public class AssetItem : ScriptableObject, IItem
{
    public int id => _id;
    public string Name => _name;
    public Sprite UIIcon => _uiIcon;

    public float cost => _cost;

    public TextMeshProUGUI count => _count;

    public int level => _level;

    public Rarity rarity => _rarity;

    public string description => _description;

    [SerializeField] private int _id;
    [SerializeField] private string _name;
    [SerializeField] private Sprite _uiIcon;
    [SerializeField] private float _cost;
    [SerializeField] private TextMeshProUGUI _count;
    [SerializeField] private int _level;
    [SerializeField] private Rarity _rarity;
    [SerializeField] private string _description;
}
