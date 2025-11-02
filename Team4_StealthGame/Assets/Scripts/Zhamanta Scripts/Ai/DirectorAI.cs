using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEditor.Build.Content;
using Cali;
using Unity.VisualScripting;
using UnityEngine.Events;
using Zhamanta;

// This script will trigger/call events from the LevelManager and the GuardManager depending on the alartness level and other minor events
// No other script should call anything from this script
public class DirectorAI : MonoBehaviour
{
    [SerializeField] float alertnessRelaxationInterval = 7;
    float relaxAlertnessElapsedTime = 0;
    [SerializeField] bool firstLockdownDone = false;
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
    private Zhamanta.GlobalUI globalUI;

    [SerializeField] DirectorOptions directorOptions;
    [SerializeField] PlayerStats playerStats;

    private Dictionary<PlayerLogic, PlayerStats> playerDictionary = new Dictionary<PlayerLogic, PlayerStats>(); //Problem A: I need a similar dictionary for guards (each guard paired with its stats).  I think it can be created in the GuardManager with a public get and private set

    public UnityEvent OnAlertnessStageChange;

    private bool alarmOn;

    private void Awake()
    {
        PlayerLogic[] players = FindObjectsByType<PlayerLogic>(FindObjectsSortMode.None);
        foreach (PlayerLogic player in players)
        {
            //playerDictionary.Add(player, player.playerStats); Problem E: PlayerLogic needs a serializable field that fetches its correcponding PlayerStats SO
        }

        levelManager = FindFirstObjectByType<LevelManager>(); 
        guardManager = FindFirstObjectByType<GuardManager>();
        globalUI = FindFirstObjectByType<Zhamanta.GlobalUI>();
    }

    private void Start()
    {
        alarmOn = false;
        StageDeterminant();
        StartCoroutine(CheckForSpecificBehavior());
    }

    public void StageDeterminant()  //Call when alertnessL gets updated (when UpdateAlertnessLevel() is called) 
    {
        var alertnessL = AlertnessLevel.alertnessL;
        if (alertnessL >= 0 && alertnessL < 25)
        {
            alertness = Alertness.Stage1;
            OnAlertnessStageChange.Invoke();
        }
        else if (alertnessL >= 25 && alertnessL < 50)
        {
            alertness = Alertness.Stage2;
            OnAlertnessStageChange.Invoke();
        }
        else if (alertnessL >= 50 && alertnessL < 75)
        {
            alertness = Alertness.Alarm;
            OnAlertnessStageChange.Invoke();
        }
        else if (alertnessL >= 75 && alertnessL < 100)
        {
            alertness = Alertness.Lockdown;
            OnAlertnessStageChange.Invoke();
            StartCoroutine(LockdownTimer());
            firstLockdownDone = true;
        }
        else if (alertnessL < 0 || alertnessL >= 100)
        { 
            print($"Alertness is out of range. Current Value: {alertnessL}");    
        }
    }

    IEnumerator LockdownTimer()
    {
        levelManager.LockdownHandler(); 

        float playersHealthTotal = 0;

        foreach (KeyValuePair<PlayerLogic, PlayerStats> pair in playerDictionary)
        {
            PlayerLogic player = pair.Key;
            PlayerStats playerStats = pair.Value;

            playersHealthTotal += playerStats.health;
        }

        float playersHealthAverage = playersHealthTotal / playerDictionary.Count;

        if (playersHealthAverage <= 50)
        {
            yield return new WaitForSeconds(directorOptions.lockdownMinTime);
            AlertnessLevel.UpdateAlertnessLevel(-26);
            levelManager.LockdownHandler(); //Lockdown deactivate
        }
        else if (playersHealthAverage > 50)
        {
            yield return new WaitForSeconds(directorOptions.lockdownMaxTime);
            AlertnessLevel.UpdateAlertnessLevel(-26);
            levelManager.LockdownHandler(); //Lockdown deactivate
        }
    }

    private void Update()
    {
        RelaxAlertness();
        ActivateAlarmUI();
    }

    public void ActivateAlarmUI() 
    {
        if (alertness == Alertness.Alarm)
        {
            globalUI.FlashingRed();
        }
    }

