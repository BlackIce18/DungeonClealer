using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameObject Settings;
    private GameObject mainMenu;

    private bool isShowedSettings;
    private bool isShowedMainMenu;

    private GameObject Tabs;

    // Start is called before the first frame update
    void Start()
    {
        Settings = GameObject.Find("SettingsInterface");
        mainMenu = GameObject.Find("MainMenu");

        isShowedSettings = Settings.transform.GetChild(0).gameObject.activeSelf;
        isShowedMainMenu = mainMenu.transform.GetChild(0).gameObject.activeSelf;
        Tabs = GameObject.Find("Tabs");
    }

    /*private bool ObjToggler(GameObject g, bool a) {
            foreach (Transform child in g.transform)
            {
                child.gameObject.SetActive(!a);
                ObjToggler(child.gameObject, a);
            }
            return !a;
        }*/
    public void ShowSettings()
    {
        //isShowedMainMenu = ObjToggler(MainMenu, isShowedMainMenu);
        isShowedMainMenu = !isShowedMainMenu;
        for (int i = 0; i < mainMenu.transform.childCount; i++)
        {
            mainMenu.transform.GetChild(i).gameObject.SetActive(isShowedMainMenu);
        }
        isShowedSettings = !isShowedSettings;
        for (int i = 0; i < Settings.transform.childCount; i++)
        {
            Settings.transform.GetChild(i).gameObject.SetActive(isShowedSettings);
        }

        //isShowedSettings = ObjToggler(Settings, isShowedSettings);
        foreach (Transform child in Tabs.transform)
        {
            child.gameObject.SetActive(isShowedSettings);
        }
    }
    public void CloseSettings() { 
        
    }
}
