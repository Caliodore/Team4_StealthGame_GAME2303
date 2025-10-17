using UnityEngine;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]

public class PlayerStats : ScriptableObject
{
    public float health;
    public float moveSpeed;
    public float weaponDamage;
}
