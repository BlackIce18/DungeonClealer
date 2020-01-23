using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChanger : MonoBehaviour
{
    public void ChangeSceneToIndx(int index) {
        SceneManager.LoadScene(index);
    }
    public void CloseApp() {
        Application.Quit();
    }
}
