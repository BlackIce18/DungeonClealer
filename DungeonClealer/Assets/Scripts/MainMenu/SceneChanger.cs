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
    // Start is called before the first frame update
    void Start()
    {
        Settings = GameObject.Find("SettingsMenu");
        MainMenu = GameObject.Find("MainMenu");
        isShowedSettings = !Settings.activeSelf;
        isShowedMainMenu = MainMenu.activeSelf;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void changeSceneToIndx(int index) {
        SceneManager.LoadScene(index);
    }
    public void CloseApp() {
        Application.Quit();
    }
    private bool objToggler(GameObject g, bool a) {
        foreach (Transform child in g.transform)
        {
            child.gameObject.SetActive(!a);
            objToggler(child.gameObject, a);
        }
        return !a;
    }
    public void ShowSettings() {
        isShowedMainMenu = objToggler(MainMenu, isShowedMainMenu);
        isShowedSettings = objToggler(Settings, isShowedSettings);
    }
}
