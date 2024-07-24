using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{

    [SerializeField] private GameObject BlockPrefab;
    [SerializeField] private GameObject CheesePrefab;

    [SerializeField] private GameObject MazeParent;

    private float gridOffset = 0.5f;

    private Maze maze;

    public void SpawnMaze(Maze m)
    {
        maze = m;
        for (int x = 0; x < maze.Size; x++)
        {
            for (int y = 0; y < maze.Size; y++)
            {
                if (maze.TileMap[x, y])
                {
                    SpawnObstacle(x, y, maze.Size);
                }
            }
        }
    }

    public void SpawnObstacle(int x, int y, int size)
    {
        var pos = new Vector3(x - maze.GetCoordOffset() + gridOffset, 0, y - maze.GetCoordOffset() + gridOffset);
        GameObject obstacle = Instantiate(BlockPrefab, pos, new Quaternion(), MazeParent.transform);
        NetworkObject network = obstacle.GetComponent<NetworkObject>();
        if (network != null)
        {
            network.Spawn();
        }
    }

    public GameObject SpawnCheese(Maze maze, Vector2Int coords)
    {
        var pos = new Vector3(coords.x - maze.GetCoordOffset() + gridOffset, 0, coords.y - maze.GetCoordOffset() + gridOffset);
        var cheese = Instantiate(CheesePrefab, pos, new Quaternion(), MazeParent.transform);
        cheese.GetComponent<NetworkObject>().Spawn();
        return cheese;
    }

    public void SetParent(GameObject parent)
    {
        MazeParent = parent;
    }
}
