using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Build.Content;
using Cali;

// This script will trigger/call events from the LevelManager and the GuardManager depending on the alartness level and other minor events
// No other script should call anything from this script
public class DirectorAI : MonoBehaviour
{
    private enum Alertness
    {
        Stage1,
        Stage2,
        Alarm,
        Lockdown
    }
    [SerializeField] private Alertness alertness;
    float alertnessL;

    private LevelManager levelManager;
    private GuardManager guardManager;

    [SerializeField] Transform playerPosition; // Will need revision if multiple levels are implemented

    private void Awake()
    {
        levelManager = FindFirstObjectByType<LevelManager>(); 
        guardManager = FindFirstObjectByType<GuardManager>();
    }

    private void Update()
    {
        StageBehavior();
        GuardsBehavior();
    }

    public void StageDeterminant()
    {
        var alertnessL = AlertnessLevel.alertnessL;
        if (alertnessL >= 0 && alertnessL < 25)
        {
            alertness = Alertness.Stage1;
        }
        else if (alertnessL >= 25 && alertnessL < 50)
        {
            alertness = Alertness.Stage2;
        }
        else if (alertnessL >= 50 && alertnessL < 75)
        {
            alertness = Alertness.Alarm;
        }
        else if (alertnessL >= 75 && alertnessL < 100)
        {
            alertness = Alertness.Lockdown;
        }
        else if (alertnessL < 0 || alertnessL >= 100)
        { 
            print($"Alertness is out of range. Current Value: {alertnessL}");    
        }
    }

    private void StageBehavior()
    {
        switch (alertness)
        {
            case Alertness.Stage1:
                // Guards patrol normally
                break;
            case Alertness.Stage2:
                // Spawns more guards
                break;
            case Alertness.Alarm:
                // Guards actively look for player
                break;
            case Alertness.Lockdown:
                /*if (PlayerHealth.health < 20 && AllDoorsLocked == false)
                {
                    levelManager.LockAllExits(DirectorOptions.lockAllDoorsMinTime());
                }
                else if (PlayerHealth.health >= 20 && AllDoorsLocked == false)
                {
                    levelManager.LockAllExits(DirectorOptions.lockAllDoorsMaxTime());
                }*/
                // Sound Alarm
                break;
        }
    }

    private void GuardsBehavior()
    {
        /*if (PlayerInput.Fire.performed()) // Or perhaps GuardManager.ShotHeard == true;
        {
            GuardManager.ShotHeard = false;
            GuardManager.NearestGuardsToPlaceShotHeard(playerPosition);
            AlertnessLevel.IncreaseAlertnessLevel(10);
        }*/
    }


    // The following could be called through Unity Events/Actions to avoid checking if statements every frame in Update()
    private void GuardAmountRegulator()
    {
        /*if (GuardManager.GuardCount < 5 && LevelManager.JewelHasBeenStolen == true)
        {
            GuardManager.AddGuardsInRoomsAdjacentToPlayer(playerPosition);
        }*/

        // Remove some guards if teammate is dead?
    }

    private void DoorsBehavior()
    {
        // Door behavior
    }

    private void SpawnItem()
    {
        // Spawn healing items if player is too low on health AND there are no healing items left?
    }



}
