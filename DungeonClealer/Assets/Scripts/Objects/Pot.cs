using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pot : MonoBehaviour
{
    public bool isSmashed;
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
        if (isSmashed == false)
        {
            anim.SetBool("smash", true);
            //this.GetComponent<BoxCollider2D>().isTrigger = true;
            audio.PlayOneShot(smahingSound);
            isSmashed = true;
            //int coinChance = Random.Range(0, 100);
            //if (coinChance <= 50) {
                // Instantiate(coin, new Vector3(this.transform.position.x, this.transform.position.y, 0), Quaternion.identity);
            //    coin.transform.SetParent(transform.parent);
            //}
        }
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
