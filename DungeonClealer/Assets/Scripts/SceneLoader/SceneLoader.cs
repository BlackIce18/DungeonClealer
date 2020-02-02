using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class SceneLoader : MonoBehaviour
{
    private int DottCount = 0;
    private TextMeshProUGUI Loading_text;
    // Start is called before the first frame update
    void Start()
    {
        Loading_text = GameObject.Find("Loading_text").GetComponent<TextMeshProUGUI>();
        StartCoroutine(Loading());
    }

    IEnumerator Loading() {
        while (true) {
            if (DottCount == 3)
            {
                Loading_text.text = "Загрузка";
                DottCount = 0;
            }
            else
            {
                Loading_text.text += ".";
                DottCount++;
            }
            yield return new WaitForSeconds(.75f);
        }
    }
}
