using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Characteristics characteristics;
    public PlayerClasses playerClass;
    // Start is called before the first frame update
    void Start()
    {
        playerClass = GameObject.Find("Player Class").GetComponent<PlayerClasses>();
        characteristics = GameObject.Find("Player Characteristics").GetComponent<Characteristics>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
