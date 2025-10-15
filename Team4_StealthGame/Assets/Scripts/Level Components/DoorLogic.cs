using UnityEditor.Build.Content;
using UnityEngine;

//namespace LevelComponent

//{
public class DoorLogic : Door
{
    private LevelManager levelManager;
    public DoorType currentDoorState { get; private set; }
    private Collider attachedCollider;

    //Yet to be implemented
    //private GameManager gameManager;
    //private AIDirector gameDirector;

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
        }
        else
            print($"This door is already set to State {(int)currentDoorState}, {currentDoorState}.");
    }

    private void StateChanged()
    { 
        switch(currentDoorState)
        { 
            case(DoorType.Open):        //When a door is simply able to be passed through by all.
                break;

            case (DoorType.Picked):     //When a door was locked by some means, but is now open, but can be locked again.
                break;

            case(DoorType.PLocked):     //When a door is locked by a physical lock.
                break;

            case(DoorType.ELocked):     //When a door is electronically/remotely locked OR needs a keycard.
                break;

            case(DoorType.Sealed):      //When a door has been physically and electronically sealed shut and there is no way to finesse through it.
                break;

            case(DoorType.Destroyed):   //When a door is permanently open due to being destroyed. Normally can just GameObject.Destroy() when this happens.
                break;

        }
    }
} 
//}
