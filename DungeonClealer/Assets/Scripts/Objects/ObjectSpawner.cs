using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    int RandNmbObj;   // Рандомный объект для спавна
    int RandCountObj; // Сколько спавнить данного объекта
    // Start is called before the first frame update
    void Start()
    {
        int CountSpawnPoints = transform.childCount;

        RandCountObj = Random.Range(0, CountSpawnPoints / 8);

        for (int i = 0; i < RandCountObj; i++)
        {
            GameObject SpawnPoint = transform.GetChild(Random.Range(0, CountSpawnPoints)).gameObject;
            if (!SpawnPoint.activeSelf) { i--; continue; }
            if ((SpawnPoint.transform.position.x >= -1.5 && SpawnPoint.transform.position.x <= 1.5) && (SpawnPoint.transform.position.y >= -1.5 && SpawnPoint.transform.position.y <= 1.5)) { 
                i--; 
                continue;  
            }
            ObjectSpawnPoint objspawnpoint = SpawnPoint.GetComponent<ObjectSpawnPoint>();
            RandNmbObj = Random.Range(0, objspawnpoint.Objects.Length);
            GameObject spawnObject = Instantiate(objspawnpoint.Objects[RandNmbObj]);
            spawnObject.transform.SetParent(transform.parent.Find("Objects"));

            spawnObject.transform.position = new Vector3(SpawnPoint.transform.position.x, SpawnPoint.transform.position.y, 0);
            SpawnPoint.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
