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

        private void Awake()
        {
            thisRoom = gameObject.GetComponent<Room>();
            FindDoors();
        }

        /// <summary>
        /// Internal method for generating a collection of doors to attach to a room.
        /// </summary>
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

        /// <summary>
        /// Method mainly called by LevelManager to seal all connected doors to the room.
        /// </summary>
        /// <param name="sealOrUnseal">T = SEAL || F = UNSEAL</param>
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

        /// <summary>
        /// Similar to SealAttachedDoors, meant for unlocking if the players hack into the mainframe or w/ever.
        /// </summary>
        /// <param name="lockOrUnlock">T = LOCK || F = UNLOCK</param>
        public void UnlockAllDoors(bool lockOrUnlock)
        { 
            foreach (GameObject currentDoor in doorList)
            {
                DoorLogic currentScript = currentDoor.GetComponent<DoorLogic>();
                if (lockOrUnlock)
                {
                    currentScript.ChangeDoorState(DoorType.Open);
                }
                else
                {
                    currentScript.ChangeDoorState(currentScript.storedDoorState);
                }
            }
        }
    }
}