using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.InputSystem;

//namespace LevelComponent

//{
public class DoorLogic : Door
{
    private LevelManager levelManager;
    public DoorType currentDoorState { get; private set; }

    //First bool = Is interaction possible || Second bool = Do guards react to it
    private DoorInteractivity interactionStatus = new DoorInteractivity(true, false);

    private Collider attachedCollider;

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
            currentDoorState = changeToState;
            StateChanged();
            print($"Door State changed to: {currentDoorState}.");
        }
        else
            print($"This door is already set to State {(int)currentDoorState}, {currentDoorState}.");
    }

    private void StateChanged()
    { 
        switch(currentDoorState)
        { 
            case(DoorType.Open):        //When a door is simply able to be passed through by all.
                interactionStatus.BoolSet(false, false);
                attachedCollider.enabled = false;
                break;

            case (DoorType.Picked):     //When a door was locked by some means, but is now open, but can be locked again.
                interactionStatus.BoolSet(true, true);
                attachedCollider.enabled = false;
                break;

            case(DoorType.PLocked):     //When a door is locked by a physical lock.
                interactionStatus.BoolSet(true, false);
                attachedCollider.enabled = true;
                break;

            case(DoorType.ELocked):     //When a door is electronically/remotely locked OR needs a keycard.
                interactionStatus.BoolSet(true, false);
                attachedCollider.enabled = true;
                break;

            case(DoorType.Sealed):      //When a door has been physically and electronically sealed shut and there is no way to finesse through it.
                interactionStatus.BoolSet(false, false);
                attachedCollider.enabled = true;
                break;

            case(DoorType.Destroyed):   //When a door is permanently open due to being destroyed. Normally can just GameObject.Destroy() when this happens.
                interactionStatus.BoolSet(false, true);
                attachedCollider.enabled = false;
                break;

        }
    }

    public void RemoteStateChange(InputAction.CallbackContext ctx)
    { 
        if(ctx.performed)
        {    
            switch(ctx.control.name)
            {
                case("r"):
                    ChangeDoorState((DoorType)(((int)currentDoorState + 1)%5));
                    break;
                case("t"):
                    ChangeDoorState(DoorType.Open);
                    break; 
                case("f"):
                    ChangeDoorState(DoorType.ELocked);
                    break;
            } 
        }
    }
} 
//}
