using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthSO", menuName = "Scriptable Objects/HealthSO")]
public class HealthSO : ScriptableObject
{
    public float maxHealth;
    public float minHealth = 0;
}
