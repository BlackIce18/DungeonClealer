using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenu : MonoBehaviour
{
    private GameObject gameMenu;
    private bool isShowedMenu;
    // Start is called before the first frame update
    void Start()
    {
        gameMenu = GameObject.Find("GameMenu");
        isShowedMenu = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowGameMenu() {
        isShowedMenu = !isShowedMenu;
        for (int i = 0; i < gameMenu.transform.childCount; i++)
        {
            gameMenu.transform.GetChild(i).gameObject.SetActive(isShowedMenu);
        }
    }

    public void ShowGameMenuSettings() { 
        
    }
}
