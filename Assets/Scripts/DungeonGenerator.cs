using UnityEngine;
using System.Collections.Generic;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Generation customization: ")]
    //[SerializeField] private int minRooms = 100;
    [SerializeField] private int maxRooms = 1000; 
    [SerializeField] private int farthestRoomCount = 5; // Objective rooms (Better not to go over 8, or i will take too much generations to find correct dungeon)
    [SerializeField] private int minObjectiveRoomCount = 4; 
    [SerializeField] private int minDistanceBetweenObjectives = 10;

    private int minUniqueObjectiveCount = 3;

    private int generationCount = 0; // Debug variable (DELETE LATER)

    [Header("Prefabs: ")]
    public GameObject startRoomPrefab;
    public List<GameObject> corridorPrefabs;
    public List<GameObject> crossroadPrefabs;
    public List<GameObject> lShapePrefabs;
    public List<GameObject> tShapePrefabs;
    public List<GameObject> deadEndPrefabs;
    public GameObject[] farthestRoomPrefabs; // [Empty, First, Second, Third, Dummy]

    private int spawnedObjectives = 0;
    private int spawnedDummy = 0;

    private HashSet<Vector2Int> dungeonRooms = new HashSet<Vector2Int>();
    private List<Vector2Int> roomPositions = new List<Vector2Int>();

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        generationCount++; // For debug (DELETE LATER)

        dungeonRooms.Clear();
        roomPositions.Clear();

        Vector2Int startPos = Vector2Int.zero;
        dungeonRooms.Add(startPos);
        roomPositions.Add(startPos);

        Vector2Int currentPos = startPos;

        // TODO MIN SIZE GENERATION

        for (int i = 0; i < maxRooms - 1; i++)
        {
            currentPos = GetNewRoomPosition(currentPos);
            dungeonRooms.Add(currentPos);
            roomPositions.Add(currentPos);
        }

        List<Vector2Int> farthestRooms = GetFarthestRooms(startPos, farthestRoomCount, minDistanceBetweenObjectives);

        BlockExtraPathsAroundRoom(startPos);

        foreach (Vector2Int farthestRoom in farthestRooms)
        {
            BlockExtraPathsAroundRoom(farthestRoom);
        }

        if (farthestRooms.Count < minObjectiveRoomCount)
        {
            Debug.Log("Generation attempt: " + generationCount + ". Regenerating the dungeon...");
            GenerateDungeon();
            return;
        }

        // Spawn Rooms
        foreach (Vector2Int roomPos in dungeonRooms)
        {
            GameObject roomToSpawn = GetRoomPrefab(roomPos, startPos, farthestRooms);
            Quaternion rotation = GetRotationForRoom(roomPos);
            Instantiate(roomToSpawn, new Vector3(roomPos.x * 10, 0, roomPos.y * 10), rotation);
        }
    }

    // NOT WORKING AS INTENDED
    //void BlockOneSideOfStart(Vector2Int startPos)
    //{
    //    Vector2Int blockDir = Vector2Int.right;

    //    Vector2Int adjacentPos = startPos + blockDir;
    //    if (!dungeonRooms.Contains(adjacentPos)) // Check if the position is free
    //    {
    //        dungeonRooms.Add(adjacentPos);
    //        roomPositions.Add(adjacentPos);
    //    }
    //}
    // OBSOLETE

    void BlockExtraPathsAroundRoom(Vector2Int roomPos)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        int connectedPaths = 0;

        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPos = roomPos + dir;
            if (dungeonRooms.Contains(adjacentPos))
            {
                connectedPaths++;
            }
        }

        foreach (Vector2Int dir in directions)
        {
            Vector2Int adjacentPos = roomPos + dir;

            if (!dungeonRooms.Contains(adjacentPos) || connectedPaths > 2) // Block extra paths
            {
                dungeonRooms.Add(adjacentPos);
                roomPositions.Add(adjacentPos);
            }
        }
    }

    Vector2Int GetNewRoomPosition(Vector2Int currentPos)
    {
        List<Vector2Int> directions = new List<Vector2Int>
        {
            Vector2Int.up,
            Vector2Int.down,
            Vector2Int.left,
            Vector2Int.right
        };

        ShuffleList(directions);

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = currentPos + dir;
            if (!dungeonRooms.Contains(newPos))
                return newPos;
        }

        return currentPos;
    }

    List<Vector2Int> GetFarthestRooms(Vector2Int startPos, int count, int minDistance)
    {
        List<Vector2Int> sortedRooms = new List<Vector2Int>(roomPositions);
        sortedRooms.Sort((a, b) => Vector2Int.Distance(startPos, b).CompareTo(Vector2Int.Distance(startPos, a)));

        List<Vector2Int> selectedRooms = new List<Vector2Int>();
        foreach (Vector2Int room in sortedRooms)
        {
            if (selectedRooms.Count >= count)
                break;

            bool isFarEnough = true;
            foreach (Vector2Int placedRoom in selectedRooms)
            {
                if (Vector2Int.Distance(room, placedRoom) < minDistance)
                {
                    isFarEnough = false;
                    break;
                }
            }
            if (isFarEnough)
                selectedRooms.Add(room);
        }

        return selectedRooms;
    }

    // NOT USED
    //List<Vector2Int> GetDeadEndRooms()
    //{
    //    List<Vector2Int> deadEnds = new List<Vector2Int>();

    //    foreach (Vector2Int room in roomPositions)
    //    {
    //        int connections = 0;
    //        foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
    //        {
    //            if (dungeonRooms.Contains(room + dir))
    //                connections++;
    //        }

    //        if (connections == 1)
    //            deadEnds.Add(room);
    //    }

    //    //Debug.Log("Dead Ends Found: " + deadEnds.Count);
    //    return deadEnds;
    //}
    //OBSOLETE

    GameObject GetRoomPrefab(Vector2Int roomPos, Vector2Int startPos, List<Vector2Int> farthestRooms)
    {
        if (roomPos == startPos) return startRoomPrefab;
        // TODO IMPROVE TO RANDOM SELECTION 
        if (farthestRooms.Contains(roomPos))
        {
            if((farthestRoomCount - spawnedDummy -1) > minUniqueObjectiveCount)
            {
                spawnedDummy++;
                return farthestRoomPrefabs[0]; // Dummy objective
            }
            else
            {
                spawnedObjectives++;
                return farthestRoomPrefabs[spawnedObjectives]; //Unique objectives
            }
        }

        int connections = GetRoomConnections(roomPos).Count;
        List<GameObject> prefabList = deadEndPrefabs;

        if (connections == 2)
        {
            List<Vector2Int> conn = GetRoomConnections(roomPos);
            if (conn.Contains(Vector2Int.left) && conn.Contains(Vector2Int.right))
                prefabList = corridorPrefabs; // Corridor (Horizontal)
            else if (conn.Contains(Vector2Int.up) && conn.Contains(Vector2Int.down))
                prefabList=corridorPrefabs; // Corridor (Vertical)
            else prefabList = lShapePrefabs; // L-Shaped Room
        }
        else if (connections == 3)
        {
           prefabList = tShapePrefabs; // T-Shaped Room
        }
        else if (connections == 4)
        {
           prefabList = crossroadPrefabs; // Crossroad
        }

        return prefabList[Random.Range(0, prefabList.Count)];
    }

    Quaternion GetRotationForRoom(Vector2Int roomPos)
    {
        List<Vector2Int> connections = GetRoomConnections(roomPos);

        if (connections.Count == 1) // Dead End or Objective Room
            return GetRotationForSingleEntryRoom(roomPos);

        if (connections.Count == 2)
        {
            if (connections.Contains(Vector2Int.left) && connections.Contains(Vector2Int.right))
                return Quaternion.Euler(0, 0, 0);
            if (connections.Contains(Vector2Int.up) && connections.Contains(Vector2Int.down))
                return Quaternion.Euler(0, 90, 0);

            if (connections.Contains(Vector2Int.up) && connections.Contains(Vector2Int.right))
                return Quaternion.Euler(0, 180, 0);
            if (connections.Contains(Vector2Int.right) && connections.Contains(Vector2Int.down))
                return Quaternion.Euler(0, -90, 0);
            if (connections.Contains(Vector2Int.down) && connections.Contains(Vector2Int.left))
                return Quaternion.Euler(0, 0, 0);
            if (connections.Contains(Vector2Int.left) && connections.Contains(Vector2Int.up))
                return Quaternion.Euler(0, 90, 0);
        }
        else if (connections.Count == 3) // T-Shaped Rooms
        {
            if (connections.Contains(Vector2Int.up) && connections.Contains(Vector2Int.left) && connections.Contains(Vector2Int.right))
                return Quaternion.Euler(0, -270, 0);
            if (connections.Contains(Vector2Int.right) && connections.Contains(Vector2Int.up) && connections.Contains(Vector2Int.down))
                return Quaternion.Euler(0, -180, 0);
            if (connections.Contains(Vector2Int.down) && connections.Contains(Vector2Int.left) && connections.Contains(Vector2Int.right))
                return Quaternion.Euler(0, -90, 0);
            if (connections.Contains(Vector2Int.left) && connections.Contains(Vector2Int.up) && connections.Contains(Vector2Int.down))
                return Quaternion.Euler(0, 0, 0);
        }

        return Quaternion.identity;
    }

    Quaternion GetRotationForSingleEntryRoom(Vector2Int roomPos)
    {
        foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
        {
            if (dungeonRooms.Contains(roomPos + dir))
            {
                if (dir == Vector2Int.up) return Quaternion.Euler(0, 270, 0);
                if (dir == Vector2Int.down) return Quaternion.Euler(0, 90, 0);
                if (dir == Vector2Int.left) return Quaternion.Euler(0, 180, 0);
                if (dir == Vector2Int.right) return Quaternion.Euler(0, 0, 0);
            }
        }

        return Quaternion.identity;
    }

    List<Vector2Int> GetRoomConnections(Vector2Int roomPos)
    {
        List<Vector2Int> connections = new List<Vector2Int>();
        foreach (Vector2Int dir in new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right })
        {
            if (dungeonRooms.Contains(roomPos + dir))
                connections.Add(dir);
        }

        return connections;
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int rnd = Random.Range(0, i + 1);
            (list[i], list[rnd]) = (list[rnd], list[i]);
        }
    }
}