using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicList;
    public AudioSource audioSource;
    private int currentNmbMusic;
    private bool played;
    void Mix()
    {
        for (int i = 0; i < musicList.Length; i++)
        {
            Random.seed = (int)System.DateTime.Now.Ticks & 0x0000FFFF;
            int temp = Random.Range(0, musicList.Length);
            AudioClip music = musicList[i];
            musicList[i] = musicList[temp];
            musicList[temp] = music;
        }
        audioSource.clip = musicList[0];
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(musicList[currentNmbMusic].length * Time.timeScale); //дождаться окончания
        played = false; //и щелкнуть переключатель назад
    }
    void NextMusic()
    {
        currentNmbMusic++;
        if (currentNmbMusic >= musicList.Length) { currentNmbMusic = 0;}
        audioSource.clip = musicList[currentNmbMusic];
    }

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentNmbMusic = 0;
        Mix();
        audioSource.Play();
        played = true;
        StartCoroutine(Wait());
        audioSource.ignoreListenerPause = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!played)
        {
            NextMusic();
            played = true;
            audioSource.Play();
            StartCoroutine(Wait());
        }
    }



}
