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
        spawnedRooms = new Room[21,13];
        spawnedRooms[6,6] = StartingRoom;

        for (int i = 0; i < 12; i++) {
          PlaceOneRoom();
          yield return new WaitForSecondsRealtime(0.5f);
        }
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
        Vector2Int position = vacantPlaces.ElementAt(Random.Range(0,vacantPlaces.Count));
        newRoom.transform.position = new Vector3((position.x-5)*19,(position.y-5)*12,0);

        spawnedRooms[position.x,position.y] = newRoom;
    }
}
