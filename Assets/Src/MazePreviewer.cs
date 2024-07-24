using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazePreviewer : MonoBehaviour
{
    GameObject preview;
    [SerializeField] private Generator generator;
    [SerializeField] public MazeSpawner spawner;
    [SerializeField] public int Size = 100;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this);
    }

    public void GeneratePreview()
    {
        DestroyPreivew();

        preview = new GameObject("Mazeeee");
        preview.transform.parent = transform;

        Maze maze = generator.GenerateMaze(Size);
        spawner.SetParent(preview);
        spawner.SpawnMaze(maze);
    }

    public void DestroyPreivew()
    {
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject);
            }
        }
    }
}
