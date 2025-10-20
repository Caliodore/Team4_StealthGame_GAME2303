using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

/// <summary>
/// Script for handling guard interactions at scale, individualized guard interactions are handled by GuardLogic.
/// This determines how the guard body AS A WHOLE interacts, not the individual guards themselves.
/// </summary>
public class GuardManager : MonoBehaviour
{
    /*
     * The purpose of this script is to make referencing groups of guards easier, and to handle determining which guards react to certain events within the game.
     * Similar to the LevelManager we have dictionaries with cached references to prevent unnecessary GetComponent usage at run-time, caching references early.
     * GameObject will be the main key used for referencing dictionaries.
     * When discussing "guards" the intent is to mean the ENTIRE body of guards, not individual guards.
     * The responses are intended to be when an individual guard communicates to the central guard brain that said event has happened.
     * 
     * Concepts this script is intended to handle guards reacting to:
     * -A body/unconscious person being found.
     * -Doors being found broken open.
     * -A weapon laying on the ground.
     * -A weapon being returned to a guard room.
     * -Target loot missing from its location (need to make sure we create an object with a script like a Dais or something)
     * -An unsilenced gun being fired.
     * -An alarm going off.
     * 
     * We will need to decide if we want there to be a central security that will act as a physical brain for the guard system.
     * If so, we will also need to handle how guards react to it being disabled like communication being cut.
     * Either way, the general reaction to stimuli will be handled by the individual guards that hear/see something.
     * We might have guards call on a certain number of other guards to help search an area as a response, and this script will help designate those guards.
     * 
     * TL;DR: This script will be a set of references that individual guards or other game objects can call upon to request more guards to go-
     * -somewhere or do something in response to player actions.
     */

    /*
     * Notes/Ideas: 
     * We can make structs to hold the various bits and bobs of data that are of different types-
     * -so we only need to reference a single dictionary and not multiple. Not the most necessary but could save some space and be less complex.
     */

    [Header("References to Cache")]
    [SerializeField] public Dictionary<GameObject, Transform> guardObjTransPairs;
    [SerializeField] public List<Transform> allPatrolPoints;
    [SerializeField] public LevelManager levelManager;
    [SerializeField] public NetworkManager netManager;

    [Header("Guard Body Vars")]
    private bool alarmActive;
    private bool unawareOfPlayers;
    private bool currentlySearchingForPlayers;
    private int currentlyActiveGuards;

    //----Property Setting-----//
    public bool AlarmActive
    { 
        get { return alarmActive; }
    }
    public bool UnawareOfPlayers
    { 
        get { return unawareOfPlayers; }
    }
    public bool SearchingForPlayers
    { 
        get { return currentlySearchingForPlayers; }
    }
    //------------------------//

    private void Awake()
    {
        currentlyActiveGuards = FindObjectsByType<GuardLogic>(FindObjectsSortMode.None).Count();
    }

    /// <summary>
    /// Script intended to use in response to players causing a larger/louder distraction.
    /// Grabs from guard list and finds the amountOfGuards closest guards to then send to sendToHere.
    /// </summary>
    /// <param name="amountOfGuards">How many guards are responding to distraction?</param>
    /// <param name="sendToHere">Where was the distraction source?</param>
    public void GetNearestGuards(int amountOfGuards, Vector3 sendToHere)
    { 
        Dictionary<float, GameObject> relativePositions = new Dictionary<float, GameObject>();
        float[] distances = new float[currentlyActiveGuards];
        relativePositions.Clear();

        foreach(KeyValuePair<GameObject,Transform> guardTransformRef in guardObjTransPairs)
        { 
            var relDis = Vector3.Distance(guardTransformRef.Value.position, sendToHere);
            relativePositions.Add(relDis, guardTransformRef.Key);
        }

        Array.Sort(distances);

        for(int i = 0; i < amountOfGuards; i++)
        {
            relativePositions.TryGetValue(distances[i], out GameObject guardRef);
            if (guardRef != null) 
            {
                //Get whichever movement script is associated with guardRef GameObject and call the public method to investigate the sendToHere position.
            }
        }
    }
}
