/**************************************************************************************************/
/** 	© 2017 NULLcode Studio. License: https://creativecommons.org/publicdomain/zero/1.0/deed.ru
/** 	Разработано в рамках проекта: http://null-code.ru/
/**                       ******   Внимание! Проекту нужна Ваша помощь!   ******
/** 	WebMoney: R209469863836, Z126797238132, E274925448496, U157628274347
/** 	Яндекс.Деньги: 410011769316504
/**************************************************************************************************/

using System.Collections;
using UnityEngine;

public class InventoryDrop : MonoBehaviour {

	private static InventoryDrop _internal;

	void Awake()
	{
		_internal = this;
	}

	public static void DropItem(InventoryComponent item, int count)
	{
		if(item == null || count <= 0) return;
		_internal.DropItem_internal(item, count);
	}

	void DropItem_internal(InventoryComponent item, int count)
	{
		for(int i = 0; i < count; i++)
		{
			// скрипт для сброса предметов, его нужно цеплять на точку, которая будет рядом с игроком
			// здесь также будут сбрасываться предметы, если они не поместились в инвентарь, которые были выданы через метод AddItem
			Instantiate(item, transform.position, Quaternion.identity);
		}
	}
}