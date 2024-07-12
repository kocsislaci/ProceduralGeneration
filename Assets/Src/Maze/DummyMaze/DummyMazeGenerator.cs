using UnityEngine;

public class DummyMazeGenerator : Generator
{

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

                if ((x) % 3 == 0 || (y) % 3 == 0)
                {
                    if ((x) % 3 == 0 && (y % 3) == 1)
                    {
                        maze.Set(x, y, false);
                    }
                    else if ((x % 3) == 2 && (y) % 3 == 0)
                    {
                        maze.Set(x, y, false);
                    }
                    else
                    {
                        maze.Set(x, y, true);
                    }
                }
            }
        }
        return maze;
    }
}
