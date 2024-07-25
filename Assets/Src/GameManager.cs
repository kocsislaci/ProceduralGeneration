using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private Generator generator;
    [SerializeField] private MazeSpawner spawner;
    [SerializeField] private int Size = 100;

    Maze maze;

    GameObject cheese;

    void Start()
    {
        NetworkManager.Singleton.OnServerStarted += OnServerStart;
    }

    public void OnServerStart()
    {
        maze = generator.GenerateMaze(Size);
        spawner.SpawnMaze(maze);

        OnCheese();

        cheese.GetComponent<CheeseController>().OnCheeseCollected.AddListener(SetWinnerTextsClientRpc);
    }

    [ClientRpc]
    public void SetWinnerTextsClientRpc(string winner)
    {
        UIManager.Instance.SetWinnerText(winner);
    }

    public void OnCheese()
    {
        Vector2Int? currentCheesePos = cheese ? Maze.WorldPosToCoords(cheese.transform.position) : null;
        var cheesePos = generator.GenerateCheese(maze, getPlayerPosiitons(), currentCheesePos);
        cheese = spawner.SpawnCheese(maze, cheesePos);
    }

    public Vector3[] getPlayerPosiitons()
    {
        return new Vector3[] { };
    }
}

interface Map
{

    public GameObject BlockPrefab { get; set; }
    public GameObject CheesePrefab { get; set; }
    bool[,] Maze { get; set; }
    int Size { get; set; }

    public bool[,] GenerateMap(int size);
    public void SpawnMaze(bool[,] generatedMap, int size);
    public (int x, int y) GenerateCheese(bool[,] map);
    public void SpawnCheese(int x, int y, int size);

}

class DefaultMap : MonoBehaviour, Map
{

    public GameObject BlockPrefab { get; set; }
    public GameObject CheesePrefab { get; set; }

    public int Size { get => 100; set { } }
    public bool[,] Maze { get; set; }

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

    public bool[,] GenerateMap(int size)
    {

        var Map = new bool[size, size];

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // if (isEdge(x, y, size)) {
                //     Map[x,y] = true;
                //     continue;
                // }
                if (isSpawnArea(x, y) || isCheeseArea(x, y))
                {
                    Map[x, y] = false;
                    continue;
                }

                if ((x + 1) % 5 == 0 && (y + x) % 3 == 0)
                {
                    Map[x, y] = true;
                }
            }
        }

        return Map;
    }

    public (int x, int y) GenerateCheese(bool[,] map)
    {
        return (95, 95);
    }

    public void SpawnMaze(bool[,] generatedMap, int size)
    {
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                if (generatedMap[x, y])
                {
                    SpawnObstacle(x, y, size);
                }
            }
        }
    }

    public void SpawnObstacle(int x, int y, int size)
    {
        var mapOffset = size / 2;
        var gridOffset = 0.5f;
        var pos = new Vector3(x - mapOffset + gridOffset, 0, y - mapOffset + gridOffset);
        var obstacle = Instantiate(BlockPrefab, pos, new Quaternion());
    }

    public void SpawnCheese(int x, int y, int size)
    {
        var mapOffset = size / 2;
        var gridOffset = 0.5f;
        var pos = new Vector3(x - mapOffset + gridOffset, 0, y - mapOffset + gridOffset);

        Instantiate(CheesePrefab, pos, new Quaternion());
    }
}
