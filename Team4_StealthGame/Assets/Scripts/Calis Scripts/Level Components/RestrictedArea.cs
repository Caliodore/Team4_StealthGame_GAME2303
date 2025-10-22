using Cali;
using UnityEngine;

public class RestrictedArea : MonoBehaviour
{
    public bool playerTrespassing;
    private string playerTag;

    private void Awake()
    {
        playerTag = FindAnyObjectByType<PlayerLogic>().gameObject.tag;
        if(playerTag == null)
            playerTag = "Player";
    }

    private void OnTriggerEnter(Collider other)
    {
        string collTag = other.tag;
        if(collTag == playerTag)
            playerTrespassing = true;
    }

    private void OnTriggerExit(Collider other)
    {
        string collTag = other.tag;
        if(collTag == playerTag)
            playerTrespassing = false;        
    }
}
