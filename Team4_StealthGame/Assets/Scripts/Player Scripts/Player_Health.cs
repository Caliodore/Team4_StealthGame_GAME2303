using UnityEditor;
using UnityEngine;


/****************** Summary of Player Health ******************/

// This script should handle everything that is related to the player health
// Examples included below such as:
// Taking damage from a guard
// Healing from a healable item as discussed in the notes (toDo_10_25)

/*****************************************************************/


public class Player_Health : MonoBehaviour
{
    // healthSO reference
    [SerializeField] HealthSO playerHealth;

    // health variables
    public float health;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = playerHealth.maxHealth;
    }


    /// <summary>
    /// Take damage to the player(s) health
    /// </summary>
    /// <param name="damage">float damage to player.</param>
    public void TakeDamage(float damage) 
    {
        health -= Mathf.Clamp(health - damage, playerHealth.minHealth, playerHealth.maxHealth);

        if (health == playerHealth.minHealth)
        {
            // die function here if implemented
        }
    }

    /// <summary>
    /// Player using healable to heal health <br/>
    /// Should be called in the necessary healable script of an healable type item
    /// </summary>
    /// <param name="amount">float amount to heal player.</param>
    public void HealingHealth(float amount)
    {
        health += amount;
    }
}
