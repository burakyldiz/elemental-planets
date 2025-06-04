using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float detectionRadius = 10f;
    public float attackRange = 2f;
    public float patrolSpeed = 3.5f;
    public float chaseSpeed = 5f;

    private NavMeshAgent agent;
    private Transform player;
    private int currentWaypointIndex = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        PatrolNextWaypoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRadius)
        {
            ChasePlayer();
        }
        else if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            PatrolNextWaypoint();
        }

        if (distanceToPlayer <= attackRange)
        {
            AttackPlayer();
        }
    }

    void PatrolNextWaypoint()
    {
        if (waypoints.Length == 0) return;

        agent.speed = patrolSpeed;
        agent.destination = waypoints[currentWaypointIndex].position;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    void ChasePlayer()
    {
        agent.speed = chaseSpeed;
        agent.destination = player.position;
    }

    void AttackPlayer()
    {
        // Deal damage to the player
    }

    void OnDestroy()
    {
        //QuestManager.Instance?.DefeatEnemy();
    }
}
