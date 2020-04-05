using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class RoomsPlacer : MonoBehaviour
{
    [System.Serializable]
    public class OneDoorRooms
    {
        public Room[] UpRooms;
        public Room[] RightRooms;
        public Room[] DownRooms;
        public Room[] LeftRooms;
    }

    [System.Serializable]
    public class Rooms {
        public OneDoorRooms onedoorrooms;
        public Room[] UpRooms;
        public Room[] RightRooms;
        public Room[] DownRooms;
        public Room[] LeftRooms;
    }

    [Header("Размер подземелья")]
    public int DungeonSize;
    [Header("Стартовая комната")]
    public Room StartingRoom; // Стартовая комната

    [Header("Типы комнат")]
    public Rooms RoomPrefabs;

    [Header("Сокровищница")]
    public OneDoorRooms Treasuries;
    private bool isSpawnedTeasuries;

    [Header("Комната с боссом")]
    public OneDoorRooms bossRooms;
    private bool isSpawnedBoosRoom;

    public Room[,] spawnedRooms; // сетка комнат

    private int centerRoom;
    private CurrentRoom currentRoom;
    HashSet<Vector2Int> vacantPlaces;
    int maxX;
    int maxY;
    void HideRooms() {
        for (int x = 1; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 1; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;
                else {
                    for (int i = 0; i < spawnedRooms[x,y].transform.childCount; i++)
                    {
                        if(spawnedRooms[x,y]!= spawnedRooms[centerRoom, centerRoom])
                        spawnedRooms[x, y].transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    void spawnRoom(Room room, Vector2Int vacantplace) {
        room.transform.SetParent(GameObject.Find("Rooms").transform);
        room.transform.position = new Vector3((vacantplace.x - centerRoom), (centerRoom - vacantplace.y), 0) * 14;
        spawnedRooms[vacantplace.x, vacantplace.y] = room;
    }

    private void Start()
    {
        isSpawnedTeasuries = false;
        isSpawnedBoosRoom = false;
        if (DungeonSize < 0)
            throw new Exception();

        spawnedRooms = new Room[DungeonSize + 1, DungeonSize + 1];
        centerRoom = Mathf.FloorToInt(DungeonSize / 2);
        spawnedRooms[centerRoom, centerRoom] = StartingRoom;

        currentRoom = GameObject.Find("Player").GetComponent<CurrentRoom>();

        currentRoom.nmb1 = centerRoom;
        currentRoom.nmb2 = centerRoom;
        int randomRoomNumber = UnityEngine.Random.Range(5, DungeonSize);

        maxX = spawnedRooms.GetLength(0) - 2;
        maxY = spawnedRooms.GetLength(1) - 2;
        PlaceOneRoom();
        //Debug.Log("Спавнить randomRoomNumber комнат: " + randomRoomNumber.ToString());
        for (int i = 0; i < randomRoomNumber; i++)
        {
            
            if ((vacantPlaces.Count == 2 && i == randomRoomNumber - 2) || (i == randomRoomNumber - 1 && vacantPlaces.Count >= 2) && (isSpawnedTeasuries == false) && (isSpawnedBoosRoom == false))
            {
                Debug.Log("Добавляем сокровищницу");
                isSpawnedTeasuries = SpawnOneDoorRoom(Treasuries);
                Debug.Log("Босс комната");
                isSpawnedBoosRoom = SpawnOneDoorRoom(bossRooms);
                break;
            }
            PlaceOneRoom();
            //yield return new WaitForSecondsRealtime(0.5f);
        }
        int vacantplacescount = vacantPlaces.Count;
        while (vacantplacescount-- > 0)
        {
            SpawnOneDoorRoom(RoomPrefabs.onedoorrooms);
            //yield return new WaitForSecondsRealtime(0.5f);
        }
        HideRooms();
    }
    Room GenerateRandomRoom()
    {
        int randomDirection = UnityEngine.Random.Range(0, 4);
        int randomVariation;
        switch (randomDirection)
        {
            case 0:
                randomVariation = UnityEngine.Random.Range(0, RoomPrefabs.UpRooms.Length);
                return RoomPrefabs.UpRooms[randomVariation];
            case 1:
                randomVariation = UnityEngine.Random.Range(0, RoomPrefabs.RightRooms.Length);
                return RoomPrefabs.RightRooms[randomVariation];
            case 2:
                randomVariation = UnityEngine.Random.Range(0, RoomPrefabs.DownRooms.Length);
                return RoomPrefabs.DownRooms[randomVariation];
            case 3:
                randomVariation = UnityEngine.Random.Range(0, RoomPrefabs.LeftRooms.Length);
                return RoomPrefabs.LeftRooms[randomVariation];
            default:
                return null;
        }
    }
    private void getVacantPlaces() {
        vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 1; x < spawnedRooms.GetLength(0); x++)
        {
            for (int y = 1; y < spawnedRooms.GetLength(1); y++)
            {
                if (spawnedRooms[x, y] == null) continue;


                if (x > 1 && spawnedRooms[x - 1, y] == null && spawnedRooms[x, y].doorL != null) // Проверяем лево
                    vacantPlaces.Add(new Vector2Int(x - 1, y));

                if (y > 1 && spawnedRooms[x, y - 1] == null && spawnedRooms[x, y].doorU != null) // Проверяем верх
                    vacantPlaces.Add(new Vector2Int(x, y - 1));

                if (x < maxX && spawnedRooms[x + 1, y] == null && spawnedRooms[x, y].doorR != null) // Проверяем право
                    vacantPlaces.Add(new Vector2Int(x + 1, y));

                if (y < maxY && spawnedRooms[x, y + 1] == null && spawnedRooms[x, y].doorD != null) // Проверяем низ
                    vacantPlaces.Add(new Vector2Int(x, y + 1));
            }
        }
    }
    private void PlaceOneRoom()
    {
        getVacantPlaces();
        string str = "";
        for (int i = 0; i < vacantPlaces.Count; i++){
            str += vacantPlaces.ToList()[i] + " ";
        }
        Debug.Log(str);
        // Эту строчку можно заменить на выбор комнаты с учётом её вероятности, вроде как в ChunksPlacer.GetRandomChunk()
        Room newRoom = Instantiate(GenerateRandomRoom());
        int limit = 100;
        while (limit-- > 0)
        {
            // Эту строчку можно заменить на выбор положения комнаты с учётом того насколько он далеко/близко от центра,
            // или сколько у него соседей, чтобы генерировать более плотные, или наоборот, растянутые данжи
            Vector2Int position = vacantPlaces.ElementAt(UnityEngine.Random.Range(0, vacantPlaces.Count));
            //newRoom.RotateRandomly();

            if (ConnectToSomething(newRoom, position))
            {
                //Debug.Log("Я " + newRoom?.doorU +" "+ newRoom?.doorR +" " +newRoom?.doorD +" "+ newRoom?.doorL + " хочу присоединиться к " + position.x+ " "+ position.y);
                /*newRoom.transform.position = new Vector3(( position.x- centerRoom), (centerRoom-position.y), 0) * 14;
                newRoom.transform.parent = GameObject.Find("Rooms").transform;
                spawnedRooms[position.x, position.y] = newRoom;*/
                spawnRoom(newRoom, position);
                return;
            }
        }

        Destroy(newRoom.gameObject);
    }
    private bool SpawnOneDoorRoom(OneDoorRooms oneDoorRoom) {
        getVacantPlaces();
        int limit = 100;
        Room room = new Room();
        while (limit-- > 0)
        {
            Vector2Int RandomVacantPlace = vacantPlaces.ToList()[UnityEngine.Random.Range(0, vacantPlaces.Count)];

            //for (int i = 0; i < vacantPlaces.Count; i++) {
                //Vector2Int RandomVacantPlace = vacantPlaces.ToList()[i];
                if (RandomVacantPlace.x > 1 && spawnedRooms[RandomVacantPlace.x - 1, RandomVacantPlace.y]?.doorR != null)
                {
                    int randomVariation = UnityEngine.Random.Range(0, oneDoorRoom.LeftRooms.Length);
                    room = Instantiate(oneDoorRoom.LeftRooms[randomVariation]);
                }

                else if (RandomVacantPlace.y > 1 && spawnedRooms[RandomVacantPlace.x, RandomVacantPlace.y - 1]?.doorD != null)
                {
                    int randomVariation = UnityEngine.Random.Range(0, oneDoorRoom.UpRooms.Length);
                    room = Instantiate(oneDoorRoom.UpRooms[randomVariation]);
                }

                else if (RandomVacantPlace.x < maxX && spawnedRooms[RandomVacantPlace.x + 1, RandomVacantPlace.y]?.doorL != null)
                {
                    int randomVariation = UnityEngine.Random.Range(0, oneDoorRoom.RightRooms.Length);
                    room = Instantiate(oneDoorRoom.RightRooms[randomVariation]);
                }

                else if (RandomVacantPlace.y < maxY && spawnedRooms[RandomVacantPlace.x, RandomVacantPlace.y + 1]?.doorU != null)
                {
                    int randomVariation = UnityEngine.Random.Range(0, oneDoorRoom.DownRooms.Length);
                    room = Instantiate(oneDoorRoom.DownRooms[randomVariation]);
                }
                if (ConnectToSomething(room, RandomVacantPlace))
                {
                    //Debug.Log("Я " + newRoom?.doorU +" "+ newRoom?.doorR +" " +newRoom?.doorD +" "+ newRoom?.doorL + " хочу присоединиться к " + position.x+ " "+ position.y);
                    /*newRoom.transform.position = new Vector3(( position.x- centerRoom), (centerRoom-position.y), 0) * 14;
                    newRoom.transform.parent = GameObject.Find("Rooms").transform;
                    spawnedRooms[position.x, position.y] = newRoom;*/
                    spawnRoom(room, RandomVacantPlace);
                    vacantPlaces.Remove(RandomVacantPlace);
                    return true;
                }
                //spawnRoom(room, RandomVacantPlace);
                //break;
            //}
        }
        return false;
    }
    private bool ConnectToSomething(Room room, Vector2Int p)
    {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        List<Vector2Int> neighbours = new List<Vector2Int>();

        if (room.doorU != null && p.y < maxY && spawnedRooms[p.x, p.y - 1]?.doorD != null) neighbours.Add(Vector2Int.up);
        if (room.doorD != null && p.y > 0 && spawnedRooms[p.x, p.y + 1]?.doorU != null) neighbours.Add(Vector2Int.down);
        if (room.doorR != null && p.x < maxX && spawnedRooms[p.x + 1, p.y]?.doorL != null) neighbours.Add(Vector2Int.right);
        if (room.doorL != null && p.x > 0 && spawnedRooms[p.x - 1, p.y]?.doorR != null) neighbours.Add(Vector2Int.left);

        if (neighbours.Count == 0) return false;

       /* Vector2Int selectedDirection = neighbours[Random.Range(0, neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];

        if (selectedDirection == Vector2Int.up)
        {
            room.doorU.SetActive(false);
            selectedRoom.doorD.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.down)
        {
            room.doorD.SetActive(false);
            selectedRoom.doorU.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.right)
        {
            room.doorR.SetActive(false);
            selectedRoom.doorL.SetActive(false);
        }
        else if (selectedDirection == Vector2Int.left)
        {
            room.doorL.SetActive(false);
            selectedRoom.doorR.SetActive(false);
        }*/

        return true;
    }
}
