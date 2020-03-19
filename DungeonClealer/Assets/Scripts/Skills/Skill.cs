using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Skill : MonoBehaviour
{
    public Characteristics characteristics;

    public string skillName;
    public string description;
    public int level;
    public int cost;
    public int coldown;
    public int durationOfUse;
    public bool learned;

    public enum SkillType { singleTarget = 1, AOE = 2, nonTarget = 3 };
    public SkillType skilltype;

    protected Animation animation;
    

    // Start is called before the first frame update
    void Start()
    {
        characteristics = GameObject.Find("Player Characteristics").GetComponent<Characteristics>();
        
    }

    /*public void UseSkill() {
        animation.Play("New Animation");
    }*/
}
