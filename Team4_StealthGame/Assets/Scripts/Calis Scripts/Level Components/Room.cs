using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Room : MonoBehaviour
{
    Room thisRoom;

    [Header("Self-Refs")]
    public Collider attachedCollider;
    public Transform roomPos;
    public Transform[] attachedPatrolPoints;
    public Transform[] possibleDoors;

    private void Awake()
    {
        thisRoom = gameObject.GetComponent<Room>();
    }

    private void FindDoors()
    {
        Door[] doorArr = GetComponentsInChildren<Door>();
        foreach(Door doorScript in doorArr)
        { 
            Transform doorTrans = doorScript.transform;
            GameObject doorObj = doorTrans.gameObject;
        }
    }
}
