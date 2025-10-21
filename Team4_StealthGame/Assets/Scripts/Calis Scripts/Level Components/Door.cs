using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum DoorType 
{ 
    Open = 0,
    Picked = 1,
    PLocked = 2,
    ELocked = 3,
    Sealed = 4,
    Destroyed = 5
}

/// <summary>
/// Struct for the interactivity of a door, and whether it draws guards' attention.
/// </summary>
public struct DoorInteractivity
{ 
    public bool interactivityState { get; private set; }
    public bool suspiciousBool { get; private set; }
    public DoorInteractivity(bool canInteract, bool isSus)
    { 
        interactivityState = canInteract;
        suspiciousBool = isSus;
    }

    public void BoolSet(bool intChange, bool susChange)
    { 
        interactivityState = intChange;
        suspiciousBool = susChange;
    }
}

public class Door : MonoBehaviour
{
    public DoorType doorState;

    public Door()
    {
        doorState = DoorType.Open;    
    }

    public DoorType GetState()
    { 
        return doorState;
    }
}
