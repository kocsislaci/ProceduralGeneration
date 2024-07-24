using UnityEngine;
using UnityEngine.Events;

public class CheeseController : MonoBehaviour
{
    public UnityEvent<string> OnCheeseCollected = new();

    public void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            var playerController = other.gameObject.GetComponent<PlayerController>();
            Debug.Log($"{playerController.OwnerClientId} has collected cheese");
            OnCheeseCollected.Invoke(playerController.OwnerClientId + " as " + playerController.PlayerName);
        }
    }
}
