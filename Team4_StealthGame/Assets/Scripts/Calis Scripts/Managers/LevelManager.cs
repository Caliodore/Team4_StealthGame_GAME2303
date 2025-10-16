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

    //Unimplemented so far
    //For referencing positions, we assign each door a number through an array, then we iterate through and store their positions along with that assigned ID.
    //private NavMeshObstacle pathBlockadeRef;
    //private GameObject[] exitArray;
    //private GameObject[] lockerArray;
    //private List<GameObject> lockerList;
    //private List<GameObject> valuablesList;
    //private List<GameObject> exitList;        //Could have a dictionary for exits/doors so we can reference the door ID and then see its state more easily.
    //private 

    private void Awake()
    {
        GenerateCollections();
    }

    private void GenerateCollections()
    {
        print("Starting collection generation.");
        DoorCollectionGeneration();
        ValuableCollectionGeneration();
        //ExitCollectionGeneration();
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

    public void DoorStateUpdate(GameObject doorRef, DoorType changeToState)
    { 
        if(doorStateRef.ContainsKey(doorRef))
        {
            doorStateRef[doorRef] = changeToState;
            print($"DoorStateRef updated {doorRef.name} state to {changeToState}.");
        }
        else
        { 
            print($"GameObj ref not found in Dictionary.\nDoorRef: {doorRef} || StateChange: {changeToState}");    
        }
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
            doorPosRef.Add(currentEntry, currentEntry.transform.position);
            iCount++;
        }
        print($"Finished generating door collections in {iCount} iterations.\nDoorStateRef Entries: {doorStateRef.Keys} Keys and {doorStateRef.Values} Values.\nDoorPositionRef Entries: {doorPosRef.Keys} Keys and {doorPosRef.Values} Values.");
    
            
    }
}
//}
