using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cali
{
    public class Room : MonoBehaviour
    {
        Room thisRoom;

        [Header("Self-Refs")]
        public Collider attachedCollider;
        public Transform roomPos;
        public Transform[] attachedPatrolPoints;
        public Transform[] possibleDoors;
        public List<GameObject> doorList;

        private DoorType storedDoorType;

        private void Awake()
        {
            thisRoom = gameObject.GetComponent<Room>();
            FindDoors();
        }

        private void FindDoors()
        {
            Door[] doorArr = GetComponentsInChildren<Door>();
            foreach (Door doorScript in doorArr)
            {
                Transform doorTrans = doorScript.transform;
                GameObject doorObj = doorTrans.gameObject;
                doorList.Add(doorObj);
            }
        }

        public void SealAttachedDoors(bool sealOrUnseal)
        {
            foreach (GameObject currentDoor in doorList)
            {
                DoorLogic currentScript = currentDoor.GetComponent<DoorLogic>();
                if (sealOrUnseal)
                {
                    currentScript.ChangeDoorState(DoorType.Sealed);
                }
                else
                {
                    currentScript.ChangeDoorState(currentScript.storedDoorState);
                }
            }
        }
    }
}