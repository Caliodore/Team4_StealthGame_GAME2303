using UnityEngine;

/// <summary>
/// A script outlining the basic capabilities of a guard, as well as the necessary references for the guard object.
/// </summary>
public class GuardBase : MonoBehaviour
{
    [SerializeField] public GuardStats guardStatRef;
    [SerializeField] public Transform attachedTransform;
    [SerializeField] public Transform[] patrolPoints;
}
