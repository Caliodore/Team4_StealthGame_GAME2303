using System;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

//namespace LevelComponent

//{

[Serializable]
public struct DoorBools
{  
    public bool collisionEnabled { get; private set; }
    public bool isSuspicious { get; private set; }
    public bool isInteractable { get; private set; }
    public bool blocksGuardPathing { get; private set; }

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
        return new string($"{collisionEnabled.ToString()},{isSuspicious.ToString()},{isInteractable.ToString()},{blocksGuardPathing.ToString()}");
    }
}

public class DoorLogic : Door
{
    private LevelManager levelManager;
    public DoorType currentDoorState { get; private set; }

    //First bool = Is interaction possible || Second bool = Do guards react to it
    private DoorInteractivity interactionStatus = new DoorInteractivity(true, false);

    private Collider attachedCollider;

    [Header("Interactivity Vars")]
    [SerializeField] private DoorBools doorInteractivity;

    //Yet to be implemented
    //private GameManager gameManager;
    //private AIDirector gameDirector;

    private void Awake()
    {
        attachedCollider = GetComponent<Collider>();
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
        //Then update components/interactivity flags based on the vars changed.
        
    }

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

    public void SetDoorOpen(InputAction.CallbackContext ctx)
    { 
        if(ctx.started)
            ChangeDoorState(0);    
    }
} 
//}
