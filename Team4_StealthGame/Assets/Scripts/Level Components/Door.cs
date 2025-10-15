using UnityEngine;
public enum DoorType 
{ 
    Open = 0,
    Picked = 1,
    PLocked = 2,
    ELocked = 3,
    Sealed = 4,
    Destroyed = 5
}

public class Door : MonoBehaviour
{
    public DoorType doorState { get; private set; }

    public Door()
    {
        doorState = DoorType.Open;    
    }

    public DoorType GetState()
    { 
        return doorState;
    }
}
