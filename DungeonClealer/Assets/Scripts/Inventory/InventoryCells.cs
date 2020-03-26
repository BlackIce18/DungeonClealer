/**************************************************************************************************/
/** 	© 2017 NULLcode Studio. License: https://creativecommons.org/publicdomain/zero/1.0/deed.ru
/** 	Разработано в рамках проекта: http://null-code.ru/
/**                       ******   Внимание! Проекту нужна Ваша помощь!   ******
/** 	WebMoney: R209469863836, Z126797238132, E274925448496, U157628274347
/** 	Яндекс.Деньги: 410011769316504
/**************************************************************************************************/

using UnityEngine;
using System.Collections;

public class InventoryCells : MonoBehaviour {

	[SerializeField] private RectTransform _rectTransform;
	public string item { get; set; }
	public bool isLocked { get; set; }

	public RectTransform rectTransform
	{
		get{ return _rectTransform; }
	}

	public void SetRectTransform(RectTransform tr)
	{
		_rectTransform = tr;
	}
}
