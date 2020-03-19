using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private GameObject gameMenu;
    private GameObject Inventory;
    private GameObject SkillMenu;
    private bool isShowedMenu;
    private bool isShowedInventory;
    private bool isShowedSkillMenu;
    // Start is called before the first frame update
    void Start()
    {
        gameMenu = GameObject.Find("GameMenu");
        Inventory = GameObject.Find("Inventory");
        SkillMenu = GameObject.Find("SkillMenu");
        isShowedMenu = false;
        isShowedInventory = false;
        isShowedSkillMenu = false;
    }
    private bool Shower(bool IsShowed, GameObject Item) {
        IsShowed = !IsShowed;
        for (int i = 0; i < Item.transform.childCount; i++)
        {
            Item.transform.GetChild(i).gameObject.SetActive(IsShowed);
        }
        return IsShowed;
    }
    public void ShowGameMenu() {
        isShowedMenu = Shower(isShowedMenu, gameMenu);
    }
    public void ShowInventory()
    {
        isShowedInventory = Shower(isShowedInventory, Inventory);
    }

    public void ShowSkillMenu() {
        isShowedSkillMenu = Shower(isShowedSkillMenu, SkillMenu);
    }
    public void ShowGameMenuSettings() { 
        
    }
}
