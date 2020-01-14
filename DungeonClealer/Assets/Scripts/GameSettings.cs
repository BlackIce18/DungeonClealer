using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameSettings : MonoBehaviour
{
    public AudioSource MusicVolume;
    public Slider SliderMusicVolume;
    public bool mute;

    private enum Settingstabs {
        GraphicsTab = 0,
        AudioTab = 1,
        ControlsTab = 2
    }

    public GameObject[] tabsText;
    public GameObject[] SettingsTabs;

    private Color ActiveColor = new Color(173 / 255.0f, 138 / 255.0f, 138 / 255.0f);
    private Color DefaultColor = new Color(255 / 255.0f, 255 / 255.0f, 255 / 255.0f);

    // Start is called before the first frame update
    void Start()
    {
        mute = Camera.main.GetComponent<AudioSource>().mute;
        (tabsText[0]).GetComponent<TextMeshProUGUI>().color = ActiveColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MuteSound() {
        mute = !mute;
        Camera.main.GetComponent<AudioSource>().mute = mute;
    }
    public void ChangeMusicVoice() {
        AudioListener.volume = SliderMusicVolume.value;
    }

    public void SaveAll() { 
        
    }

    public void ChangeSettingsTab(int TabIndx) {
        for (int i = 0; i < SettingsTabs.Length; i++) {
            if (i != TabIndx) {
                foreach (Transform child in SettingsTabs[i].transform)
                {
                    child.gameObject.SetActive(false);
                }
                (tabsText[i]).GetComponent<TextMeshProUGUI>().color = DefaultColor;
            } 
        }
        //TogglerSettings.ToggleSettings(SettingsTabs[TabIndx], false);
        (tabsText[TabIndx]).GetComponent<TextMeshProUGUI>().color = ActiveColor;
        foreach (Transform child in SettingsTabs[TabIndx].transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
