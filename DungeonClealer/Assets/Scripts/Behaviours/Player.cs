using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Characteristics characteristics;
    public PlayerClasses playerClass;
    // Start is called before the first frame update
    private Slider HP;
    private Slider MP;
    private Slider Armor;
    Animator anim;
    GameObject PrefabEffect;
    float EffectDurationSec;
    GameObject EffectParent;
    void Start()
    {
        playerClass = GameObject.Find("Player Class").GetComponent<PlayerClasses>();
        characteristics = GameObject.Find("Player Characteristics").GetComponent<Characteristics>();

        HP = GameObject.Find("HP").GetComponent<Slider>();
        MP = GameObject.Find("MP").GetComponent<Slider>();
        Armor = GameObject.Find("Armor").GetComponent<Slider>();

        HP.maxValue = characteristics.hp;
        MP.maxValue = characteristics.mp;
        Armor.maxValue = characteristics.armor;

        HP.value = characteristics.hp;
        MP.value = characteristics.mp;
        Armor.value = characteristics.armor;

        anim = GetComponent<Animator>();
    }
    public void UseSkill(Skill skill) {
        MP.value = MP.value - skill.cost;
    }
    public void UseSkillAnimation(string skillName) {
        anim.Play(skillName, -1, 0f);
    }
    public void AddEffect(GameObject prefabeffect) {
        if (prefabeffect != null)
        {
            PrefabEffect = prefabeffect;
        }
    }
    public void EffectTimer(float sec) {
        if (sec > 0 && sec <= 3600)
        {
            EffectDurationSec = sec;
        }
    }
    public void EffectParentObj(GameObject parent) {
        if (parent != null)
        {
            EffectParent = parent;
        }
        if (PrefabEffect != null && EffectDurationSec != 0 && EffectParent != null)
        {
            StartCoroutine(CreateEffect(PrefabEffect, EffectDurationSec, EffectParent));
        }
    }
    IEnumerator CreateEffect(GameObject prefabeffect, float seconds, GameObject parent) {
        GameObject effect = Instantiate(prefabeffect);
         effect.transform.position = new Vector3(parent.transform.position.x, parent.transform.position.y, -1);
        //effect.transform
        effect.transform.SetParent(parent.transform);
        yield return new WaitForSeconds(seconds);
        Destroy(effect);
    }
}
