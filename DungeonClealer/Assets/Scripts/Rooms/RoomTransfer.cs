using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransfer : MonoBehaviour
{

    public int x;
    public int y;
    public Vector3 playerChange;
    private CameraMovement cam;
    private CurrentRoom curRoom;

    private RoomsPlacer roomPlacer;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.GetComponent<CameraMovement>();
        curRoom = GameObject.Find("Player").GetComponent<CurrentRoom>();
        roomPlacer = GameObject.Find("RoomsPlacer").GetComponent<RoomsPlacer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("player")) {
            if (roomPlacer.spawnedRooms[curRoom.nmb1+x, curRoom.nmb2+y])
            {
                for (int i = 0; i < roomPlacer.spawnedRooms[curRoom.nmb1 + x, curRoom.nmb2 + y].transform.childCount; i++) {
                    roomPlacer.spawnedRooms[curRoom.nmb1 + x, curRoom.nmb2 + y].transform.GetChild(i).gameObject.SetActive(true);
                }

                curRoom.nmb1 += x;
                curRoom.nmb2 += y;
                curRoom.currentRoom = roomPlacer.spawnedRooms[curRoom.nmb1, curRoom.nmb2];
                collision.transform.position += playerChange;
            


                curRoom.currentRoom.transform.position = new Vector3(curRoom.currentRoom.transform.position.x, curRoom.currentRoom.transform.position.y, 0);

                cam.target = curRoom.currentRoom.transform;
            }
        }
    }
}
