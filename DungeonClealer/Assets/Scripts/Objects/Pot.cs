using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    private Animator anim;
    [Header("Звук разбития:")]
    public AudioClip smahingSound;
    AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Smash()
    {
        anim.SetBool("smash", true);
        audio.PlayOneShot(smahingSound);
        //StartCoroutine(breakCo());
    }

    // Если нужно чтобы объект исчезал
    /*IEnumerator breakCo()
    {
        yield return new WaitForSeconds(.1f);
        this.gameObject.SetActive(false);
        audio.PlayOneShot(smahingSound);
    }*/
}
