using UnityEngine;

[CreateAssetMenu(fileName = "GuardStats", menuName = "Scriptable Objects/GuardStats")]
public class GuardStats : ScriptableObject
{
    [SerializeField] float guardMaxHealth = 2;
    [SerializeField] float guardSpeed = 5;

    public float MaxHealth
    {
        get {  return guardMaxHealth; }
    }

    public float GuardSpeed
    { 
        get { return guardSpeed; }    
    }
}
