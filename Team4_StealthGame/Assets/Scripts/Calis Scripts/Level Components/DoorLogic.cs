using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Cali

{

    public struct DoorBools
    {  
        public bool collisionEnabled { get; private set; }
        public bool isSuspicious { get; private set; }
        public bool isInteractable { get; private set; }
        public bool blocksGuardPathing { get; private set; }

        /// <summary>
        /// bools = collisionEnabled, isSuspicious, isInteractable, blocksGuardPathing
        /// </summary>
        /// <param name="collisionEnabled">Whether or not collision should be enabled.</param>
        /// <param name="isSuspicious">Whether it draws guards' attention.</param>
        /// <param name="isInteractable">Whether or not the player can interact with it.</param>
        /// <param name="blocksGuardPathing">Whether or not the attached NavMeshObstacle is active or not.</param>
        public DoorBools(bool collisionEnabled, bool isSuspicious, bool isInteractable, bool blocksGuardPathing)
        { 
            this.collisionEnabled = collisionEnabled;
            this.isSuspicious = isSuspicious;
            this.isInteractable = isInteractable;
            this.blocksGuardPathing = blocksGuardPathing;
        }

        /// <summary>
        /// Set states of interactivity for this door.
        /// </summary>
        /// <param name="collisionEnabled">Whether or not collision should be enabled.</param>
        /// <param name="isSuspicious">Whether it draws guards' attention.</param>
        /// <param name="isInteractable">Whether or not the player can interact with it.</param>
        /// <param name="blocksGuardPathing">Whether or not the attached NavMeshObstacle is active or not.</param>
        public void SetBools(bool collisionEnabled, bool isSuspicious, bool isInteractable, bool blocksGuardPathing)
        { 
            this.collisionEnabled = collisionEnabled;
            this.isSuspicious = isSuspicious;
            this.isInteractable = isInteractable;
            this.blocksGuardPathing = blocksGuardPathing;
        }

        public override string ToString()
        {
            return new string($"{collisionEnabled.ToString()}, {isSuspicious.ToString()}, {isInteractable.ToString()}, {blocksGuardPathing.ToString()}");
        }
    }

    public class DoorLogic : Door
    {
        public DoorType currentDoorState { get; private set; }
        [SerializeField] NavMeshObstacle attachedMeshObstacle;
        public UnityAction lockdownToggle;
        public UnityEvent playerFailsInteraction;
        public DoorType storedDoorState;

        private LevelManager levelManager;
        private DoorBools doorInteractivity;
        private Vector3 doorPosition;

        [Header("Player Interaction")]
        public Collider attachedCollider;
        public Collider lastPlayerCollided;
        public List<Collider> playersInCollider;
        public List<Collider> guardSensorsInCollider;
        public string playerTag;

        private void Awake()
        {
            attachedCollider = this.gameObject.GetComponent<Collider>();
            doorPosition = gameObject.transform.position;
            lockdownToggle += LockdownToggle;
            playerTag = FindAnyObjectByType<PlayerLogic>().tag;
        }

        /// <summary>
        /// Updates door's state to whichever is needed, whether by guards, players, or director/manager.
        /// </summary>
        /// <param name="changeToState"></param>
        public void ChangeDoorState(DoorType changeToState)
        {
            if(currentDoorState != changeToState)
            {
                storedDoorState = currentDoorState;
                print("Calling StateChanged");
                currentDoorState = changeToState;
                StateChanged();
                print($"Door State changed to: {currentDoorState}.");
            }
            else
                print($"This door is already set to State {currentDoorState}.");
        }

    
        /// <summary>
        /// For mainly updating the DoorBools of attached door script, and then calling the appropriate methods to update other game components attached to the door.
        /// </summary>
        private void StateChanged()
        {
            switch(currentDoorState)        //Set collision and sus vars this switch case.
            { 
                case(DoorType.Open):        //When a door is simply able to be passed through by all.
                    doorInteractivity.SetBools(false, false, true, false);      
                    break;                  //CAN be walked through, does NOT arouse suspicion from guards, and CAN be interacted with still, guards WILL path through.

                case (DoorType.Picked):     //When a door was locked by some means, but is now open, but can be locked again.
                    doorInteractivity.SetBools(false, true, true, false);
                    break;                  //CAN be walked through, is NOT suspicious, CAN be interacted with, guards WILL path through.

                case(DoorType.PLocked):     //When a door is locked by a physical lock.
                    doorInteractivity.SetBools(true, false, true, false);
                    break;                  //Can NOT be walked through, is NOT suspicious, CAN be interacted with, guards WILL path through.

                case(DoorType.ELocked):     //When a door is electronically/remotely locked OR needs a keycard.
                    doorInteractivity.SetBools(true, false, true, false);
                    break;                  //Can NOT be walked through, is NOT suspicious, CAN be interacted with, guards WILL path through.

                case(DoorType.Sealed):      //When a door has been physically and electronically sealed shut and there is no way to finesse through it.
                    doorInteractivity.SetBools(true, false, false, true);
                    break;                  //Can NOT be walked through, is NOT suspicious, can NOT be interacted with, guards will NOT path through.

                case(DoorType.Destroyed):   //When a door is permanently open due to being destroyed. Normally can just GameObject.Destroy() when this happens.
                    doorInteractivity.SetBools(false, true, false, false);
                    //Update collections that reference this door before destroying GameObject.
                    Destroy(gameObject);
                    break;                  //CAN be walked through, IS suspicious, can NOT be interacted with, guards WILL path through.
            }
            print(doorInteractivity.ToString());
            levelManager.DoorStateUpdate(gameObject,currentDoorState);
            UpdateDoorComponents();
        }

        /// <summary>
        /// Just for testing purposes, cycle through the DoorType states on call.
        /// </summary>
        /// <param name="ctx"></param>
        public void RemoteStateChange(InputAction.CallbackContext ctx)
        {
            if(ctx.started)
            {
                int stateInt = (int)currentDoorState;
                stateInt++;
                stateInt%=5;
                ChangeDoorState((DoorType)stateInt);
            }
        }

        /// <summary>
        /// Made for testing to force the door back to DoorType.Open, i.e., the beginning of the enum index.
        /// </summary>
        /// <param name="ctx"></param>
        public void SetDoorOpen(InputAction.CallbackContext ctx)
        { 
            if(ctx.started)
                ChangeDoorState(0);    
        }

        /// <summary>
        /// To be called by the state/logic handler after updating DoorInteractivity and stuff.
        /// </summary>
        private void UpdateDoorComponents()
        {
            if(doorInteractivity.collisionEnabled)
            {
                attachedCollider.enabled = true;
            }
            else if(!doorInteractivity.collisionEnabled)
            { 
                attachedCollider.enabled = false;
            }
        //------------------------------------------------//
            if(doorInteractivity.isSuspicious)
            { 
                //Enable suspicion collider/coroutine.
            }
            else if(!doorInteractivity.isSuspicious)
            { 
                //Invert above values.
            }
        //------------------------------------------------//
            if(doorInteractivity.isInteractable)
            { 
                //Placeholder
            }
            else if(!doorInteractivity.isInteractable)
            { 
                //Invert it.
            }
        //------------------------------------------------//
            if(doorInteractivity.blocksGuardPathing)
            { 
                attachedMeshObstacle.enabled = true;
            }
            else if(!doorInteractivity.blocksGuardPathing)
            { 
                attachedMeshObstacle.enabled = true;
            }
        }

        /// <summary>
        /// Method to be called when player attempts to interact with a door by holding a button like E or w/ever we decide.
        /// </summary>
        public void PlayerInteractHandler()
        { 
            if(doorInteractivity.isInteractable)
            {
                switch(currentDoorState)
                {
                    case(DoorType.Open):
                        //Players can close the door to block LoS or obscure sound.
                        break;

                    case(DoorType.Picked):
                        //Players can re-lock.
                        break;

                    case(DoorType.PLocked):
                        //Player needs to be able to lockpick, maybe some have tools that make it faster?
                        break;

                    case(DoorType.ELocked):
                        //Player needs to be able to hack.
                        break;
                }
            }
            else
            { 
                print($"Player cannot interact with this door, it is: {currentDoorState}");    
            }
        }

        /// <summary>
        /// To be invoked by an event in PlayerLogic for players currently colliding with the door.
        /// Use this to trigger animation starts and UI updates.
        /// </summary>
        private void DoorInteractBuffer(PlayerLogic interactingPlayer)
        {
            if(doorInteractivity.isInteractable)
            { 
                print($"Player started interacting with the door at {doorPosition}.");
                StartCoroutine(AwaitingPlayerToFinishInteraction(interactingPlayer));
            }
            else
            { 
                print("Player cannot interact with this door, this code should've been unreachable.");
                return;
            }
        }

        IEnumerator AwaitingPlayerToFinishInteraction(PlayerLogic interactingPlayer)
        {
            while(interactingPlayer.playerInteracting)
            { 
                //Mainly an area for UI updates over time, like a circular icon filling up radially.
                if(interactingPlayer.interactionTimerDone)
                { 
                    PlayerInteractHandler();    
                }
                yield return null;
            }
            if(!interactingPlayer.playerInteracting && !interactingPlayer.interactionTimerDone)
            { 
                //Depending on the DoorType we could sound an alarm or make a noise if they interrupt their action.
                playerFailsInteraction?.Invoke();
            }
            yield return null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag(playerTag))
            {
                PlayerLogic playerEntering = other.GetComponent<PlayerLogic>();
                bool playerFoundAlready = playersInCollider.Contains(other);
                if(playerFoundAlready)
                { 
                    print("Player is already in the list for tracking currently colliding players. Either they were not removed or OnTriggerEnter is being called more than it needs to be with the same player collider.");
                }
                else
                { 
                    playersInCollider.Add(other);
                    playerEntering.playerAttemptInteract.AddListener(() => DoorInteractBuffer(playerEntering));
                    playerEntering.isDoorListening = true;
                }
            }
            else if(other.name == "Sensor")
            {
                GuardLogic guardScript = other.GetComponent<GuardLogic>();
                bool guardAlreadyTracked = guardSensorsInCollider.Contains(other);
                if(!guardAlreadyTracked)
                { 
                    guardSensorsInCollider.Add(other);
                    playerFailsInteraction.AddListener(() => levelManager.CallGuard(guardScript.gameObject, doorPosition));
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if(other.CompareTag(playerTag))
            {
                PlayerLogic playerLeaving = other.GetComponent<PlayerLogic>();
                bool playerFoundAndRemoved = playersInCollider.Remove(other);
                if(!playerFoundAndRemoved)
                {
                    print($"Either player was not found as having already collided or started in this collider. PlayerObjName: {other.gameObject.name}"); 
                    return;
                }
                playerLeaving.playerAttemptInteract.RemoveListener(() => DoorInteractBuffer(playerLeaving));
                playerLeaving.isDoorListening = false;
            }
            else if(other.name == "Sensor")
            { 
                GuardLogic guardScript = other.GetComponent<GuardLogic>();
                bool guardNoLongerTracked = guardSensorsInCollider.Remove(other);
                if(guardNoLongerTracked)
                { 
                    playerFailsInteraction.RemoveListener(() => levelManager.CallGuard(guardScript.gameObject, doorPosition));                    
                }
            }
        }

        /// <summary>
        /// Called to toggle the lockdown state of this specific door.
        /// </summary>
        private void LockdownToggle()
        { 
            if(levelManager.lockdownActive)
            { 
                storedDoorState = currentDoorState;
                ChangeDoorState(DoorType.Sealed);
            }
            else
            { 
                ChangeDoorState(storedDoorState);
            }
        
        }
    } 
}
