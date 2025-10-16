using Unity.VisualScripting;
using UnityEngine;

public enum ExitType 
{ 
    Constant = 0,
    Stealth = 1,
    Loud = 2,
    Conditional = 3
}

public class Exit : MonoBehaviour
{
    public ExitType exitTypeState { get; private set; }

    public Exit()
    {
        exitTypeState = ExitType.Constant;    
    }

    public ExitType GetState()
    { 
        return exitTypeState;
    }
}
