using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Generator : MonoBehaviour
{
    public abstract Maze GenerateMaze(int size);
    public abstract Vector2Int GenerateCheese(Maze maze, Vector3[] playerPositions, Vector2Int? currentCheese);
}