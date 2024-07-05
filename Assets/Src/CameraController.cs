using UnityEngine;
using Unity.Netcode;

public class CameraController : MonoBehaviour
{
    NetworkObject player;

    private void Update()
    {
        if (!player)
        {
            player = NetworkManager.Singleton.LocalClient.PlayerObject;
            transform.parent = player.transform;
            transform.localPosition = new Vector3(0, 15.0f, 0.0f);
            transform.LookAt(player.transform.position);
        }
    }
}
