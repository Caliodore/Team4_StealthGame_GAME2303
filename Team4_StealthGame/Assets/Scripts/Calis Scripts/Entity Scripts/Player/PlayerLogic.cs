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
        private DoorType desiredDoorState;

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

                    break;

                case 2:        //Exit

                    break;

                case 3:        //Room

                    break;

                case 4:        //RestrictedArea

                    break;

                case 5:

                    break;
            }
        }

        public void TrespassingToggle()
        {
            playerInst.IsTrespassing = !playerInst.IsTrespassing;
        }

        private void OnTriggerEnter(Collider other)
        {
            string colliderEnterTag = other.tag;
            lastCollidedObj = other.gameObject;
            TagBasedSwitch(colliderEnterTag, true);
            if (colliderEnterTag == restrictedAreaTag)
                TrespassingToggle();
        }

        private void OnTriggerExit(Collider other)
        {
            string colliderExitTag = other.tag;
            TagBasedSwitch(colliderExitTag, false);
            if (colliderExitTag == restrictedAreaTag)
                TrespassingToggle();
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
                //Then call whatever method will do the interaction.
            }
            yield return null;
        }
    }
}