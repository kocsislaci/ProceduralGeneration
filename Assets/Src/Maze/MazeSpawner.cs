using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeSpawner : MonoBehaviour
{

    [SerializeField] private GameObject BlockPrefab;
    [SerializeField] private GameObject CheesePrefab;

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
        Instantiate(BlockPrefab, pos, new Quaternion());
    }

    public GameObject SpawnCheese(Maze maze, Vector2Int coords)
    {
        var pos = new Vector3(coords.x - maze.GetCoordOffset() + gridOffset, 0, coords.y - maze.GetCoordOffset() + gridOffset);
        return Instantiate(CheesePrefab, pos, new Quaternion());
    }
}
