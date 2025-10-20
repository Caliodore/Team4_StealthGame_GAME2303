using UnityEngine;

public class PlayerLogic : PlayerBase
{
    PlayerLogic playerInst;

    [Header("Component Refs")]
    [SerializeField] Collider alertGuardsCollider;
    [SerializeField] GameObject restrictedAreaObject;   //Any singular restricted area item should be fine, as long as they all have the correct tag.

    [Header("Internal Vars")]
    public string restrictedAreaTag;

    private void Awake()
    {
        playerInst.IsTrespassing = false;
        restrictedAreaTag = restrictedAreaObject.tag;
    }

    public void TrespassingToggle()
    { 
        playerInst.IsTrespassing = !playerInst.IsTrespassing;
    }

    private void OnTriggerEnter(Collider other)
    {
        string colliderEnterTag = other.tag;
        if(colliderEnterTag == restrictedAreaTag)
        { 
            TrespassingToggle();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        string colliderExitTag = other.tag;
        if(colliderExitTag == restrictedAreaTag)
        { 
            TrespassingToggle();
        }
    }
}
