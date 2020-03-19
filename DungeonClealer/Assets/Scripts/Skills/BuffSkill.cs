using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BuffSkill : Skill
{
    public int duration;
    public Characteristics Buffcharacteristics;
    private GameObject kill;
    // Start is called before the first frame update
    void Start()
    {
        characteristics = GetComponent<Characteristics>();
        animation = GetComponent<Animation>();

    }
    public void UseSkill()
    {
        animation.Play();
        kill = GameObject.Find("CANVA");
        for(int i = 0; i < kill.transform.childCount; i++)
            kill.transform.GetChild(i).gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
