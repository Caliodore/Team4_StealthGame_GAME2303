using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthSO", menuName = "Scriptable Objects/HealthSO")]
public class HealthSO : ScriptableObject
{
    public float currentHealth;
    public float maxHealth;
    private float minHealth = 0;

    public void RestoreHealth()
    {
        currentHealth = maxHealth;
    }
    
    public void DecreaseHealth(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth -  amount, minHealth, maxHealth);
        Debug.Log("Player damaged!");
    }
}