    public void ActivateAlarm() //Called when invoking OnAlertnessStageChange
    {
        if (alertness == Alertness.Alarm)
        {
            alarmOn = true;
            globalUI.AlarmSwitch(alarmOn);
        }
        else
        {
            alarmOn = false;
        }
    }

    public void TurnLightsOff() //Called when invoking OnAlertnessStageChange
    {
        if (alertness == Alertness.Lockdown)
        {
            globalUI.TurnLightsOff();
        }
    }

    public void RelaxAlertness()
    {
        if (guardManager.UnawareOfPlayers)
        {
            while (relaxAlertnessElapsedTime != alertnessRelaxationInterval)
            {
                relaxAlertnessElapsedTime += Time.deltaTime;
            }
            if (relaxAlertnessElapsedTime == alertnessRelaxationInterval)
            {
                AlertnessLevel.UpdateAlertnessLevel(-1);
                relaxAlertnessElapsedTime = 0;
            }
        }
    }

    public void ChangeGuardSpeeds() //Called when invoking OnAlertnessStageChange
    {
        if (guardManager.SearchingForPlayers)
        {
            if (alertness == Alertness.Stage1 || alertness == Alertness.Stage2)
            {
                //Problem A
                //Iterate through the guard dictionary to change their speed to the GuardStats SO original speed
            }
            else if (alertness == Alertness.Alarm)
            {
                //Problem A
                //guardsSpeed *= directorOptions.guardSpeedAlarmMultiplier;
            }
            else if (alertness == Alertness.Lockdown)
            {
                //Problem A
                //guardsSpeed *= directorOptions.guardSpeedLockdownMultiplier;
            }
        }
    }

    public void NearestGuardsToPosition(Vector3 position) // The player/item that calls this needs to pass their position.
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

    private void SpecificGuardsToPositions() //Called every now and then through a coroutine that is started in Start()
    {
        // Currently, no way of accessing guards' stats (guards's health in this case), lack of GuardStats SO (Problem A)

        /*GameObject guardWithMaxHealth = FindFirstObjectByType<GuardLogic>().health = 100;

        if (levelManager.jewelStolen)
        {
            guardManager.SendSpecificGuards(guardWithMaxHealth, levelManager.jewelPosition);
        }*/

        if (alertness == Alertness.Stage2)
        {
            foreach (KeyValuePair<PlayerLogic, PlayerStats> pair in playerDictionary)
            {
                PlayerLogic player = pair.Key;
                PlayerStats playerStats = pair.Value;

                if (alertness == Alertness.Stage2 && playerStats.health == 100)
                {
                    //Also send a few full health guards to player, but I need to access guard health somehow (Problem A)
                }
            }
        }
    }

    IEnumerator CheckForSpecificBehavior()
    {
        while (true)
        {
            yield return new WaitForSeconds(20f);
            SpecificGuardsToPositions();
        }
    }


    public void GuardAmountRegulator() //Call when alertnessL gets updated (when UpdateAlertnessLevel() is called).  Another option is to simply call it when invoking OnAlertnessStageChange  <<<Let me know which one/your thoughts
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
                    guardManager.SpawnGuards(actualNumGuardsToAdd);
                }
                break;
            case Alertness.Stage2:
                actualNumGuardsToAdd = GuardsToAdd() * 2;
                if ((numCurrentGuards + actualNumGuardsToAdd) <= (directorOptions.stage2MaxGuards))
                {
                    guardManager.SpawnGuards(actualNumGuardsToAdd);
                }
                break;
            case Alertness.Alarm:
                actualNumGuardsToAdd = GuardsToAdd() * 3;
                if ((numCurrentGuards + actualNumGuardsToAdd) <= (directorOptions.alarmMaxGuards))
                {
                    guardManager.SpawnGuards(actualNumGuardsToAdd);
                }
                break;
            case Alertness.Lockdown:
                actualNumGuardsToAdd = GuardsToAdd() * 4;
                if ((numCurrentGuards + actualNumGuardsToAdd) <= (directorOptions.lockdownMaxGuards))
                {
                    guardManager.SpawnGuards(actualNumGuardsToAdd);
                }
                break;
        }
    }

    private int GuardsToAdd()
    {
        int numPlayersAlive = playerDictionary.Count;
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

    public void RemovePlayer(PlayerLogic playerLogic) //Call through event (OnPlayerDeath or something similar)
    {
        playerDictionary.Remove(playerLogic);
    }
}
