using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PrimsMazeGenerator : Generator
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

        // Fill the maze with walls
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                maze.Set(x, y, true);
            }
        }
        // initial cells
        Vector2Int startCell = new Vector2Int(5, 5);
        Vector2Int goalCell = new Vector2Int(95, 95);

        //
        // Random passage generation
        // 




        // A random cell is selected to be the origin of the generated maze, the initial passage cell
        maze.Set(startCell.x, startCell.y, false);


        // Create a list to store all of the frontier cells and calculte the first ones of the intial passage
        HashSet<(int, int)> frontierCells = GetNeighborCells(startCell.x, startCell.y, true, maze);

        // While there's frontier cells, continue creating new passages
        while (frontierCells.Any())
        {
            // Select a random frontier cell and change it into a passage
            int randomIndex = Random.Range(0, frontierCells.Count);
            (int, int) randomFrontierCell = frontierCells.ElementAt(randomIndex);
            int randomFrontierCellX = randomFrontierCell.Item1;
            int randomFrontierCellY = randomFrontierCell.Item2;
            maze.Set(randomFrontierCellX, randomFrontierCellY, false);

            // Obtain the list of passage cells which can be connected to the selected frontier cell
            HashSet<(int, int)> candidateCells = GetNeighborCells(randomFrontierCellX, randomFrontierCellY, false, maze);
            if (candidateCells.Any()) // safety statement
            {
                // Select a random passage to connect with the frontier cell
                int randomIndexCandidate = Random.Range(0, candidateCells.Count);
                (int, int) randomCellConnection = candidateCells.ElementAt(randomIndexCandidate);
                int randomCellConnectionX = randomCellConnection.Item1;
                int randomCellConnectionY = randomCellConnection.Item2;

                // Calculate which cell is inbetween the frontier cell and the passage
                (int, int) cellBetween;
                if (randomFrontierCellX < randomCellConnectionX)
                    cellBetween = (randomFrontierCellX + 1, randomFrontierCellY);
                else if (randomFrontierCellX > randomCellConnectionX)
                    cellBetween = (randomFrontierCellX - 1, randomFrontierCellY);
                else
                {
                    if (randomFrontierCellY < randomCellConnectionY)
                        cellBetween = (randomFrontierCellX, randomFrontierCellY + 1);
                    else
                        cellBetween = (randomFrontierCellX, randomFrontierCellY - 1);
                }

                // Make the cell in between a passage to connect the frontier cell and passage cell
                maze.Set(cellBetween.Item1, cellBetween.Item2, false);
            }

            // Remove the frontier cell that has been converted to a passage 
            frontierCells.Remove(randomFrontierCell);

            // Calculate the frontier cells from the previous frontier cell selected
            frontierCells.UnionWith(GetNeighborCells(randomFrontierCellX, randomFrontierCellY, true, maze));
        }


        //
        // End of random passage generation
        //

        // DO NOT TOUCH
        // Fill in the spawn and cheese areas
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
            }
        }

        return maze;
    }

    private HashSet<(int, int)> GetNeighborCells(int x, int y, bool checkFrontier, Maze actualMaze)
    {
        HashSet<(int, int)> newNeighborCells = new HashSet<(int, int)>();

        // Check if the given cell can have a neighbor cell in the different axis
        // A neighbor cell can't be outside the boundaries of the 2d array or belong to the outer edge.
        // It also makes a different check depending on whether we are looking for the frontier cells or the possible passage cells 
        if (x > 2)
        {
            var cellToCheck = new Vector2Int(x - 2, y);
            if (checkFrontier ? actualMaze.GetTile(cellToCheck) == true : actualMaze.GetTile(cellToCheck) == false)
            {
                newNeighborCells.Add((cellToCheck.x, cellToCheck.y));
            }
        }
        if (x < actualMaze.Size - 3)
        {
            var cellToCheck = new Vector2Int(x + 2, y);
            if (checkFrontier ? actualMaze.GetTile(cellToCheck) == true : actualMaze.GetTile(cellToCheck) == false)
            {
                newNeighborCells.Add((cellToCheck.x, cellToCheck.y));
            }
        }

        if (y > 2)
        {
            var cellToCheck = new Vector2Int(x, y - 2);

            if (checkFrontier ? actualMaze.GetTile(cellToCheck) == true : actualMaze.GetTile(cellToCheck) == false)
            {
                newNeighborCells.Add((cellToCheck.x, cellToCheck.y));
            }
        }
        if (y < actualMaze.Size - 3)
        {
            var cellToCheck = new Vector2Int(x, y + 2);
            if (checkFrontier ? actualMaze.GetTile(cellToCheck) == true : actualMaze.GetTile(cellToCheck) == false)
            {
                newNeighborCells.Add((cellToCheck.x, cellToCheck.y));
            }
        }

        return newNeighborCells;
    }
}
