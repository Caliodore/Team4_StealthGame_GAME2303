using UnityEngine;

public class Valuable
{
    [SerializeField] public enum ValuableTyping
    { 
        Coin = 0,
        Loot = 1,
        Target = 2,
    }

    ValuableTyping valType;

    public Valuable()
    { 
        valType = 0;
    }
}
