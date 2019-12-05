using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RoomsPlacer : MonoBehaviour
{
    public Room[] RoomPrefabs;
    public Room StartingRoom;

    private Room[,] spawnedRooms;
    // Start is called before the first frame update
    private IEnumerator Start()
    {
        spawnedRooms = new Room[11,11]; // Размер подземелья максимум 5 комнат наверх/вниз и влево/вправо
        spawnedRooms[5,5] = StartingRoom;

        for (int i = 0; i < 10; i++) {
          PlaceOneRoom();
          yield return new WaitForSecondsRealtime(0.5f);
        }

        //contectToSomething

    }

    // Update is called once per frame
    private void PlaceOneRoom()
    {
        HashSet<Vector2Int> vacantPlaces = new HashSet<Vector2Int>();
        for (int x = 0; x < spawnedRooms.GetLength(0); x++) {
            for(int y = 0; y < spawnedRooms.GetLength(1); y++) {
                if(spawnedRooms[x,y] == null) continue;

                int maxX = spawnedRooms.GetLength(0) - 1;
                int maxY = spawnedRooms.GetLength(1) - 1;

                if(x > 0 && spawnedRooms[x-1,y] == null) vacantPlaces.Add(new Vector2Int(x-1,y));
                if(y > 0 && spawnedRooms[x,y-1] == null) vacantPlaces.Add(new Vector2Int(x,y-1));
                if(x < maxX && spawnedRooms[x+1,y] == null) vacantPlaces.Add(new Vector2Int(x+1,y));
                if(y < maxY && spawnedRooms[x,y+1] == null) vacantPlaces.Add(new Vector2Int(x,y+1));
            }
        }
        Room newRoom = Instantiate(RoomPrefabs[Random.Range(0, RoomPrefabs.Length)]);
        int limit = 500;
        while(limit-- > 0) {
            Vector2Int position = vacantPlaces.ElementAt(Random.Range(0,vacantPlaces.Count));
            newRoom.RotateRandomly();
            if(ConnectToSomething(newRoom,position)){
                //position.x-5,position.y-5  - -5 это от spawnedRooms[5,5]
                //*20 - размер комнаты по x - 1 стенку , аналогично и для *12
                newRoom.transform.position = new Vector3((position.x-5)*20,(position.y-5)*12,0);
                spawnedRooms[position.x,position.y] = newRoom;
                return;
            }
        }
        Destroy(newRoom.gameObject);
    }

    private bool ConnectToSomething(Room room, Vector2Int p) {
        int maxX = spawnedRooms.GetLength(0) - 1;
        int maxY = spawnedRooms.GetLength(1) - 1;

        //Список к чему подсоединяться
        List<Vector2Int> neighbours = new List<Vector2Int>();

        // ? возвращает null если объект слева равен null
        if(room.DoorU != null && p.y < maxY && spawnedRooms[p.x,p.y+1]?.DoorD != null) neighbours.Add(Vector2Int.up);
        if(room.DoorD != null && p.y > 0 && spawnedRooms[p.x,p.y-1]?.DoorU != null) neighbours.Add(Vector2Int.down);
        if(room.DoorR != null && p.x < maxX && spawnedRooms[p.x+1,p.y]?.DoorL != null) neighbours.Add(Vector2Int.right);
        if(room.DoorL != null && p.x > 0 && spawnedRooms[p.x-1,p.y]?.DoorR != null) neighbours.Add(Vector2Int.left);

        if(neighbours.Count == 0) return false;

        Vector2Int selectedDirection = neighbours[Random.Range(0,neighbours.Count)];
        Room selectedRoom = spawnedRooms[p.x + selectedDirection.x, p.y + selectedDirection.y];



        return true;
    }

}
