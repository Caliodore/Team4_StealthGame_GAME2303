using UnityEngine;
using UnityEngine.Events;

public class Sensor : MonoBehaviour
{

    [SerializeField] UnityEvent<bool, bool, bool> onHeard;
    [SerializeField] UnityEvent<bool> onSight;
    Controller player;

    public bool playerInSensor = false;

    private void Start()
    {
      player = FindAnyObjectByType<Controller>();
    }

    private void OnTriggerEnter(Collider other)

    {
        if (other.GetComponent<Controller>())
        {
            playerInSensor = true;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Controller>())
        {
            playerInSensor = false;
            onSight.Invoke(playerInSensor);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        onHeard.Invoke(player.isMakingSound, player.isMoving, playerInSensor);
        onSight.Invoke(playerInSensor);
    }
}
