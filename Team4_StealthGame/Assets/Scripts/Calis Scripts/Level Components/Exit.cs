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
    private int exitInt;

    public int ExitInt
    { 
        get { return (int)exitTypeState; }    
    }

    public ExitType ExitTypeState
    { 
        get {return exitTypeState; }
    }
}
