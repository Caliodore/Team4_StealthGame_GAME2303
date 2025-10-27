using UnityEngine;



/// <summary>
///  Item class should be the base class of all items in the game. <br/>
///  This should only include things that the player will have in their inventory. <br/>
///  The reason for this the inventory script uses a list of type Item(s). <br/>
///  List of Items: Guns, Healables, Tools, and more (if added). <br/> <br/>
///  NOTE: If there is any problems please message Carlos
/// </summary>
public class Item : MonoBehaviour
{
    protected Player_Inventory player;
    public virtual void Equip(Player_Inventory p)
    {
        player = p;
    }

    public virtual void Unequip() { }
}
