using NUnit.Framework;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static Door;
using UnityEngine.AI;

//namespace Manager
//{ 
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

    [Header("Door Collections")]
    [SerializeField] private GameObject[] doorArray;
    [SerializeField] private Dictionary<GameObject, DoorType> doorStateRef;
    [SerializeField] private Dictionary<GameObject, Vector3> doorPosRef;

    [Header("Valuable Collections")]    
    [SerializeField] private GameObject[] valuableArray;
    [SerializeField] private Dictionary<GameObject, int> valuableTypeRef;
    [SerializeField] private Dictionary<GameObject, Vector3> valuablePosRef;

    [Header("Exit Collections")]
    [SerializeField] private GameObject[] exitArray;
    [SerializeField] private Dictionary<GameObject, int> exitTypeRef;
    [SerializeField] private Dictionary<GameObject, Vector3> exitPosRef;

    [Header("Player References")]
    [SerializeField] private Dictionary<GameObject, Transform> playerObjTransColl;
    [SerializeField] private Transform playerLastKnownLocation;

    private void Awake()
    {
        GenerateCollections();
    }

    private void GenerateCollections()
    {
        print("Starting collection generation.");
        DoorCollectionGeneration();
        ValuableCollectionGeneration();
        ExitCollectionGeneration();
        print("Finished generating collections.");
    }

    private void DoorCollectionGeneration()
    { 
        int iCount = 0;
        doorArray = GameObject.FindGameObjectsWithTag("Door");
        foreach(GameObject currentEntry in doorArray)
        { 
            doorStateRef.Add(currentEntry, currentEntry.GetComponent<DoorLogic>().currentDoorState);
            doorPosRef.Add(currentEntry, currentEntry.transform.position);
            iCount++;
        }
        print($"Finished generating door collections in {iCount} iterations.\nDoorStateRef Entries: {doorStateRef.Keys} Keys and {doorStateRef.Values} Values.\nDoorPositionRef Entries: {doorPosRef.Keys} Keys and {doorPosRef.Values} Values.");
    }

    private void ValuableCollectionGeneration()
    {
        int iCount = 0;
        valuableArray = GameObject.FindGameObjectsWithTag("Valuable");
        foreach(GameObject currentEntry in valuableArray)
        { 
            int valInt = 0;
            if(currentEntry)
            valuableTypeRef.Add(currentEntry, valInt);
            valuablePosRef.Add(currentEntry, currentEntry.transform.position);
            iCount++;
        }
        print($"Finished generating valuables collections in {iCount} iterations.\nValuableStateRef Entries: {valuableTypeRef.Keys} Keys and {valuableTypeRef.Values} Values.\nValuablePositionRef Entries: {valuablePosRef.Keys} Keys and {valuablePosRef.Values} Values.");
    }

    private void ExitCollectionGeneration()
    {
        int iCount = 0;
        exitArray = GameObject.FindGameObjectsWithTag("Exit");
        foreach(GameObject currentEntry in exitArray)
        { 
            int valInt = 0;
            if(currentEntry)
            exitTypeRef.Add(currentEntry, valInt);
            exitPosRef.Add(currentEntry, currentEntry.transform.position);
            iCount++;
        }
        print($"Finished generating valuables collections in {iCount} iterations.\nValuableStateRef Entries: {exitTypeRef.Keys} Keys and {exitTypeRef.Values} Values.\nValuablePositionRef Entries: {exitPosRef.Keys} Keys and {exitPosRef.Values} Values.");
    }

    public void DoorStateUpdate(GameObject doorRef, DoorType changeToState)
    { 
        if(doorStateRef.ContainsKey(doorRef))
        {
            doorStateRef[doorRef] = changeToState;
            print($"DoorStateRef updated {doorRef.name} state to {changeToState}.");
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
        if(!playerChanged.CompareTag("Player"))
        { 
            print("Object reference does not have the Player tag, this method is only for use with objects with the Player tag.");
            return;
        }

        bool dictCheck = playerObjTransColl.ContainsKey(playerChanged);
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

        print($"There are {playerObjTransColl.Keys} PlayerGameObjectRefs and {playerObjTransColl.Values} PlayerTransformRefs recorded.");

        if(!addingPlayer && playerObjTransColl.ContainsKey(playerChanged))
        { 
            print("There are still transforms associated with this GameObject. Cali needs to troubleshoot PlayerCollectionUpdate in this case.");
        }
    }
}
//}
