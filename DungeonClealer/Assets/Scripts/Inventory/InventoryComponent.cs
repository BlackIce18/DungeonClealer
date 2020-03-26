/**************************************************************************************************/
/** 	© 2017 NULLcode Studio. License: https://creativecommons.org/publicdomain/zero/1.0/deed.ru
/** 	Разработано в рамках проекта: http://null-code.ru/
/**                       ******   Внимание! Проекту нужна Ваша помощь!   ******
/** 	WebMoney: R209469863836, Z126797238132, E274925448496, U157628274347
/** 	Яндекс.Деньги: 410011769316504
/**************************************************************************************************/

using UnityEngine;
using System.Collections;

public class InventoryComponent : MonoBehaviour {

	[SerializeField] private int _limit = 99; // сколько предметов данного типа может быть в инвентаре
	[SerializeField] private Sprite _icon; // иконка предмета для инвентаря
	[SerializeField] private string _item = "myItem"; // идентификатор (имя предмета)
	[SerializeField] private InventoryEnum _size; // выбираем размер иконки

	public int limit
	{
		get{ return _limit; }
	}

	public Sprite icon
	{
		get{ return _icon; }
	}

	public string item
	{
		get{ return _item; }
	}

	public InventoryEnum size
	{
		get{ return _size; }
	}
}
