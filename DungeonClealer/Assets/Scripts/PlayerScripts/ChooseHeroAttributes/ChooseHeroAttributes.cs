using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ChooseHeroAttributes : MonoBehaviour
{
    public int AvailableAttributePoints = 5;
    public TextMeshProUGUI AvailablePoints;
    public TextMeshProUGUI StrPoints;
    public TextMeshProUGUI DexPoints;
    public TextMeshProUGUI IntPoints;

    public void Increase(TextMeshProUGUI PointsText) {
        if (AvailableAttributePoints > 0) {
            AvailableAttributePoints--;
            AvailablePoints.text = AvailableAttributePoints.ToString();
            PointsText.text = (Convert.ToInt32(PointsText.text)+1).ToString();
        }
    }
    public void Decrease(TextMeshProUGUI PointsText)
    {
        if (Convert.ToInt32(PointsText.text) > 1)
        {
            AvailableAttributePoints++;
            AvailablePoints.text = AvailableAttributePoints.ToString();
            PointsText.text = (Convert.ToInt32(PointsText.text) - 1).ToString();
        }
    }
}
