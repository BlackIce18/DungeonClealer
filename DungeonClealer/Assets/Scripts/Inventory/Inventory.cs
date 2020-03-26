﻿/**************************************************************************************************/
/** 	© 2017 NULLcode Studio. License: https://creativecommons.org/publicdomain/zero/1.0/deed.ru
/** 	Разработано в рамках проекта: http://null-code.ru/
/**                       ******   Внимание! Проекту нужна Ваша помощь!   ******
/** 	WebMoney: R209469863836, Z126797238132, E274925448496, U157628274347
/** 	Яндекс.Деньги: 410011769316504
/**************************************************************************************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour {

	private enum ProjectMode {Project3D, Project2D};

	[SerializeField] private string prefabPath = "Prefab"; // папка с префабами предметов в Resources
	[SerializeField] private KeyCode key; // клавиша вкл/выкл инвентаря
	[SerializeField] private GameObject inventory; // объект который вкл/выкл
	[SerializeField] private ProjectMode mode = ProjectMode.Project3D;
	[SerializeField] private RectTransform overlap; // здесь размещается иконка, которая в данный момент перетаскивается (чтобы была поверх других)
	[SerializeField] private RectTransform content; // здесь все иконки
	[SerializeField] private RectTransform grid; // для сетки
	[SerializeField] private Button sortButton;
	[SerializeField] private Button exitButton;
	[SerializeField] private InventoryCells[] cells;
	[SerializeField] private InventoryIcon[] icons;
	[SerializeField] private GameObject dropParent;
	[SerializeField] private Button applyDrop;
	[SerializeField] private Button cancelDrop;
	[SerializeField] private Slider sliderDrop;
	[SerializeField] private Text textDrop;
	[Header("Настройки сетки инвентаря:")]
	[SerializeField] private int width = 5;
	[SerializeField] private int height = 5;
	[SerializeField] private int layerMask = 1; // создайте отдельный слой для ячеек инвентаря и укажите его номер

	private bool isCrypt = false; // шифрование данных
	private string fileName = "Inventory.data"; // файл сохранения
	private string nullCell = "*"; // символ, который обозначает пустую ячейку
	private List<InventoryCells> targetCell;
	private List<InventoryCells> lastCell;
	private InventoryComponent current;
	private InventoryIcon icon, iconDrop;
	private List<InventoryIcon> pIcon;
	private InventoryCells[,] field;
	private Vector2 lastPosition;
	private static Inventory _internal;
	private static bool _Active;
	private bool isDrop;
	private Vector3 offset;
	private PointerEventData pointerData = new PointerEventData(EventSystem.current);
	private List<RaycastResult> resultsData = new List<RaycastResult>();
	private InventoryComponent[] items;

	public static bool isActive // можно проверить открыт инвентарь или нет
	{
		get{ return _Active; }
	}

	// проверка, есть ли такой предмет, по имени предмета "item"
	public static bool CheckItem(string item)
	{
		if(string.IsNullOrEmpty(item)) return false;
		return _internal.Check_internal(item);
	}

	// использовать предмет(ы), указываем имя "item" и количество "count" (не меньше одного)
	public static void UseItem(string item, int count)
	{
		if(string.IsNullOrEmpty(item) || count <= 0) return;
		_internal.UseItem_internal(item, count);
	}

	// добавить предмет(ы), указываем имя "item" и количество "count" (не меньше одного)
	public static void AddItem(string item, int count)
	{
		if(string.IsNullOrEmpty(item) || count <= 0) return;
		_internal.AddItem_internal(item, count);
	}

	void AddItem_internal(string item, int count)
	{
		foreach(InventoryComponent i in items)
		{
			if(i.item.CompareTo(item) == 0)
			{
				AddQuickly(i, count);
				return;
			}
		}

		Debug.Log(this + " --> item: [ " + item + " ] | Предмет(ы) добавить невозможно! Запрашиваемого предмета не существует!");
	}

	bool Check_internal(string item)
	{
		foreach(InventoryIcon i in pIcon)
		{
			if(i.item.CompareTo(item) == 0) return true;
		}

		return false;
	}

	void UseItem_internal(string value, int count)
	{
		int j = 0;
		foreach(InventoryIcon i in pIcon)
		{
			if(i.item.CompareTo(value) == 0 && i.counter > 1)
			{
				i.counter -= count;
				if(i.counter <= 0)
				{
					SetUnlock(i.item);
					pIcon.RemoveAt(j);
					Destroy(i.gameObject);
					return;
				}
				i.iconCountText.text = i.counter.ToString();
				return;
			}
			else if(i.item.CompareTo(value) == 0 && i.counter == 1)
			{
				SetUnlock(i.item);
				pIcon.RemoveAt(j);
				Destroy(i.gameObject);
				return;
			}

			j++;
		}

		Debug.Log(this + " --> item: [ " + value + " ] | Предмет(ы) использовать невозможно! Запрашиваемого предмета не существует!");
	}

	void LoadSettings()
	{
		if(!File.Exists(Path())) return;

		InventoryIcon tmp = null;
		string item = string.Empty;
		int counter = 0;
		InventoryEnum size = InventoryEnum.size1x1;
		Vector2 pos = Vector2.zero;

		string[] field = new string[]{};

		StreamReader reader = new StreamReader(Path());

		while(!reader.EndOfStream)
		{
			string value = Crypt(reader.ReadLine());

			string[] result = value.Split(new char[]{'='});

			if(result.Length == 1 && item == string.Empty)
			{
				item = value.Trim(new char[]{'[', ']'});
			}

			switch(result[0]) // фильтруем ключи
			{
			case "size":
				size = (InventoryEnum)System.Enum.Parse(typeof(InventoryEnum), result[1]); // string переводим в enum
				break;
			case "counter":
				counter = int.Parse(result[1]);
				break;
			case "pX":
				pos.x = float.Parse(result[1]);
				break;
			case "pY":
				pos.y = float.Parse(result[1]);
				break;
			case "field":
				field = result[1].Split(new char[]{','});
				break;
			}

			if(value == string.Empty) 
			{
				tmp = SetIcon(size, item, content, counter);
				tmp.isInside = true;
				tmp.iconImage.raycastTarget = true;
				tmp.iconCountObject.SetActive(true);
				tmp.rectTransform.localPosition = pos;
				pIcon.Add(tmp);
				item = string.Empty;
			}
		}

		if(field.Length > 0)
		{
			for(int i = 0; i < cells.Length; i++)
			{
				if(field[i].CompareTo(nullCell) == 0)
				{
					cells[i].item = string.Empty;
					cells[i].isLocked = false;
				}
				else
				{
					cells[i].item = field[i];
					cells[i].isLocked = true;
				}
			}	
		}

		reader.Close();
	}

	string Crypt(string text)
	{
		if(!isCrypt) return text;

		string result = string.Empty;
		foreach (char j in text)
		{
			result += (char)((int)j ^ 47);
		}
		return result;
	}

	void SaveSettings()
	{
		if(pIcon.Count == 0) return;

		StreamWriter writer = new StreamWriter(Path());

		foreach(InventoryIcon i in pIcon)
		{
			writer.WriteLine(Crypt("[" + i.item + "]"));
			writer.WriteLine(Crypt("size=" + i.size));
			writer.WriteLine(Crypt("counter=" + i.counter));
			writer.WriteLine(Crypt("pX=" + i.rectTransform.localPosition.x));
			writer.WriteLine(Crypt("pY=" + i.rectTransform.localPosition.y));
			writer.WriteLine(string.Empty); // пустая строка, обязательный ключ, запускает создание иконки
		}

		string field = string.Empty;
		foreach(InventoryCells i in cells)
		{
			if(field.Length > 0) field += ",";
			if(string.IsNullOrEmpty(i.item)) field += nullCell; else field += i.item;
		}

		writer.WriteLine(Crypt("field=" + field));

		writer.Close();
		Debug.Log(this + " --> Сохранение инвентаря игрока: " + Path());
	}

	string Path() // путь сохранения
	{
		return Application.persistentDataPath + "/" + fileName;
	}

	public static void BeginDrag(InventoryIcon curIcon) // функция для иконок инвентаря
	{
		_internal.BeginDrag_internal(curIcon);
	}

	public void BeginDrag_internal(InventoryIcon curIcon)
	{
		offset = curIcon.rectTransform.position - Input.mousePosition;
		lastPosition = curIcon.rectTransform.position;
		icon = curIcon;
		icon.iconImage.raycastTarget = false;
		SetUnlock(curIcon.item);
		icon.rectTransform.SetParent(overlap);
	}

	void SetUnlock(string item)
	{
		lastCell = new List<InventoryCells>();
		foreach(InventoryCells cell in cells)
		{
			if(cell.item.CompareTo(item) == 0)
			{
				lastCell.Add(cell);
				cell.item = string.Empty;
				cell.isLocked = false;
			}
		}
	}

	void RemoveItem(string item, int count)
	{
		int j = 0;
		foreach(InventoryIcon i in pIcon)
		{
			if(i.item.CompareTo(item) == 0)
			{
				i.counter -= count;
				if(i.counter <= 0)
				{
					pIcon.RemoveAt(j);
					Destroy(i.gameObject);
					return;
				}
				i.iconCountText.text = i.counter.ToString();
				return;
			}
			j++;
		}
	}

	void ClearField()
	{
		foreach(InventoryCells cell in cells)
		{
			cell.item = string.Empty;
			cell.isLocked = false;
		}
	}

	void Awake()
	{
		inventory.SetActive(false);
		dropParent.SetActive(false);
		_Active = false;
		_internal = this;
		pIcon = new List<InventoryIcon>();
		field = new InventoryCells[width, height];

		int j = 0;
		for(int y = 0; y < height; y++)
		{
			for(int x = 0; x < width; x++)
			{
				cells[j].item = string.Empty;
				field[x, y] = cells[j];
				j++;
			}
		}

		items = Resources.LoadAll<InventoryComponent>(prefabPath);

		sliderDrop.wholeNumbers = true;
		applyDrop.onClick.AddListener(() => {DropApply();});
		cancelDrop.onClick.AddListener(() => {DropCancel();});
		exitButton.onClick.AddListener(() => {Hide();});
		sortButton.onClick.AddListener(() => {Sort();});
		sliderDrop.onValueChanged.AddListener(delegate{DropSlider(sliderDrop);});

		LoadSettings();
	}

	InventoryComponent GetPrefab(string item)
	{
		foreach(InventoryComponent i in items)
		{
			if(i.item.CompareTo(item) == 0) return i;
		}

		return null;
	}

	void DropApply()
	{
		int i = (int)sliderDrop.value;
		RemoveItem(iconDrop.item, i);

		ResetDrop();

		InventoryDrop.DropItem(GetPrefab(iconDrop.item), i);
		dropParent.SetActive(false);
		isDrop = false;
	}

	void ResetDrop()
	{
		if(iconDrop.counter > 0)
		{
			iconDrop.gameObject.SetActive(true);

			foreach(InventoryCells cell in lastCell)
			{
				cell.item = iconDrop.item;
				cell.isLocked = true;
			}

			iconDrop.iconImage.raycastTarget = true;
			iconDrop.rectTransform.SetParent(content);
			iconDrop.rectTransform.position = lastPosition;
		}
	}

	void DropCancel()
	{
		ResetDrop();
		isDrop = false;
		dropParent.SetActive(false);
	}

	void DropSlider(Slider slider)
	{
		textDrop.text = sliderDrop.value.ToString();
	}

	void ShowDrop(InventoryIcon i)
	{
		iconDrop = i;
		isDrop = true;
		sliderDrop.minValue = 1;
		sliderDrop.maxValue = icon.counter;
		sliderDrop.value = 1;
		textDrop.text = "1";
		dropParent.SetActive(true);
		icon.gameObject.SetActive(false);
	}

	InventoryComponent GetCurrent() // запрос компонента с объекта
	{
		Vector3[] worldCorners = new Vector3[4];
		content.GetWorldCorners(worldCorners);
		if(PointInside(Input.mousePosition, worldCorners)) return null; // отмена, если курсор в поле инвентаря

		Transform tr = null;

		if(mode == ProjectMode.Project3D)
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if(Physics.Raycast(ray, out hit)) tr = hit.transform;
		}
		else
		{
			RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
			if(hit.transform != null) tr = hit.transform;
		}

		if(tr == null) return null;

		InventoryComponent t = tr.GetComponent<InventoryComponent>();
		if(t != null) return t;

		return null;
	}

	Sprite GetSprite(string item)
	{
		foreach(InventoryComponent i in items)
		{
			if(i.item.CompareTo(item) == 0) return i.icon;
		}

		return null;
	}

	InventoryIcon SetIcon(InventoryEnum size, string item, RectTransform targetRect, int count) // создание и настройка новой иконки
	{
		InventoryIcon target = null;

		foreach(InventoryIcon i in icons)
		{
			if(i.size == size)
			{
				target = i;
				break;
			}
		}

		if(target)
		{
			InventoryIcon clone = Instantiate(target) as InventoryIcon;
			clone.iconImage.sprite = GetSprite(item);
			clone.iconCountObject.SetActive(false);
			clone.gameObject.name = item;
			clone.item = item;
			clone.isInside = false;
			clone.counter = count;
			clone.iconCountText.text = count.ToString();
			clone.iconImage.raycastTarget = false;
			clone.rectTransform.SetParent(targetRect);
			clone.rectTransform.localScale = Vector3.one;
			clone.rectTransform.position = Input.mousePosition;
			clone.gameObject.SetActive(true);
			return clone;
		}

		return null;
	}

	void Sort()
	{
		if(isDrop) return;

		ClearField();

		for(int i = 0; i < pIcon.Count; i++)
		{
			icon = pIcon[i];
			targetCell = GetCells(icon.size);
			AddCurrentIcon(true);
		}
	}

	void Control() // управление инвентарем
	{
		if(icon != null) icon.rectTransform.position = offset + Input.mousePosition;

		if(Input.GetMouseButtonDown(0))
		{
			current = GetCurrent();
			if(current == null) return;
			icon = SetIcon(current.size, current.item, overlap, 1);
			offset = Vector3.zero;
			current.gameObject.SetActive(false);
		}
		else if(Input.GetMouseButtonUp(0) && icon != null)
		{
			if(!IsInside(icon.element) && icon.isInside)
			{
				if(icon.counter > 1)
				{
					ShowDrop(icon);
					icon = null;
				}
				else
				{
					InventoryDrop.DropItem(GetPrefab(icon.item), 1);
					RemoveItem(icon.item, 1);
					ResetCurrent();
				}

				return;
			}

			if(current != null && IsInside(icon.element) && IsSimilar(current, 1))
			{
				Destroy(icon.gameObject);
				Destroy(current.gameObject);
				return;
			}

			if(!icon.isInside && IsOverlap(icon.element))
			{
				ResetCurrent();
				return;
			}
			else if(icon.isInside && IsOverlap(icon.element))
			{
				ResetDrag();
				return;
			}

			AddCurrentIcon(false);
			resultsData.Clear();
		}
		else if(Input.GetMouseButtonDown(1))
		{
			current = GetCurrent();
			if(current != null)
			{
				AddQuickly(current, 1);
				Destroy(current.gameObject);
			}
		}
	}

	void Show()
	{
		_Active = true;
		inventory.SetActive(true);
	}

	void Hide()
	{
		if(isDrop) return;
		_Active = false;
		inventory.SetActive(false);
		SaveSettings();
	}

	void LateUpdate()
	{
		if(isDrop) return;

		if(!_Active && Input.GetKeyDown(key)) Show();
		else if(_Active && Input.GetKeyDown(key)) Hide();

		if(_Active) Control();
	}

	bool IsSimilar(InventoryComponent val, int count) // поиск похожей иконки
	{
		for(int i = 0; i < pIcon.Count; i++)
		{
			if(pIcon[i].item.CompareTo(val.item) == 0)
			{
				if((pIcon[i].counter + count) > val.limit)
				{
					InventoryDrop.DropItem(GetPrefab(val.item), count);
					Debug.Log(this + " --> item: [ " + val.item + " ] count: [ " + count + " ] | Предмет(ы) добавить невозможно! Превышен лимит для данного типа.");
					return true;
				}
				pIcon[i].counter += count;
				pIcon[i].iconCountText.text = pIcon[i].counter.ToString();
				return true;
			}
		}

		return false;
	}

	void AddQuickly(InventoryComponent val, int count) // быстрое добавление
	{
		if(IsSimilar(val, count))
		{
			return;
		}

		targetCell = GetCells(val.size);

		if(targetCell.Count > 0)
		{
			icon = SetIcon(val.size, val.item, content, count);
			if(!icon) return;
			AddCurrentIcon(false);
		}
		else
		{
			InventoryDrop.DropItem(val, count);
			Debug.Log(this + " --> item: [ " + val.item + " ] count: [ " + count + " ] | Предмет(ы) добавить невозможно! В инвентаре нет места!");
		}
	}

	List<InventoryCells> GetCells(InventoryEnum value)
	{
		// внимание, если вы соберете новую иконку со своими размерами
		// ее необходимо добавить в InventoryEnum
		// и соответственно интегрировать в общую логику ниже

		int xx = 0, yy = 0;
		// определение области поиска, обрезка, чтобы не выйти за рамки двухмерного массива
		switch(value)
		{
		case InventoryEnum.size1x3:
			yy = 2;
			break;
		case InventoryEnum.size1x2:
			yy = 1;
			break;
		case InventoryEnum.size2x1:
			xx = 1;
			break;
		case InventoryEnum.size3x1:
			xx = 2;
			break;
		case InventoryEnum.size2x2:
			xx = 1; 
			yy = 1;
			break;
		case InventoryEnum.size3x2:
			xx = 2; 
			yy = 1;
			break;
		case InventoryEnum.size2x3:
			xx = 1; 
			yy = 2;
			break;
		}

		targetCell = new List<InventoryCells>();
		for(int y = 0; y < height-yy; y++)
		{
			for(int x = 0; x < width-xx; x++)
			{
				switch(value) // ищем место для новой иконки
				{
				case InventoryEnum.size1x1:
					if(!field[x, y].isLocked)
					{
						targetCell.Add(field[x, y]);
						return targetCell;
					}
					break;
				case InventoryEnum.size1x3:
					if(!field[x, y].isLocked && !field[x, y+1].isLocked && !field[x, y+2].isLocked)
					{
						targetCell.Add(field[x, y]);
						targetCell.Add(field[x, y+1]);
						targetCell.Add(field[x, y+2]);
						return targetCell;
					}
					break;
				case InventoryEnum.size1x2:
					if(!field[x, y].isLocked && !field[x, y+1].isLocked)
					{
						targetCell.Add(field[x, y]);
						targetCell.Add(field[x, y+1]);
						return targetCell;
					}
					break;
				case InventoryEnum.size2x1:
					if(!field[x, y].isLocked && !field[x+1, y].isLocked)
					{
						targetCell.Add(field[x, y]);
						targetCell.Add(field[x+1, y]);
						return targetCell;
					}
					break;
				case InventoryEnum.size3x1:
					if(!field[x, y].isLocked && !field[x+1, y].isLocked && !field[x+2, y].isLocked)
					{
						targetCell.Add(field[x, y]);
						targetCell.Add(field[x+1, y]);
						targetCell.Add(field[x+2, y]);
						return targetCell;
					}
					break;
				case InventoryEnum.size2x2:
					if(!field[x, y].isLocked && !field[x+1, y].isLocked && !field[x, y+1].isLocked && !field[x+1, y+1].isLocked)
					{
						targetCell.Add(field[x, y]);
						targetCell.Add(field[x+1, y]);
						targetCell.Add(field[x, y+1]);
						targetCell.Add(field[x+1, y+1]);
						return targetCell;
					}
					break;
				case InventoryEnum.size3x2:
					if(!field[x, y].isLocked && !field[x+1, y].isLocked && !field[x, y+1].isLocked && !field[x+1, y+1].isLocked && !field[x+2, y].isLocked && !field[x+2, y+1].isLocked)
					{
						targetCell.Add(field[x, y]);
						targetCell.Add(field[x+1, y]);
						targetCell.Add(field[x+2, y]);
						targetCell.Add(field[x, y+1]);
						targetCell.Add(field[x+1, y+1]);
						targetCell.Add(field[x+2, y+1]);
						return targetCell;
					}
					break;
				case InventoryEnum.size2x3:
					if(!field[x, y].isLocked && !field[x+1, y].isLocked && !field[x, y+1].isLocked && !field[x+1, y+1].isLocked && !field[x, y+2].isLocked && !field[x+1, y+2].isLocked)
					{
						targetCell.Add(field[x, y]);
						targetCell.Add(field[x+1, y]);
						targetCell.Add(field[x, y+1]);
						targetCell.Add(field[x+1, y+1]);
						targetCell.Add(field[x, y+2]);
						targetCell.Add(field[x+1, y+2]);
						return targetCell;
					}
					break;
				}
			}
		}

		return targetCell;
	}

	void ResetDrag()
	{
		foreach(InventoryCells cell in lastCell)
		{
			cell.item = icon.item;
			cell.isLocked = true;
		}

		icon.iconImage.raycastTarget = true;
		icon.rectTransform.SetParent(content);
		icon.rectTransform.position = lastPosition;
		icon = null;
	}

	void ResetCurrent()
	{
		Destroy(icon.gameObject);
		if(!current) return;
		current.gameObject.SetActive(true);
		current = null;
	}

	void AddCurrentIcon(bool isSort) // добавление иконки
	{
		Vector3 p1 = targetCell[0].rectTransform.position;
		Vector3 p2 = targetCell[targetCell.Count-1].rectTransform.position;
		Vector3 pos = (p1 - p2)/2;
		icon.transform.position = p2 + pos;

		foreach(InventoryCells cell in targetCell)
		{
			cell.item = icon.item;
			cell.isLocked = true;
		}

		if(!icon.isInside && !isSort) pIcon.Add(icon);
		icon.rectTransform.SetParent(content);
		icon.isInside = true;
		icon.iconImage.raycastTarget = true;
		icon.iconCountObject.SetActive(true);
		icon = null;
		if(current != null) Destroy(current.gameObject);
	}

	bool PointInside(Vector3 position, Vector3[] worldCorners)
	{
		if(position.x > worldCorners[0].x && position.x < worldCorners[2].x 
			&& position.y > worldCorners[0].y && position.y < worldCorners[2].y)
		{
			return true;
		}

		return false;
	}

	bool IsInside(RectTransform[] rectTransform) // проверка, объект внутри поля или нет
	{
		Vector3[] worldCorners = new Vector3[4];
		content.GetWorldCorners(worldCorners);
		foreach(RectTransform tr in icon.element)
		{
			if(!PointInside(tr.position, worldCorners)) return false;
		}

		return true;
	}

	InventoryCells RaycastUI(Vector3 position) // рейкаст по UI клеткам инвентаря
	{
		pointerData.position = position;
		EventSystem.current.RaycastAll(pointerData, resultsData);

		if(resultsData.Count > 0 && resultsData[0].gameObject.layer == layerMask)
		{
			return resultsData[0].gameObject.GetComponent<InventoryCells>();
		}

		return null;
	}

	bool IsOverlap(RectTransform[] rectTransform) // проверка на перекрытие
	{
		targetCell = new List<InventoryCells>();
		foreach(RectTransform tr in rectTransform)
		{
			InventoryCells t = RaycastUI(tr.position);
			if(t == null || t.isLocked) return true;
			targetCell.Add(t);
		}

		if(targetCell.Count == 0) return true;

		return false;
	}

	#if UNITY_EDITOR
	// Внимание! Если изменить размер ячейки инвентаря, то необходимо соответствующим образом изменить и размеры самих иконок.
	private int cellsSize = 50; // ширина и высота одной клетки (иконки типа 1:1)
	public Sprite cellSprite;
	public Color cellColor = Color.white;
	public void BuildGrid() // инструмент для создания сетки, используется только в редакторе
	{
		foreach(InventoryCells cell in cells)
		{
			if(cell) DestroyImmediate(cell.gameObject);
		}
		Vector2 delta = new Vector2(cellsSize * width, cellsSize * height);
		grid.sizeDelta = delta;
		content.sizeDelta = delta;
		overlap.sizeDelta = delta;
		RectTransform sample = new GameObject().AddComponent<RectTransform>();
		sample.gameObject.layer = layerMask;
		sample.gameObject.AddComponent<Image>().sprite = cellSprite;
		sample.gameObject.GetComponent<Image>().color = cellColor;
		sample.gameObject.AddComponent<InventoryCells>().SetRectTransform(sample);
		sample.sizeDelta = new Vector2(cellsSize, cellsSize);
		float posX = -cellsSize * width/2 - cellsSize/2;
		float posY = cellsSize * height/2 + cellsSize/2;
		float Xreset = posX;
		int i = 0;
		cells = new InventoryCells[width*height];
		for(int y = 0; y < height; y++)
		{
			posY -= cellsSize;
			for(int x = 0; x < width; x++)
			{
				posX += cellsSize;
				RectTransform tr = Instantiate(sample) as RectTransform;
				tr.SetParent(grid);
				tr.localScale = Vector3.one;
				tr.anchoredPosition = new Vector2(posX, posY);
				tr.name = "Cell_" + i;
				cells[i] = tr.GetComponent<InventoryCells>();
				i++;
			}
			posX = Xreset;
		}

		DestroyImmediate(sample.gameObject);
	}
	#endif
}
