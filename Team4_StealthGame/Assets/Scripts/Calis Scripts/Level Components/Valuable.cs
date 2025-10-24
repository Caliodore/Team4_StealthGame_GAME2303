using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Valuable
{
    [SerializeField] public enum ValuableTyping
    { 
        Coin = 0,
        Loot = 1,
        Target = 2,
    }

    private string valTypeName;
    public string ValTypeName
    {
        get { return valTypeName; }    
    }

    private ValuableTyping valType;
    public ValuableTyping ValType
    { 
        get { return valType; }    
    }

    public int ValInt
    { 
        get { return (int)valType; }
    }

    public Valuable(ValuableTyping valType)
    {
        this.valType = valType;
        if((int)valType == 2)
        { 
            valTypeName = new string("Target");
        }
    }
}