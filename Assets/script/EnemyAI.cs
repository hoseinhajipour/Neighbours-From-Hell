using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

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
    public Animator animator;

    private float attackTimer;
    public float totalFOV = 90.0f;

    public float Walkspeed = 3.0f;
    public float Runspeed = 5.0f;

    public GameObject loseMenu;

    public int angry = 0;
    private bool FindSabotage = false;
    private GameObject SabotageObject;
    private bool isPlayingAngryAnimation = false;

    private LevelController LevelController_;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        // animator = GetComponent<Animator>();
        patrolWaypoints = Tasks.GetComponentsInChildren<Waypoint>();
        LevelController_ = GameObject.Find("LevelController").GetComponent<LevelController>();
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

        // Check if the target angry value has changed
        if (currentAngryValue != targetAngryValue)
        {
            // Start a coroutine to update the slider value gradually
            StartCoroutine(UpdateAngryValue());
        }
    }

    void Patrol()
    {
        checkSabotages();
        if (FindSabotage == false)
        {
            Waypoint currentWaypoint = patrolWaypoints[currentWaypointIndex];
            agent.SetDestination(currentWaypoint.transform.position);

            float dist = Vector3.Distance(currentWaypoint.transform.position, transform.position);
            if (dist <= 1.5f)
            {
                if (!isPlayingIdleAnimation)
                {
                    animator.SetBool("isWalking", false);
                    StartCoroutine(PlayIdleAnimation());
                }
            }
            else
            {
                animator.SetBool("isWalking", true);
            }
        }
    }


    private bool isPlayingIdleAnimation = false;

    IEnumerator PlayIdleAnimation()
    {
        transform.rotation =
            Quaternion.Euler(0f, patrolWaypoints[currentWaypointIndex].transform.rotation.eulerAngles.y, 0f);
        transform.position = patrolWaypoints[currentWaypointIndex].transform.position;
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
        animator.Play("idle");
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

    public void checkSabotages()
    {
        if (FindSabotage)
        {
            agent.SetDestination(SabotageObject.transform.position);
            float dist = Vector3.Distance(SabotageObject.transform.position, transform.position);
            //  if (agent.remainingDistance <= stoppingDistance)
            if (dist <= 1.5f)
            {
                animator.SetBool("isWalking", false);
                if (!isPlayingAngryAnimation)
                {
                    Debug.Log("what happend ?!");
                    isPlayingAngryAnimation = true;
                    StartCoroutine(PlayAngryAnimation());
                }
            }
            else
            {
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            Vector3 direction = player.position - transform.position;
            float angle = Vector3.Angle(direction, transform.forward);
            //     if (angle < totalFOV) {
            RaycastHit hito;
            if (Physics.Raycast(transform.position, direction, out hito, lookRadius))
            {
                //  Debug.Log( hit.collider.gameObject.name);
                if (hito.collider.tag == "Sabotage")
                {
                    Sabotage sabotage = hito.collider.GetComponent<Sabotage>();
                    if (sabotage.Broke == true)
                    {
                        Debug.Log("FindSabotage");
                        FindSabotage = true;
                        SabotageObject = hito.collider.gameObject;
                    }
                }
            }
            //  }
        }
    }

    public Slider angrySlider;
    public int currentAngryValue = 0;
    public int targetAngryValue = 0;
    private float animationDuration = 2.0f;

    IEnumerator UpdateAngryValue()
    {
        float timeElapsed = 0;
        float startValue = currentAngryValue;
        float endValue = targetAngryValue;
        while (timeElapsed < animationDuration)
        {
            // Calculate the interpolation factor
            float t = timeElapsed / animationDuration;

            // Update the slider value using a smoothed Lerp function
            float value = Mathf.SmoothStep(startValue, endValue, t);
            angrySlider.value = value;

            // Wait for the next frame
            yield return null;

            // Update the time elapsed
            timeElapsed += Time.deltaTime;
        }

        // Set the current angry value to the target value
        currentAngryValue = targetAngryValue;
    }

    public void IncreaseAngryValue(int amount)
    {
        targetAngryValue += amount;
    }

    public void DecreaseAngryValue(int amount)
    {
        targetAngryValue -= amount;
    }

    IEnumerator PlayAngryAnimation()
    {
        agent.isStopped = true;
        animator.Play("angry");
        angry += 10;
        IncreaseAngryValue(SabotageObject.GetComponent<Sabotage>().angryAmount);
        yield return new WaitForSeconds(3.0f);

        animator.SetBool("isFix", true);
        yield return new WaitForSeconds(5.0f);
        animator.SetBool("isFix", false);
        animator.SetBool("isWalking", true);
        agent.isStopped = false;
        SabotageObject.GetComponent<Sabotage>().fix();
        FindSabotage = false;
        SabotageObject = null;
        LevelController_.Increse();
        yield return new WaitForSeconds(1.0f);
        // Reset the flag to false to indicate that the animation is completed
        isPlayingAngryAnimation = false;
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