using System;
using Unity.Netcode;
using UnityEngine;

public class PlayerController : NetworkBehaviour
{
    private NetworkObject networkObject;

    [SerializeField] private Material[] playerSkins;
    [SerializeField] private MeshRenderer flagMesh;
    [SerializeField] private Rigidbody rigidbody;
    private void Awake() { }

    public override void OnNetworkSpawn()
    {
        GameObject spawnPoints = GameObject.FindWithTag("SpawnPoints");

        var connectedClients = NetworkManager.Singleton.ConnectedClients;

        int spawnIndex = connectedClients.Count % spawnPoints.transform.childCount;

        Debug.Log("hello index: " + spawnIndex + " clients " + connectedClients.Count + " spawnpoints " + spawnPoints.transform.childCount);
        Transform spawnPoint = spawnPoints.transform.GetChild(spawnIndex);

        transform.position = spawnPoint.position;

        networkObject = GetComponent<NetworkObject>();
    }

    private void Start()
    {
        SetColor((uint)OwnerClientId);
    }

    private void Update()
    {
        if (!IsOwner) return;
        Move();
    }

    private void Move()
    {
        float movement = 0f;
        float movementSpeed = 40f;
        float rotation = 0f;
        float rotationSpeed = 80f;

        if (Input.GetKey(KeyCode.W)) movement = +movementSpeed;
        if (Input.GetKey(KeyCode.S)) movement = -movementSpeed;
        if (Input.GetKey(KeyCode.A)) rotation = -rotationSpeed;
        if (Input.GetKey(KeyCode.D)) rotation = +rotationSpeed;
        MoveServerRpc(movement, rotation);
    }

    [ServerRpc]
    private void MoveServerRpc(float movement, float rotation)
    {
        Quaternion deltaRotation = Quaternion.Euler(0, rotation * Time.deltaTime, 0);
        rigidbody.MoveRotation(rigidbody.rotation * deltaRotation);
        rigidbody.MovePosition(transform.position + transform.forward * movement * Time.deltaTime);
    }

    void SetColor(long skinId)
    {
        Debug.Log($"Set Player #{OwnerClientId} color to #{skinId}");
        flagMesh.materials = new Material[] { playerSkins[skinId] };
    }
}
