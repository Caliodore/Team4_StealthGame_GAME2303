using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Build.Content;
using Cali;
using Unity.VisualScripting;

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

    private LevelManager levelManager;
    private GuardManager guardManager;
    [SerializeField] DirectorOptions directorOptions;
    [SerializeField] PlayerStats playerStats;

    private Dictionary<PlayerLogic, PlayerStats> playerDictionary = new Dictionary<PlayerLogic, PlayerStats>();

    private void Awake()
    {
        PlayerLogic[] players = FindObjectsByType<PlayerLogic>(FindObjectsSortMode.None);
        foreach (PlayerLogic player in players)
        {
            //playerDictionary.Add(player, player.playerStats);
        }

        levelManager = FindFirstObjectByType<LevelManager>(); 
        guardManager = FindFirstObjectByType<GuardManager>();
    }

    public void StageDeterminant()  //Call when alertnessL gets updated (when IncreaseAlertnessLevel() is called) 
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

    public void NearestGuardsToPosition(Vector3 position) // The player/item that invokes this needs to pass their position
    {
        int numberOfGuards = guardManager.CurrentlyActiveGuards;

        switch (alertness)
        {
            case Alertness.Stage1:
                guardManager.GetNearestGuards(1, position);
                break;
            case Alertness.Stage2:
                guardManager.GetNearestGuards(2, position);
                break;
            case Alertness.Alarm:
                guardManager.GetNearestGuards(3, position);
                break;
            case Alertness.Lockdown:
                guardManager.GetNearestGuards(4, position);
                break;
        }
    }

    private void SpecificGuardsToPosition()
    {
        /*GameObject guardWithMaxHealth = FindFirstObjectByType<GuardLogic>().health = 100;

        if (levelManager.foundJewel)
        {
            guardManager.SendSpecificGuards(guardWithMaxHealth, levelManager.GetJewelTransform());
        }*/

        if (alertness == Alertness.Stage2 && playerStats.health == 100)
        {
            
        }
    }


    public void GuardAmountRegulator() //Call when alertnessL gets updated (when IncreaseAlertnessLevel() is called)
    {
        int numCurrentGuards = guardManager.CurrentlyActiveGuards;
        int initialAddValue = GuardsToAdd();
        int actualNumGuardsToAdd;

        switch (alertness)
        {
            case Alertness.Stage1:
                actualNumGuardsToAdd = GuardsToAdd();
                if ((numCurrentGuards + actualNumGuardsToAdd) <= directorOptions.stage1MaxGuards)
                {
                    //guardManager.AddGuards(guardsToAdd);
                    //Problem A: I think Cali wants me to use the UpdateGuardRefs for this, but I am not sure how.

                    //Maybe...
                    /*for (int i = 0; i < actualNumGuardsToAdd; i++)
                    {
                        GameObject guardToAdd = FindFirstObjectByType<GuardLogic>().gameObject;
                        guardManager.UpdateGuardRefs(guardToAdd, true);
                    }*/

                    // Please let me know if the above for loop would be correct, close, or completely wrong
                }
                break;
            case Alertness.Stage2:
                actualNumGuardsToAdd = GuardsToAdd() * 2;
                if ((numCurrentGuards + actualNumGuardsToAdd) <= (directorOptions.stage2MaxGuards))
                {
                    //guardManager.AddGuards(guardsToAdd);
                    //Problem A
                }
                break;
            case Alertness.Alarm:
                actualNumGuardsToAdd = GuardsToAdd() * 3;
                if ((numCurrentGuards + actualNumGuardsToAdd) <= (directorOptions.alarmMaxGuards))
                {
                    //guardManager.AddGuards(guardsToAdd);
                    //Problem A
                }
                break;
            case Alertness.Lockdown:
                actualNumGuardsToAdd = GuardsToAdd() * 4;
                if ((numCurrentGuards + actualNumGuardsToAdd) <= (directorOptions.lockdownMaxGuards))
                {
                    //guardManager.AddGuards(guardsToAdd);
                    //Problem A
                }
                break;
        }
    }

    private int GuardsToAdd()
    {
        int numPlayersAlive = FindObjectsByType<PlayerLogic>(FindObjectsSortMode.None).Length;
        int numGuardsToAdd = 0;

        switch (numPlayersAlive)
        {
            case 1:
                numGuardsToAdd = 1;
                break;
            case 2:
                numGuardsToAdd = 2;
                break;
            case 3:
                numGuardsToAdd = 3;
                break;
            case >= 4:
                numGuardsToAdd = 4;
                break;
        }

        return numGuardsToAdd;
    }

}
