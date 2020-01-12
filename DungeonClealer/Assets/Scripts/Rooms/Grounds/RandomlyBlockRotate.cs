using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomlyBlockRotate : MonoBehaviour, IRotateble
{
    // Start is called before the first frame update
    void Start()
    {
        RotateRandomly();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Rotate() {
        transform.Rotate(new Vector3(0, 0, 90) * 1);
    }
    public void RotateRandomly()
    {
        int count = Random.Range(0, 4);
        for (int i = 0; i < count; i++)
        {
            Rotate();
        }
    }
}
