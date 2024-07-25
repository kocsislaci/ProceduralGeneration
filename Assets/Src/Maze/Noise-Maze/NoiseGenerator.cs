using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseGenerator : Generator
{
    [SerializeField] int scale = 10;
    [SerializeField] int seed = 0;

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

    public override Vector2Int GenerateCheese(Maze maze, Vector3[] playerPositions, Vector2Int? currentCheese)
    {
        return new Vector2Int(95, 95);
    }

    private Vector2Int pathByParam(int i, int size) {
         float noise = 0.5f - Mathf.PerlinNoise((float)i / size * scale, 1 + seed);
            int ampl = 50 - Mathf.Abs(50 - i);
            int offset = (int)Mathf.Round(noise * ampl);
            int x = i - offset;
            int y = i + offset;
            return new Vector2Int(x, y);
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

                var val = Mathf.PerlinNoise(((float)x) / size * scale, ((float)y) / size * scale + seed);
                maze.Set(x, y, Mathf.Abs(val - 0.5f) > 0.1f);
            }
        }

        for (int i = 0; i < 100; i++)
        {
            var now = pathByParam(i, size);
            var next = pathByParam(i + 1, size);

            var dist = Vector2Int.Distance(now, next);
            var dir = (next - (Vector2)now).normalized;
            for(int p = 0; p < dist; p++) {
                var px = (int)Mathf.Round(p * dir.x);
                var py = (int)Mathf.Round(p * dir.y);

                maze.Set(now.x + px, now.y + py, false);
                maze.Set(now.x + px, now.y + py +1, false);
                // maze.Set(now.x + px + 1, now.y + py +1, false);
                maze.Set(now.x + px + 1, now.y + py, false);

            }
        }

        return maze;
    }
}
