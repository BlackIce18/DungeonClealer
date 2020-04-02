using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour, IRotateble
{
    public int x;
    public int y;
    public GameObject doorU;
    public GameObject doorR;
    public GameObject doorD;
    public GameObject doorL;

    public GameObject roomTransferU;
    public GameObject roomTransferR;
    public GameObject roomTransferD;
    public GameObject roomTransferL;

    public int roomSizeX;
    public int roomSizeY;

    public List<GameObject> CheckDoors(Room room) {
        List<GameObject> doors = new List<GameObject>();
        if (room.doorU)
            doors.Add(room.doorU);
        if (room.doorR)
            doors.Add(room.doorR);
        if (room.doorD)
            doors.Add(room.doorD);
        if (room.doorL)
            doors.Add(room.doorL);
        return doors;
    }

    public void RotateTransfer(RoomTransfer transfer) {
        float temp;
        temp = transfer.playerChange.x;
        transfer.playerChange.x *= -1;
        transfer.playerChange.y *= -1;
        transfer.playerChange.x = transfer.playerChange.y;
        transfer.playerChange.y = temp;

        temp = transfer.x;
        transfer.x *= -1;
        transfer.y *= -1;
        transfer.x = transfer.y;
        transfer.y = (int)temp;
    }
    public void Rotate()
    {
        transform.Rotate(0, 0, 90);
        int tmpSize;
        tmpSize = roomSizeX;
        roomSizeX = roomSizeY;
        roomSizeY = tmpSize;
        GameObject tmp = doorL;
        doorL = doorU;
        doorU = doorR;
        doorR = doorD;
        doorD = tmp;

        GameObject tmp1 = roomTransferL;
        roomTransferL = roomTransferU;
        roomTransferU = roomTransferR;
        roomTransferR = roomTransferD;
        roomTransferD = tmp1;

        if (roomTransferU)
            RotateTransfer(roomTransferU.GetComponent<RoomTransfer>());
        if (roomTransferR)
            RotateTransfer(roomTransferR.GetComponent<RoomTransfer>());
        if (roomTransferD)
            RotateTransfer(roomTransferD.GetComponent<RoomTransfer>());
        if (roomTransferL)
            RotateTransfer(roomTransferL.GetComponent<RoomTransfer>());
    }
    public void RotateRandomly() {
        int count = Random.Range(0, 4);
        for (int i=0; i < count; i++) {
            Rotate();
        }
    }
}
