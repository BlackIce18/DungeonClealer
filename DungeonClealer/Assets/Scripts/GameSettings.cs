using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;
public class GameSettings : MonoBehaviour
{

    public AudioSource MusicVolume;
    public Slider SliderMusicVolume;
    public bool mute;

    public GameObject[] tabsText;
    public GameObject[] SettingsTabs;

    private Color ActiveColor = new Color(173 / 255.0f, 138 / 255.0f, 138 / 255.0f);
    private Color DefaultColor = new Color(255 / 255.0f, 255 / 255.0f, 255 / 255.0f);

    public TMP_Dropdown ScreenResolutions;
    public Toggle ActiveWindowMode;
    private Resolution[] Resolutions;
    public int resolutionX;
    public int resolutionY;
    // Start is called before the first frame update
    void Start()
    {
        mute = Camera.main.GetComponent<AudioSource>().mute;
        (tabsText[0]).GetComponent<TextMeshProUGUI>().color = ActiveColor;
        resolutionX = Screen.width;
        resolutionY = Screen.height;
        GetResolutions();
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
    public void ChangeWindowMode()
    {
        Screen.fullScreen = !ActiveWindowMode.isOn;
    }
    public void ChangeResolution()
    {
        Screen.SetResolution(Resolutions[ScreenResolutions.value].width, Resolutions[ScreenResolutions.value].height, !ActiveWindowMode.isOn);
    }
    private void GetResolutions() {
        Resolutions = Screen.resolutions.Where(resolution => resolution.refreshRate >= 58).Reverse().ToArray();
        Resolution[] res = Resolutions.Distinct().ToArray();
        Screen.SetResolution(res[0].width, res[0].height, true);
        string[] strRes = new string[res.Length];
        for (int i = 0; i < res.Length; i++) {
            //strRes[i] = res[i].width.ToString() + "x" + res[i].height.ToString();
            strRes[i] = Resolutions[i].ToString();
        }
        ScreenResolutions.ClearOptions();
        ScreenResolutions.AddOptions(strRes.ToList());
    }


}
