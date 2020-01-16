using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChanger : MonoBehaviour
{
    private GameObject Settings;
    private GameObject MainMenu;

    private bool isShowedSettings;
    private bool isShowedMainMenu;

    private GameObject GraphicsTab;

    private GameSettings gameSettings;
    // Start is called before the first frame update
    void Start()
    {
        Settings = GameObject.Find("SettingsInterface");
        MainMenu = GameObject.Find("MainMenu");

        isShowedSettings = Settings.transform.GetChild(0).gameObject.activeSelf;
        isShowedMainMenu =  MainMenu.transform.GetChild(0).gameObject.activeSelf;
        GraphicsTab = GameObject.Find("GraphicsTab");
        gameSettings = GameObject.Find("GameSettings").GetComponent<GameSettings>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeSceneToIndx(int index) {
        SceneManager.LoadScene(index);
    }
    public void CloseApp() {
        Application.Quit();
    }
    /*private bool ObjToggler(GameObject g, bool a) {
        foreach (Transform child in g.transform)
        {
            child.gameObject.SetActive(!a);
            ObjToggler(child.gameObject, a);
        }
        return !a;
    }*/
    public void ShowSettings() {
        //isShowedMainMenu = ObjToggler(MainMenu, isShowedMainMenu);
        isShowedMainMenu = !isShowedMainMenu;
        for (int i = 0; i < MainMenu.transform.childCount; i++)
        {
            MainMenu.transform.GetChild(i).gameObject.SetActive(isShowedMainMenu);
        }
        isShowedSettings = !isShowedSettings;
        for (int i = 0; i < Settings.transform.childCount; i++)
        {
            Settings.transform.GetChild(i).gameObject.SetActive(isShowedSettings);
        }

        //isShowedSettings = ObjToggler(Settings, isShowedSettings);

        foreach (Transform child in GraphicsTab.transform)
        {
            child.gameObject.SetActive(true);
        }

    }

}
