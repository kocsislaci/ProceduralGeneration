using UnityEngine;
using Unity.Netcode;

public class CameraController : MonoBehaviour
{
    NetworkObject player;

    [SerializeField] private bool isFixed = false;

    private void Update()
    {
        if (!player)
        {
            player = NetworkManager.Singleton.LocalClient.PlayerObject;
            transform.parent = player.transform;
            transform.localPosition = new Vector3(0, 50.0f, 0.0f);
            transform.LookAt(player.transform.position, Vector3.forward);
        }
        if (player && isFixed)
        {
            transform.LookAt(player.transform.position, Vector3.forward);
        }
    }
}
