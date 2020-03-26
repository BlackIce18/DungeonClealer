/**************************************************************************************************/
/** 	© 2017 NULLcode Studio. License: https://creativecommons.org/publicdomain/zero/1.0/deed.ru
/** 	Разработано в рамках проекта: http://null-code.ru/
/**                       ******   Внимание! Проекту нужна Ваша помощь!   ******
/** 	WebMoney: R209469863836, Z126797238132, E274925448496, U157628274347
/** 	Яндекс.Деньги: 410011769316504
/**************************************************************************************************/

#if UNITY_EDITOR
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Inventory))]

public class InventoryEditor : Editor {

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		Inventory e = (Inventory)target;
		GUILayout.Label("Build Grid:", EditorStyles.boldLabel);
		if(GUILayout.Button("Create / Update"))
		{
			e.BuildGrid();
		}
	}
}
#endif
