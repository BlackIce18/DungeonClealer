using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AttackSkill : Skill
{
    public int range;
    public int skillDamage;
    public int magicDamage;

    private GameObject skill;
    // Start is called before the first frame update
    void Start()
    {
        animation = GetComponent<Animation>();
    }
    public void UseSkill()
    {
        animation.Play();
        skill = GameObject.Find("Hail blows skill");
        for (int i = 0; i < skill.transform.childCount; i++)
            skill.transform.GetChild(i).gameObject.SetActive(true);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
