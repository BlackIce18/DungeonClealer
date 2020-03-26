using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class SceneChanger : MonoBehaviour
{
    private void Start()
    {
        if (GameObject.Find("TransitionImage"))
        {
            GameObject.Find("TransitionImage").GetComponent<Animation>().Play();
            StartCoroutine(ShowDungeonName());

        }
    }
    IEnumerator ShowDungeonName() {
        yield return new WaitForSeconds(1.25f);
        GameObject.Find("DungeonName").GetComponent<Animation>().Play();
    }
    public void ChangeSceneToIndx(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(sceneIndex);
    }
    public void ChangeSceneToIndxWithLoad(int sceneIndex) {
        StartCoroutine(LoadAsync(sceneIndex));
    }
    public void CloseApp() {
        Application.Quit();
    }
    IEnumerator LoadAsync(int sceneIndex)
    {
        SceneManager.LoadSceneAsync(3);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        
        while (!operation.isDone) {

            yield return null;
        }
    }
}
