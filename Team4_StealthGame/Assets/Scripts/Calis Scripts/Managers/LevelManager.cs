using NUnit.Framework;
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using static Door;
using UnityEngine.AI;
using Unity.AI.Navigation;
using UnityEngine.InputSystem;
using System.Linq;
using UnityEngine.Events;

namespace Cali
{ 
/// <summary>
/// Script intended to keep track of and simplify communication/calculations between player actions, the director, and the physical components of the level.
/// Specifically, it does not keep track of or interact with the guards directly, that is done through GuardManager.
/// </summary>
public class LevelManager : MonoBehaviour
{
    /*
    * Whenever a level is created, this script should create collections for all the physical dynamic components of the level that the director would
    * want to interact with. Doors, exits, valuables, lockers and their sub-types will be stored here along with caching their positions. The director
    * will call methods within this script to manipulate the level usually. Example: Director will say seal all electronic doors, telling this script to
    * run the corresponding method(s). Director says to only close doors closest to lockers, same deal. For closing things close to the players that are
    * spotted, the director could pass in player positions to a method that determines the closest door and closes them as such.
    */

    /* Notes/Ideas:
     * -Have restricted areas handled by a collider that will trigger OnEnter and OnExit to change colliders and suspiciousness bools.
     */

    [Header("Interactions")]
    [SerializeField] UnityEvent toggleLockdown;
    [SerializeField] GuardManager guardManager;
    [SerializeField] NavMeshSurface levelMesh;
    public bool lockdownActive;

    [Header("Door Collections")]
    [SerializeField] List<GameObject> doorList;
    [SerializeField] Dictionary<GameObject, DoorType> doorStateRef;
    [SerializeField] Dictionary<GameObject, Vector3> doorPosRef;
    [SerializeField] Dictionary<GameObject, GameObject> roomDoorsRef;

    [Header("Valuable Collections")]    
    [SerializeField] List<GameObject> valuableList;
    [SerializeField] Dictionary<GameObject, int> valuableTypeRef;
    [SerializeField] Dictionary<GameObject, Vector3> valuablePosRef;

    [Header("Exit Collections")]
    [SerializeField] List<GameObject> exitList;
    [SerializeField] Dictionary<GameObject, int> exitTypeRef;
    [SerializeField] Dictionary<GameObject, Vector3> exitPosRef;

    [Header("Player References")]
    [SerializeField] Dictionary<GameObject, Transform> playerObjTransColl;
    [SerializeField] Transform playerLastKnownLocation;

    private void Awake()
    {
        playerObjTransColl = new Dictionary<GameObject, Transform>();
        lockdownActive = false;
        GenerateCollections();
    }

    private void GenerateCollections()
    {
        print("Starting collection generation.");
        DoorCollectionGeneration();
        RoomCollectionGeneration();
        ValuableCollectionGeneration();
        ExitCollectionGeneration();
        print("Finished generating collections.");
    }

    private void DoorCollectionGeneration()
    { 
        int iCount = 0;
        GameObject[] doorArray = GameObject.FindGameObjectsWithTag("Door");

        if(doorArray.Length == 0)
        { 
            print("No doors found.");
            return;
        }

        doorList = doorArray.ToList();
        foreach(GameObject currentEntry in doorList)
        { 
            DoorLogic doorScript = currentEntry.GetComponent<DoorLogic>();
            doorStateRef.Add(currentEntry, doorScript.currentDoorState);
            doorPosRef.Add(currentEntry, currentEntry.transform.position);
            toggleLockdown.AddListener(doorScript.lockdownToggle);
            iCount++;
        }
        print($"Finished generating door collections in {iCount} iterations.\nDoorStateRef Entries: {doorStateRef.Keys.Count} Keys and {doorStateRef.Values.Count} Values.\nDoorPositionRef Entries: {doorPosRef.Keys.Count} Keys and {doorPosRef.Values.Count} Values.");
    }

    private void RoomCollectionGeneration()
    {
        int dCount = 0;
        int rCount = 0;
        int dInRCount = 0;
        GameObject[] roomArray = GameObject.FindGameObjectsWithTag("Room");

        if(roomArray.Length == 0)
        { 
            print("No rooms found.");
            return;
        }

        var roomList = roomArray.ToList();
        foreach(GameObject roomRef in roomArray)
        {
            foreach(GameObject doorInRoom in roomRef.GetComponent<Room>().doorList)
            { 
                roomDoorsRef.Add(roomRef, doorInRoom);
                dCount++;
            }
            dInRCount = dCount;
            rCount++;
            print($"Room {rCount} has {dInRCount} doors.\nTotal So-Far || Rooms: {rCount} || Doors: {dCount}");
            dInRCount = 0;
        }
        print($"Finished Room/Door collection in {dCount} interior-loop iterations and {rCount} exterior-loop iterations.\nRoom/Door Coll || Rooms: {rCount} || Doors: {dCount}");
    }

    private void ValuableCollectionGeneration()
    {
        int iCount = 0;
        GameObject[] valuableArray = GameObject.FindGameObjectsWithTag("Valuable");

        if(valuableArray.Length == 0)
        { 
            print("No rooms found.");
            return;
        }

        valuableList = valuableArray.ToList();
        foreach(GameObject currentEntry in valuableList)
        { 
            int valInt = 0;
            if(currentEntry)
            valuableTypeRef.Add(currentEntry, valInt);
            valuablePosRef.Add(currentEntry, currentEntry.transform.position);
            iCount++;
        }
        print($"Finished generating valuables collections in {iCount} iterations.\nValuableStateRef Entries: {valuableTypeRef.Keys.Count} Keys and {valuableTypeRef.Values.Count} Values.\nValuablePositionRef Entries: {valuablePosRef.Keys.Count} Keys and {valuablePosRef.Values.Count} Values.");
    }

