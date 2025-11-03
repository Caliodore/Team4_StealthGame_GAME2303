using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Cali;
using Unity.Cinemachine;

public class PatrolPathCreator : MonoBehaviour
{
    /*
     * Input room names/room GameObj references/room script references to calculate a path of patrol points between them.
     * 
     * Theories:
     * - We can determine which patrol point in a room is closest to another room by using the FindClosestPoint of its collider.
     * - We can find the patrol points within a hallway between points by doing a box raycast from collider to collider.
     */

    public LayerMask patrolPointMask;

    private List<Transform> tempPatrolPoints;

    private void Awake()
    {
        patrolPointMask = LayerMask.GetMask("PatrolPoint");
    }

    /// <summary>
    /// Creates an array of patrol points between the first entry in the input array through each of the rooms in order.
    /// </summary>
    /// <param name="roomsToPathBetween"></param>
    public List<Transform> CreatePatrolBetween(GameObject[] roomsToPathBetween)
    {
        int roomIndex = 0;

        foreach(GameObject currentRoom in roomsToPathBetween)
        {
            Collider currentRoomCollider = currentRoom.GetComponent<Collider>();
            Vector3 rayStartPosition = Vector3.zero;
            Vector3 lastPatrolLocation = Vector3.zero;
            Vector3 rayDirection = Vector3.zero;
            Vector3 boxSize = new Vector3(1,1,1);

            Transform[] currentRoomsPatrols = currentRoom.GetComponent<Room>().attachedPatrolPoints;
            foreach(Transform currentInteriorPatrolPoint in currentRoomsPatrols)
            {
                tempPatrolPoints.Add(currentInteriorPatrolPoint); 
            }

            lastPatrolLocation = currentRoomsPatrols[currentRoomsPatrols.Length - 1].position;
            rayStartPosition = currentRoomCollider.ClosestPoint(lastPatrolLocation);

            Vector3 nextRoomClosest = roomsToPathBetween[roomIndex + 1].GetComponent<Collider>().ClosestPoint(rayStartPosition);

            float distanceBetweenRooms = Vector3.Distance(rayStartPosition, nextRoomClosest);
            rayDirection = nextRoomClosest - rayStartPosition;
            rayDirection.Normalize();

            //bool foundPatrolPoints = Physics.BoxCast(rayStartPosition, boxSize, rayDirection, out RaycastHit hits, Quaternion.identity, 999f, patrolPointMask, QueryTriggerInteraction.Collide);
            RaycastHit[] hitsOut = Physics.BoxCastAll(rayStartPosition, boxSize, rayDirection, Quaternion.identity, distanceBetweenRooms, patrolPointMask, QueryTriggerInteraction.Collide);
            if(hitsOut.Length > 0)
            { 
                foreach(RaycastHit currentHit in hitsOut)
                { 
                    tempPatrolPoints.Add(currentHit.collider.gameObject.transform);    
                }
            }
            else
                print("No patrol points found between rooms.");

            roomIndex++;
        }
        if(roomIndex >= roomsToPathBetween.Length)
        { 
            return tempPatrolPoints;    
        }
        else
        { 
            print("Didn't properly iterate through all inputted rooms. Debug PatrolPathCreator script.");
            return tempPatrolPoints;
        }
    }
}
