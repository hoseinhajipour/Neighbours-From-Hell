using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float lookRadius = 10f;
    public float attackRadius = 2f;
    public float timeBetweenAttacks = 1f;

    public GameObject Tasks;
    private Waypoint[] patrolWaypoints;
    private float idleTimer;
    private int currentWaypointIndex = 0;
    private bool isPatrolling = true;
    public float stoppingDistance = 0.1f;

    private NavMeshAgent agent;
    private Transform player;
    private Animator animator;

    private float attackTimer;
    public float totalFOV = 90.0f;

    public float Walkspeed = 3.0f;
    public float Runspeed = 5.0f;

    public GameObject loseMenu;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        patrolWaypoints = Tasks.GetComponentsInChildren<Waypoint>();
    }

    void Update()
    {
        if (CanSeePlayer())
        {
            isPatrolling = false;
            AttackPlayer();
        }
        else if (isPatrolling)
        {
            Patrol();
        }
    }

    void Patrol()
    {
        Waypoint currentWaypoint = patrolWaypoints[currentWaypointIndex];
        agent.SetDestination(currentWaypoint.transform.position);

        if (agent.remainingDistance <= stoppingDistance)
        {
            animator.SetBool("isWalking", false);
            if (!isPlayingIdleAnimation)
            {
                StartCoroutine(PlayIdleAnimation());
            }
        }
        else
        {
            animator.SetBool("isWalking", true);
        }
    }

    private bool isPlayingIdleAnimation = false;

    IEnumerator PlayIdleAnimation()
    {
        // Set the flag to true to indicate that the animation is playing
        isPlayingIdleAnimation = true;
        agent.isStopped = true;
        float waitTime = patrolWaypoints[currentWaypointIndex].waitTime;

        animator.Play(patrolWaypoints[currentWaypointIndex].AnimationName);
        currentWaypointIndex++;
        if (currentWaypointIndex >= patrolWaypoints.Length)
        {
            currentWaypointIndex = 0;
        }

        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isWalking", true);
        agent.isStopped = false;
        // Reset the flag to false to indicate that the animation is completed
        isPlayingIdleAnimation = false;
    }

    void AttackPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= attackRadius)
        {
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
            animator.SetBool("isAttacking", true);

            // Wait for 3 seconds after the attack animation finishes
            StartCoroutine(ShowLoseMenuAfterDelay(3.0f));
        }
        else
        {
            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            animator.SetBool("isAttacking", false);
            Vector3 targetPosition = player.position - transform.forward * 1.0f;
            agent.SetDestination(targetPosition);

            // Increase agent speed when player is seen
            float playerDistance = Vector3.Distance(transform.position, player.position);
            if (playerDistance <= lookRadius)
            {
                agent.speed = Runspeed;
            }
            else
            {
                agent.speed = Walkspeed;
            }
        }
    }

    IEnumerator ShowLoseMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        // show the lose menu
        loseMenu.SetActive(true);
    }

    bool CanSeePlayer()
    {
        Vector3 direction = player.position - transform.position;
        float angle = Vector3.Angle(direction, transform.forward);
        if (angle < totalFOV)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit, lookRadius))
            {
                if (hit.collider.tag == "Player")
                {
                    Debug.Log("Find Player");
                    hit.collider.gameObject.GetComponent<PlayerController>().freezePlayer();
                    return true;
                }
            }
        }

        return false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawWireSphere(transform.position, lookRadius);
        DrawCircle(transform.position, lookRadius, 32);

        float halfFOV = totalFOV / 2.0f;
        Quaternion leftRayRotation = Quaternion.AngleAxis(-halfFOV, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(halfFOV, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * transform.forward;
        Vector3 rightRayDirection = rightRayRotation * transform.forward;
        Gizmos.DrawRay(transform.position, leftRayDirection * lookRadius);
        Gizmos.DrawRay(transform.position, rightRayDirection * lookRadius);
    }


    public int segments = 16;

    private void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angle = 0f;
        float angleDelta = 2 * Mathf.PI / segments;
        Vector3 previousPoint = center + new Vector3(radius, 0f, 0f);
        for (int i = 1; i <= segments; i++)
        {
            Vector3 point = center + new Vector3(radius * Mathf.Cos(angle), 0f, radius * Mathf.Sin(angle));
            Debug.DrawLine(previousPoint, point, Color.red);
            previousPoint = point;
            angle += angleDelta;
        }
    }
}