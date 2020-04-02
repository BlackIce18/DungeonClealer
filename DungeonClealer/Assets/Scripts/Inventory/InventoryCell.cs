using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class InventoryCell : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _itemCount;
    [SerializeField] private Image _iconField;
    public void Render(IItem item)
    {
        _itemCount = item.count;
        _iconField.sprite = item.UIIcon;
    }
}
