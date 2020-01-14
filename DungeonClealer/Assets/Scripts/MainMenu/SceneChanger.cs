using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneChanger : MonoBehaviour
{
    private GameObject Settings;
    private GameObject MainMenu;

    private bool isShowedSettings;
    private bool isShowedMainMenu;

    private GameObject GraphicsTab;
    // Start is called before the first frame update
    void Start()
    {
        Settings = GameObject.Find("Settings");
        MainMenu = GameObject.Find("MainMenu");

        isShowedSettings = !Settings.activeSelf;
        isShowedMainMenu =  MainMenu.activeSelf;
        GraphicsTab = GameObject.Find("GraphicsTab");
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
    private bool ObjToggler(GameObject g, bool a) {
        foreach (Transform child in g.transform)
        {
            child.gameObject.SetActive(!a);
            ObjToggler(child.gameObject, a);
        }
        return !a;
    }
    public void ShowSettings() {
        isShowedMainMenu = ObjToggler(MainMenu, isShowedMainMenu);
        isShowedSettings = ObjToggler(Settings, isShowedSettings);
        foreach (Transform child in GraphicsTab.transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
