using UnityEngine;
using UnityEngine.Events;

public class Guard_DetectionSensor : MonoBehaviour
{

    [SerializeField] UnityEvent<bool, bool, bool> onHeard;
    [SerializeField] UnityEvent<bool> onSight;
    Player_Movement player;

    public bool playerInSensor = false;

    private void Start()
    {
      player = FindAnyObjectByType<Player_Movement>();
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.CompareTag("Player"))
        {
            playerInSensor = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInSensor = false;
            onSight.Invoke(playerInSensor);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            onHeard.Invoke(player.isMakingSound, player.isMoving, playerInSensor);
            onSight.Invoke(playerInSensor);
        }
    }
}
