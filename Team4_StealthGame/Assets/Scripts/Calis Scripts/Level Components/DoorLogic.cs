using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

//namespace LevelComponent

//{

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
    private LevelManager levelManager;
    [SerializeField] public DoorType currentDoorState { get; private set; }

    //First bool = Is interaction possible || Second bool = Do guards react to it
    private DoorInteractivity interactionStatus = new DoorInteractivity(true, false);

    private Collider attachedCollider;

    private DoorBools doorInteractivity;

    //Yet to be implemented
    //private GameManager gameManager;
    //private AIDirector gameDirector;

    private void Awake()
    {
        attachedCollider = this.gameObject.GetComponent<Collider>();
    }

    /// <summary>
    /// Updates door's state to whichever is needed, whether by guards, players, or director/manager.
    /// </summary>
    /// <param name="changeToState"></param>
    public void ChangeDoorState(DoorType changeToState)
    {
        if(currentDoorState != changeToState)
        { 
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
                Destroy(gameObject);
                break;                  //CAN be walked through, IS suspicious, can NOT be interacted with, guards WILL path through.
        }
        print(doorInteractivity.ToString());
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
            //Activate NavMeshObstacle and make sure guard paths update.
        }
        else if(!doorInteractivity.blocksGuardPathing)
        { 
            //Deactivate and update guard paths.
        }
    }

    /// <summary>
    /// Method to be called when player attempts to interact with a door.
    /// </summary>
    public void PlayerInteractHandler()
    { 
        if(doorInteractivity.isInteractable)
        { 
            //We could use a switch case here or another struct to handle bools a bit neater.
            //When it comes to being interactable doors can be picked, re-locked, or simply closed maybe?
        }
        else
        { 
            print($"Player cannot interact with this door, it is: {currentDoorState.ToString()}");    
        }
    }
} 
//}
