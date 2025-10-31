using UnityEngine;
using Cali;

public class GuardSpawnPoint : MonoBehaviour
{
    [Header("Guard Refs")]
    [SerializeField] GameObject guardPrefab;
    [SerializeField] GuardManager guardManager;

    [Header("Individualized Refs")]
    public int spawnPointIndex;

    private void Awake()
    {
        guardManager = FindAnyObjectByType<GuardManager>();
    }

    public void SpawnGuard()
    { 
        Instantiate(guardPrefab);
    }
}
