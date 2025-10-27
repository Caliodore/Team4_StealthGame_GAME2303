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
         * ToDo Post-10/26
         * Zhamanta's Requests:
         * - Clarification on UpdateGuardRefs and spawning guards
         *      } Maybe add a separate function to spawn new guards that auto-incorporates UpdateGuardRefs.
         *      } Could also have it as a part of Awake on the base guard logic script? That way we don't even need the collection generator
         * - Dictionary clean-up
         *      } Make a dictionary that puts guards with their base stats.
         *      } Maybe this could be a use for a struct?
         * 
         */

        [Header("References to Cache")]
        [SerializeField] Dictionary<GameObject, GuardLogic> guardObjScriptDict;
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
            hearingDistance = FindAnyObjectByType<Guard_DetectionSensor>().GetComponent<SphereCollider>().radius;
            GenerateRefs();
        }

        private void GenerateRefs()
        {
            GenerateGuardCollection();
        }

        /// <summary>
        /// For use within GuardManager to collect all guards within a dictionary and associate them with their transform refs.
        /// </summary>
        private void GenerateGuardCollection()
        {
            GameObject[] guardArray = GameObject.FindGameObjectsWithTag("Guard");
            if(guardArray.Length != 0)
            {
                foreach (GameObject guard in guardArray) 
                {
                    GuardLogic guardTrans = guard.GetComponent<GuardLogic>();
                    guardObjScriptDict.Add(guard, guardTrans);
                } 
            }
            else
                print("No GameObjects with the tag of 'Guard' found in scene.");
        }

        /// <summary>
        /// Update collections to add/remove guard GameObjects and their associated values.
        /// Checks if GameObject has GuardLogic attached to it, so make sure the reference is the correct level of inheritance.
        /// </summary>
        /// <param name="guardRef">GameObject reference of the guard being altered.</param>
        /// <param name="addOrRemove">T = ADDING || F = REMOVING</param>
        public void UpdateGuardRefs(GameObject guardRef, bool addOrRemove)
        {
            
            bool refFound = guardObjScriptDict.ContainsKey(guardRef);
            bool isGuard = guardRef.TryGetComponent(out GuardLogic attachedScript);
            if (isGuard)
            {
                if (refFound)
                {
                    if (!addOrRemove)
                    {
                        guardObjScriptDict.Remove(guardRef);
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
                        guardObjScriptDict.Add(guardRef, guardRef.GetComponent<GuardLogic>());
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

            foreach (KeyValuePair<GameObject, GuardLogic> guardTransformRef in guardObjScriptDict)   //Iterate through the dictionary 
            {
                var relDis = Vector3.Distance(guardTransformRef.Key.transform.position, sendToHere);        //Calculate a distance between the input position and the guard's position.
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
        
        /// <summary>
        /// Call to send a guard to a specific location, using a singular GameObject reference.
        /// </summary>
        /// <param name="guardRef">GameObject the GuardLogic is attached to.</param>
        /// <param name="sendToHere">Where the guards should path to.</param>
        public void SendSpecificGuards(GameObject guardRef, Vector3 sendToHere)
        { 
            guardRef.GetComponent<GuardLogic>().ReactToNoise(sendToHere);
        }
    }
}