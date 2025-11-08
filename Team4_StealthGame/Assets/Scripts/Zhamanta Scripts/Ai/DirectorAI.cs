using Cali;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

// This script will trigger/call events from the LevelManager and the GuardManager depending on the alartness level and other minor events
// No other script should call anything from this script

namespace Zhamanta
{
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

        private List<Player_Health> playerHealths = new List<Player_Health>();

        public UnityEvent OnAlertnessStageChange;

        private bool alarmOn;

        private void Awake()
        {
            Player_Health[] players = FindObjectsByType<Player_Health>(FindObjectsSortMode.None);
            playerHealths = new List<Player_Health>(players);

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

            foreach (Player_Health ph in playerHealths)
            {
                playersHealthTotal += ph.health;
            }

            float playersHealthAverage = playersHealthTotal / playerHealths.Count;

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
            NavMeshAgent[] guards = FindObjectsByType<NavMeshAgent>(FindObjectsSortMode.None);

            if (guardManager.SearchingForPlayers)
            {
                if (alertness == Alertness.Stage1 || alertness == Alertness.Stage2)
                {
                    foreach (NavMeshAgent agent in guards)
                    {
                        agent.speed = 3.5f;
                    }
                }
                else if (alertness == Alertness.Alarm)
                {
                    foreach (NavMeshAgent agent in guards)
                    {
                        agent.speed *= directorOptions.guardSpeedAlarmMultiplier;
                    }
                }
                else if (alertness == Alertness.Lockdown)
                {
                    foreach (NavMeshAgent agent in guards)
                    {
                        agent.speed *= directorOptions.guardSpeedLockdownMultiplier;
                    }
                }
            }
        }

        public void NearestGuardsToPosition(Vector3 position) // CALL THIS.  The player/item that calls this needs to pass their position.
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

        private void SpecificGuardsToPositions()
        {
            if (alertness == Alertness.Stage2)
            {
                foreach (Player_Health ph in playerHealths)
                {

                    if (alertness == Alertness.Stage2 && ph.health == 100)
                    {
                        guardManager.GetNearestGuards(1, ph.gameObject.transform.position);
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


        public void GuardAmountRegulator() 
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
            int numPlayersAlive = playerHealths.Count;
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

        public void RemovePlayerFromList(Player_Health playerHealth) //CALL THIS through event (OnPlayerDeath or something similar)
        {
            playerHealths.Remove(playerHealth);
        }
    }
}

