using Cali;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// The script handling the basis of guard reactions to player actions/changes in environment.
/// There should be a separate script for handling movement/patrolling.
/// </summary>
public class GuardLogic : GuardBase
{
    [SerializeField] GuardManager guardManager;
    public bool isProcessingReaction;
    public UnityEvent guardHearsFailedInteraction;
    public float nonHostileReactionTime = 2f;

    /// <summary>
    /// Method to be called mainly by GuardManager to interrupt patrol/idle state for guards to check noise distractions.
    /// </summary>
    /// <param name="goToHere">Wherever noise in their range was.</param>
    public void ReactToNoise(Vector3 goToHere)
    {
        isProcessingReaction = true;
        StartCoroutine(ReactionTiming(goToHere));
    }

    IEnumerator ReactionTiming(Vector3 noiseLocation)
    {
        yield return new WaitForSeconds(nonHostileReactionTime);
        //MoveToLocation(noiseLocation);
        isProcessingReaction = false;
    }

    private void OnDestroy()
    {
        guardManager.UpdateGuardRefs(gameObject, false);
    }

}
