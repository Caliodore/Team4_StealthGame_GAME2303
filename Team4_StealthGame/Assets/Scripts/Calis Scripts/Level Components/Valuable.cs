using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class Valuable : MonoBehaviour
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

    private int valPoints;
    public int ValPoints
    { 
        get { return  valPoints; }    
    }

    public Valuable(ValuableTyping valTypeIn)
    {
        valType = valTypeIn;
        switch((int)valType)
        { 
            case(0):
                valTypeName = "Coin";
                valPoints = 1;
                break;
            case(1):
                valTypeName = "Loot";
                valPoints = 2;
                break;
            case(2):
                valTypeName = "Target";
                valPoints = 10;
                break;
        }
    }

    public Valuable()
    { 
        valType = ValuableTyping.Coin;    
    }
}