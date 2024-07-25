using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : Generator
{
    [SerializeField] int scale = 10;

    private bool isCheeseArea(int x, int y)
    {
        return x > 90 && y > 90;
    }

    private bool isSpawnArea(int x, int y)
    {
        return x < 10 && y < 10;
    }

    private bool isEdge(int x, int y, int size)
    {
        return x == 0 || y == 0 || x == size - 1 || y == size - 1;
    }

    private bool isSolution(int x, int y, int size)
    {
        return Mathf.Abs(x - y) <= 1;
    }

    public override Vector2Int GenerateCheese(Maze maze, Vector3[] playerPositions, Vector2Int? currentCheese)
    {
        return new Vector2Int(95, 95);
    }

    public override Maze GenerateMaze(int size)
    {
        var maze = new Maze(size);

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (isEdge(x, y, size))
                {
                    maze.Set(x, y, true);
                    continue;
                }
                if (isSpawnArea(x, y) || isCheeseArea(x, y))
                {
                    maze.Set(x, y, false);
                    continue;
                }


                if (isSolution(x, y, size))
                {
                    maze.Set(x, y, false);
                    continue;
                }


                // var val = Mathf.PerlinNoise(((float)x) / size * scale, ((float)y) / size * scale);
                // maze.Set(x, y, Mathf.Abs(val - 0.5f) > 0.1f);
                maze.Set(x, y, true);
            }
        }
        return maze;
    }
}
