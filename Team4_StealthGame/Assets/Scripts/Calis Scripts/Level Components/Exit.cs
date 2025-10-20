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
    private ExitType exitTypeState;

    public Exit()
    {
        exitTypeState = ExitType.Constant;    
    }

    public ExitType ExitTypeState
    { 
        get {return exitTypeState; }
    }

    public void SetExitType(ExitType inputState)
    { 
        exitTypeState = inputState;
    }
}
