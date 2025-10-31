using UnityEngine;
using Cali;

public class GuardSpawnPoint : MonoBehaviour
{
    [Header("Guard Refs")]
    [SerializeField] GameObject guardPrefab;
    [SerializeField] GuardManager guardManager;

    [Header("Individualized Refs")]
    public int spawnPointIndex;


    public void SpawnGuard()
    { 
        Instantiate(guardPrefab);
    }
}
