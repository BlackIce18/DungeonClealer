using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillBuff : Skill
{
    public int duration;
    public Characteristics Buffcharacteristics;
    // Start is called before the first frame update
    void Start()
    {
        characteristics = GetComponent<Characteristics>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
