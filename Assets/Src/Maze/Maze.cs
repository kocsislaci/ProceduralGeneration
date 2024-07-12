using UnityEngine;

public class Maze
{
    public bool[,] TileMap { get; set; }
    public int Size { get; set; }

    public Maze(int size)
    {
        Size = size;
        TileMap = new bool[size, size];
    }

    public void Set(Vector2Int pos, bool value)
    {
        TileMap[pos.x, pos.y] = value;
    }

    public void Set(int x, int y, bool value)
    {
        TileMap[x, y] = value;
    }

    public bool GetTile(Vector2Int pos)
    {
        return TileMap[pos.x, pos.y];
    }

    public int GetCoordOffset()
    {
        return Size / 2;
    }


    static public Vector2Int WorldPosToCoords(Vector3 vec)
    {
        return new Vector2Int((int)vec.x, (int)vec.z);
    }
}