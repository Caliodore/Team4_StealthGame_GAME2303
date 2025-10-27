using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.GridBrushBase;

namespace Cali
{
    public class PlayerLogic : PlayerBase
    {
        /*
         * ToDo Post-10/26
         * Zhamanta's Requests:
         * - Clarification on player deaths/downs and updating the UI respectively. 
         * 
         */


        PlayerLogic playerInst;

        [Header("Component Refs")]
        [SerializeField] Collider alertGuardsCollider;
        [SerializeField] GameObject restrictedAreaObject;   //Any singular restricted area item should be fine, as long as they all have the correct tag.

        [Header("Interaction Vars")]
        public UnityEvent playerAttemptInteract;
        public bool isDoorListening = false, playerInteracting = false, interactionTimerDone = false;
        public GameObject lastCollidedObj;
        public float interactTime;

        [Header("Internal Vars")]
        public string restrictedAreaTag;
        public List<string> tagRefs;

        private void Awake()
        {
            playerInst = GetComponent<PlayerLogic>();
            GenerateTagRefs();
            if (playerAttemptInteract == null)
                playerAttemptInteract = new UnityEvent();
        }

        private void GenerateTagRefs()
        {
            string[] allTags = UnityEditorInternal.InternalEditorUtility.tags;
            for (int i = 7; i < allTags.Length; i++)
            {
                tagRefs.Add(allTags[i]);
            }
        }

        /// <summary>
        /// Central method for handling what to change based on tags of colliding objects.
        /// </summary>
        /// <param name="inputTag">Object's tag.</param>
        private void TagBasedSwitch(string inputTag, bool enterOrExit)
        {
            int tagIndex = 0;
            if (!tagRefs.Contains(inputTag))
            {
                print("That object is not set to a custom dev tag. Logic is only done for tags made by the devs.");
                return;
            }
            else if (tagRefs.Contains(inputTag))
            {
                tagIndex = tagRefs.IndexOf(inputTag);
            }

            switch (tagIndex)
            {
                case 0:        //Door

                    break;

                case 1:        //Valuable
                    //Call whatever method updates the loot counter and destroys the reference object/sets it inactive.
                    break;

                case 2:        //Exit

                    break;

                case 3:        //Room
                    //Maybe we could have a public property that is a string of what room the player is in?
                    break;

                case 4:        //RestrictedArea
                    TrespassingToggle(enterOrExit);
                    break;

                case 5:         //(Empty for expansion)

                    break;
            }
        }

        public void TrespassingToggle(bool enterOrLeave)
        {
            if(enterOrLeave)
                playerInst.IsTrespassing = true;
            else
                playerInst.IsTrespassing = false;
        }   

        private void OnTriggerEnter(Collider other)
        {
            string colliderEnterTag = other.tag;
            lastCollidedObj = other.gameObject;
            TagBasedSwitch(colliderEnterTag, true);
        }

        private void OnTriggerStay(Collider other)
        {
            string colliderEnterTag = other.tag;
            if(other.CompareTag(restrictedAreaTag) && !IsTrespassing)
            { 
                TrespassingToggle(true);    
            }
        }

        private void OnTriggerExit(Collider other)
        {
            string colliderExitTag = other.tag;
            TagBasedSwitch(colliderExitTag, false);
        }

        public void OnInteract(InputAction.CallbackContext ctx)
        {
            if (isDoorListening)
            {
                if (ctx.started)
                {
                    playerInteracting = true;
                    StartCoroutine(ObjectInteraction());
                }
                if (ctx.canceled)
                {
                    playerInteracting = false;
                }
            }
        }

        IEnumerator ObjectInteraction()
        {
            print("Player has started interacting with a door.");
            float elapsedTime = 0;
            while (playerInteracting && !interactionTimerDone)
            {
                interactionTimerDone = (elapsedTime >= interactTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }
            if(!playerInteracting && !interactionTimerDone)
            { 
                print("Player stopped interacting before finishing interaction.");    
            }
            else if(interactionTimerDone)
            { 
                print("Player finished interaction.");
                //The DoorLogic handles the outcome of the interaction. These are mainly here for debugging/expansion.
            }
            yield return null;
        }
    }
}