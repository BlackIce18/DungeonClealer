using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private GameObject gameMenu;
    private GameObject Inventory;
    private bool isShowedMenu;
    private bool isShowedInventory;
    // Start is called before the first frame update
    void Start()
    {
        gameMenu = GameObject.Find("GameMenu");
        Inventory = GameObject.Find("Inventory");
        isShowedMenu = false;
        isShowedInventory = false;
    }

    public void ShowGameMenu() {
        isShowedMenu = !isShowedMenu;
        for (int i = 0; i < gameMenu.transform.childCount; i++)
        {
            gameMenu.transform.GetChild(i).gameObject.SetActive(isShowedMenu);
        }
    }
    public void ShowInventory()
    {
        isShowedInventory = !isShowedInventory;
        for (int i = 0; i < Inventory.transform.childCount; i++)
        {
            Inventory.transform.GetChild(i).gameObject.SetActive(isShowedInventory);
        }
    }
    public void ShowGameMenuSettings() { 
        
    }
}
