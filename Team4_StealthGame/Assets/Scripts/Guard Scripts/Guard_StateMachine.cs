using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
public enum enemyStates
{
    PATROL, 
    INVESTIGATE,
    PURSUE
}

public class Guard_StateMachine : GuardBase
{
    //variables 
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Transform target;

    Player_Movement player;
    Player_Health playerHealth;
    GameManager gameManager;

    Transform currentPatrolPoint;
    enemyStates states;
    int gotPosition;
     private bool playerIsSeen;
    Vector3 getPlayerPostion;
    int PatrolPointIndex = 0;
    float visionRadius = 0.8f;
    float timeElapsed;
    float playerAttackRange = 1;

    Guard_DetectionSensor sensor;

    [SerializeField] LayerMask environmentLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sensor = FindAnyObjectByType<Guard_DetectionSensor>();  
        player = FindAnyObjectByType<Player_Movement>(); 
        gameManager = FindAnyObjectByType<GameManager>();
        currentPatrolPoint = patrolPoints[0];
        agent.SetDestination(currentPatrolPoint.position);  
    }

    // Update is called once per frame
    void Update()
    {

        switch (states)
        {
            case enemyStates.PATROL:
                Patrol();
                break;
            case enemyStates.INVESTIGATE:
                Investigate();
                break;
            case enemyStates.PURSUE:
                Pursue();
                break;
        }
    }

    public void HeardSomething(bool playerIsMakingSound, bool isMoving, bool onSensor)
    {
        if (playerIsMakingSound && isMoving && onSensor)
        {
            Debug.Log("Actively litsening now");
            states = enemyStates.INVESTIGATE;
        }
        else
            Debug.Log("Player not in SENSOR ");

    }

    public void SawSomething(bool onSensor)
    {
        Vector3 guardForward = transform.forward;
        guardForward.y = 0;
        guardForward.Normalize();
        Vector3 lineToTarget = (target.position - transform.position).normalized;
        float dot = Vector3.Dot(guardForward, lineToTarget);
        if (onSensor)
        {
            Debug.Log("Player in SIGHT sensor");
            gotPosition = 0;

            if (dot > visionRadius)
            {
                RaycastHit hit;
                if (Physics.Raycast(transform.position, lineToTarget, out hit, 75, environmentLayer))
                {
                    Debug.Log("Seeing a WALL");
                    playerIsSeen = false;
                    
                }
                else
                {
                    Debug.Log("Saw the PLAYER");
                    playerIsSeen = true;
                    states = enemyStates.PURSUE;
                    
                    GetPlayerPosition();
                }

            }
            else
            {
                Debug.Log("Player is not seen");
                playerIsSeen = false;
            }
        }
        else
        {
            playerIsSeen = false;
            gotPosition = 0;
        }
    }

    void GetPlayerPosition()
    {
      if(gotPosition == 0)
      {
        getPlayerPostion = target.position;
            gotPosition = 1;
      }
    }

    void Patrol()
    {
        float distance = Vector3.Distance(transform.position, currentPatrolPoint.position);
      
        if (distance < 2)
        {
            PatrolPointIndex++;


            if(PatrolPointIndex >= patrolPoints.Length)
            {
                PatrolPointIndex = 0;
            }

            currentPatrolPoint = patrolPoints[PatrolPointIndex];
            agent.SetDestination(currentPatrolPoint.position);


        }
        else
            agent.SetDestination(currentPatrolPoint.position);

    }

    void Investigate()
    {
        Debug.Log("INVESTIGATE STATE");

        if (sensor.playerInSensor && player.isMakingSound)
        {
            transform.forward = (target.position - transform.position).normalized;

            Debug.Log("ROTATING TO PLAYER...");


        }
        else
        {
            timeElapsed += Time.deltaTime;

            if (timeElapsed > 5)
            {
                Debug.Log("going to PATROL state");
                states = enemyStates.PATROL;
                timeElapsed = 0;
            }
        }



    }

    void Pursue()
    {

        if (playerIsSeen)
        {
            float distance = Vector3.Distance(transform.position, target.position);
            agent.SetDestination(target.position);
            Debug.Log("PURSUING THE PLAYER");

            // going to change some logic here

        }
        else
        {
            Debug.Log("LOST PLAYER GOING BACK");
            float distance2 = Vector3.Distance(transform.position, getPlayerPostion);
            agent.SetDestination(getPlayerPostion);

            if (distance2 < 2.5f)
            {
                states = enemyStates.INVESTIGATE;
            }
        }
    }

    public void TakeDamage(float damage)
    {

    }
}
