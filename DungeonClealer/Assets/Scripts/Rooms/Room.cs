using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject DoorU;
    public GameObject DoorR;
    public GameObject DoorD;
    public GameObject DoorL;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RotateRandomly() {
        int count = Random.Range(0, 4);
        for (int i=0; i < count; i++) {
            transform.Rotate(0,0,90);

            GameObject tmp = DoorL;
            DoorL = DoorD;
            DoorD = DoorR;
            DoorR = DoorU;
            DoorU = tmp;
        }
    }
}
