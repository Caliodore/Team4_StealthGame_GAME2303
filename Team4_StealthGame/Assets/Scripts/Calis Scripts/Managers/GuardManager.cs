using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

namespace Cali
{
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
        [SerializeField] Dictionary<GameObject, Transform> guardObjTransPairs;
        [SerializeField] List<Transform> allPatrolPoints;
        [SerializeField] LevelManager levelManager;
        [SerializeField] NetworkManager netManager;

        [Header("Guard Body Vars")]
        [SerializeField] private bool alarmActive = false;
        private bool unawareOfPlayers = true;
        private bool currentlySearchingForPlayers = false;
        private int currentlyActiveGuards = 0;
        private float hearingDistance;

        //----Property Setting-----//
        [SerializeField] public bool AlarmActive
        {
            get { return alarmActive; }
        }
        [SerializeField] public bool UnawareOfPlayers
        {
            get { return unawareOfPlayers; }
        }
        [SerializeField] public bool SearchingForPlayers
        {
            get { return currentlySearchingForPlayers; }
        }
        [SerializeField] public int CurrentlyActiveGuards
        { 
            get { return currentlyActiveGuards; }    
        }
        //------------------------//

        private void Awake()
        {
            currentlyActiveGuards = FindObjectsByType<GuardLogic>(FindObjectsSortMode.None).Count();
            hearingDistance = FindAnyObjectByType<Sensor>().GetComponent<SphereCollider>().radius;
            GenerateRefs();
        }

        private void GenerateRefs()
        {
            GenerateGuardCollection();
        }

        private void GenerateGuardCollection()
        {
            GameObject[] guardArray = GameObject.FindGameObjectsWithTag("Guard");
            if(guardArray.Length != 0)
            {
                foreach (GameObject guard in guardArray) 
                {
                    Transform guardTrans = guard.transform;
                    guardObjTransPairs.Add(guard, guardTrans);
                } 
            }
            else
                print("No GameObjects with the tag of 'Guard' found in scene.");
        }

        public void UpdateGuardRefs(GameObject guardRef, bool addOrRemove)
        {
            bool refFound = guardObjTransPairs.ContainsKey(guardRef);
            bool isGuard = guardRef.TryGetComponent(out GuardLogic attachedScript);
            if (isGuard)
            {
                if (refFound)
                {
                    if (!addOrRemove)
                    {
                        guardObjTransPairs.Remove(guardRef);
                    }
                    else if (addOrRemove)
                    {
                        print("GuardObj would be a duplicate key, i.e. this guard ref is already stored.");
                        return;
                    }
                }
                else if (!refFound)
                {
                    if (addOrRemove)
                    {
                        guardObjTransPairs.Add(guardRef, guardRef.transform);
                    }
                    else if (!addOrRemove)
                    {
                        print("No reference found to remove, either invert your input bool or double-check your input reference.");
                        return;
                    }
                }
            }
            else
            {
                print("No GuardLogic component found on object reference, make sure the script component is attached to the object you are inputting as referencing.");
                return;
            }
        }

        /// <summary>
        /// Script intended to use in response to players causing a larger/louder distraction.
        /// Grabs from guard list and finds the amountOfGuards closest guards to then send to sendToHere.
        /// </summary>
        /// <param name="amountOfGuards">How many guards are responding to distraction?</param>
        /// <param name="sendToHere">Where was the distraction source?</param>
        public void GetNearestGuards(int amountOfGuards, Vector3 sendToHere)
        {
            bool furthestGuardReached = false;
            bool isGuardInRange = false;
            float currentGuardDis = 0;
            int dictIteration = 0;
            Dictionary<float, GameObject> relativePositions = new Dictionary<float, GameObject>();  //Make a dictionary linking a guard to a distance calc
            float[] distances = new float[currentlyActiveGuards];                                   //Make an array of those distances
            List<GameObject> guardsInRange = new List<GameObject>();
            relativePositions.Clear();

            foreach (KeyValuePair<GameObject, Transform> guardTransformRef in guardObjTransPairs)   //Iterate through the dictionary 
            {
                var relDis = Vector3.Distance(guardTransformRef.Value.position, sendToHere);        //Calculate a distance between the input position and the guard's position.
                relDis = Mathf.Abs(relDis);                                                         //Probably redundant AbsValue but better safe than sorry.
                isGuardInRange = relDis <= hearingDistance;
                if(isGuardInRange)                                                                  //If guard IS in range, then add to array and dict
                {
                    relativePositions.Add(relDis, guardTransformRef.Key);                           //Add that calc to the array and dictionary
                    distances[dictIteration] = relDis;                                                  
                    dictIteration++;
                }
            }

            if(distances.Length <= 0)
            { 
                return;    
            }

            Array.Sort(distances);      //Sort the distances so we can find the nearest x guards.
                
            bool guardsSent = false;
            
            for (int i = 0; i < amountOfGuards; i++)
            {
                currentGuardDis = distances[i];
                furthestGuardReached = ((currentGuardDis > hearingDistance) || (currentGuardDis == 0)); //Bool to check each loop if the distance is further than the guards hearing radius.
                if(!furthestGuardReached)
                { 
                    relativePositions.TryGetValue(currentGuardDis, out GameObject guardRef);            //Double-check that the dict isn't messing up and actually has the guardRef hooked to a distance.
                    if (guardRef != null)
                    {
                        isGuardInRange = true;
                        guardsSent = true;
                        guardRef.GetComponent<GuardLogic>().ReactToNoise(sendToHere);
                    }
                }
                else if(!guardsSent && furthestGuardReached)
                { 
                    isGuardInRange = false;
                    return;
                }
            }
        }
        
        public void SendSpecificGuards(GameObject guardRef, Vector3 sendToHere)
        { 
            guardRef.GetComponent<GuardLogic>().ReactToNoise(sendToHere);
        }
    }
}