    private void ExitCollectionGeneration()
    {
        int iCount = 0;
        GameObject[] exitArray = GameObject.FindGameObjectsWithTag("Exit");

        if(exitArray.Length == 0)
        { 
            print("No rooms found.");
            return;
        }

        exitList = exitArray.ToList();
        foreach(GameObject currentEntry in exitList)
        { 
            int valInt = 0;
            if(currentEntry)
            exitTypeRef.Add(currentEntry, valInt);
            exitPosRef.Add(currentEntry, currentEntry.transform.position);
            iCount++;
        }
        print($"Finished generating valuables collections in {iCount} iterations.\nValuableStateRef Entries: {exitTypeRef.Keys.Count} Keys and {exitTypeRef.Values.Count} Values.\nValuablePositionRef Entries: {exitPosRef.Keys.Count} Keys and {exitPosRef.Values.Count} Values.");
    }

    /// <summary>
    /// Sees if GameObject reference for doorRef is stored, if so then update the associated DoorType.
    /// </summary>
    /// <param name="doorRef">GameObject ref of the door entry being changed.</param>
    /// <param name="changeToState">State that the door is changing TO.</param>
    public void DoorStateUpdate(GameObject doorRef, DoorType changeToState)
    { 
        if(doorStateRef.ContainsKey(doorRef))
        {
            doorStateRef[doorRef] = changeToState;
            print($"DoorStateRef updated {doorRef.name} state to {changeToState}.");
            if (changeToState == DoorType.Destroyed)
            {
                toggleLockdown.RemoveListener(doorRef.GetComponent<DoorLogic>().lockdownToggle);
                doorStateRef.Remove(doorRef);
            }
        }
        else
        { 
            print($"GameObj reference not found in Dictionary.\nDoorRef: {doorRef.name} || StateChange: {changeToState}");
        }
    }

    /// <summary>
    /// Call whenever a player is added/removed so references can be updated for later use.
    /// </summary>
    /// <param name="playerChanged"> The GameObject attached to the player being altered. </param>
    /// <param name="addingPlayer"> True = Added || False = Removed </param>
    public void PlayerCollectionUpdate(GameObject playerChanged, bool addingPlayer)
    { 
        bool dictCheck;
        if(!playerChanged.CompareTag("Player"))
        { 
            print("Object reference does not have the Player tag, this method is only for use with objects with the Player tag.");
            return;
        }

        if(playerObjTransColl.Count > 0)
        {
            dictCheck = playerObjTransColl.ContainsKey(playerChanged);
        }
        else
            dictCheck = false;
        
        if(!dictCheck)          //Object reference not found in keys, i.e. the player object hasn't been noted yet in the dictionary.
        { 
            if(addingPlayer)    //Since ref is not in dictionary yet, if true we want to add it!
            { 
                playerObjTransColl.Add(playerChanged, playerChanged.GetComponent<Transform>());
            }
            else                //Ref is not in dictionary and is not supposed to be added, therefore this is an error most likely.
            {
                print($"PlayerObjRef not found and input addingPlayer ({addingPlayer}) states that it should not be added. If wanting to add this player reference to the dictionary please invert the value of addingPlayer.");
            }
        }
        else if(dictCheck && !addingPlayer)     //Player ref IS in dictionary, and we DO want to remove it.
        { 
            playerObjTransColl.Remove(playerChanged);
        }
        else                    //Player ref is already in dictionary and whatever is attempting to add it again, so we should just throw an error stating so.
        { 
            print("PlayerObjRef is already in dictionary. If attempting to remove try inverting addingPlayer, your input bool.");
        }

        print($"There are {playerObjTransColl.Keys.Count} PlayerGameObjectRefs and {playerObjTransColl.Values.Count} PlayerTransformRefs recorded.");

        if(!addingPlayer && playerObjTransColl.ContainsKey(playerChanged))
        { 
            print("There are still transforms associated with this GameObject. Cali needs to troubleshoot PlayerCollectionUpdate in this case.");
        }
    }

    /// <summary>
    /// For debugging handling multiple players while using InputActions.
    /// </summary>
    /// <param name="ctx"></param>
    public void AddPlayerRemoteTest(InputAction.CallbackContext ctx)
    {
        if(ctx.started)
        {
            print("Attempting to find playerRef in scene and add to dictionary.");
            GameObject playerRef = GameObject.FindGameObjectWithTag("Player");
            if (playerRef != null)
                PlayerCollectionUpdate(playerRef, true);
            else
                print("Could not find an object with the tag 'Player'.");
        }
    }
    
    /// <summary>
    /// Call to seal the doors within a specific room.
    /// </summary>
    public void SealUnsealRoom(GameObject roomRef, bool sealOrUnseal)
    {
        roomRef.GetComponent<Room>().SealAttachedDoors(sealOrUnseal);
    }

    /// <summary>
    /// Call to start/end a lockdown.
    /// </summary>
    /// <param name="lockdownStatus">T = ACTIVATE LOCKDOWN || F = RELEASE LOCKDOWN</param>
    public void LockdownHandler()
    {
        toggleLockdown.Invoke();
    }

    /// <summary>
    /// Mainly for the GuardManager to update references to NavMeshObstacles. Individual guards can handle the pathing.
    /// </summary>
    public void UpdateObstacleReferences()
    {
        guardManager.UpdateNavMeshesCall();
    }
}
}
