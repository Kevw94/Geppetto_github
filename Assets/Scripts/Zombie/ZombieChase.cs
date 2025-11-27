using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public class ZombieChase : MonoBehaviour
{
    [Header("Target and Ranges")]
    public Transform player;
    public float detectionRange = 15f;
    public float attackRange = 2f;

    [Header("Behavior Parameters")]
    public float lookSpeed = 5f;
    public float attackCooldown = 1.5f;

    [Header("Wandering")]
    public float wanderRadius = 6f;
    public float wanderInterval = 3f;

    // Private variables
    private float nextWanderTime = 0f;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttacking = false;
    private float lastAttackTime = -999f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null) player = mainCam.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        // Player detected
        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);

            // ATTACK
            if (distance <= attackRange)
            {
                agent.isStopped = true;
                FacePlayer();

                if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
                {
                    isAttacking = true;
                    lastAttackTime = Time.time;

                    animator.SetBool("isWalking", false);
                    animator.SetTrigger("attack");

                    Debug.Log("Zombie attack animation triggered");
                }
            }
            else
            {
                // CHASE
                agent.isStopped = false;
                animator.SetBool("isWalking", true);
                isAttacking = false;
            }
        }
        else
        {
            // WANDER
            if (Time.time >= nextWanderTime)
            {
                Vector3 newPos = RandomNavmeshLocation(wanderRadius);
                agent.SetDestination(newPos);

                nextWanderTime = Time.time + wanderInterval + Random.Range(-1f, 1f);
            }

            agent.isStopped = false;
            animator.SetBool("isWalking", true);
            isAttacking = false;
        }
    }

    void FacePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookSpeed);
    }

    // Called by Animation Event from zombie attack animation
    public void DealDamage()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            // First search on player
            HaileyHealth hailey = player.GetComponent<HaileyHealth>();

            // If not found, search in parents (VR case)
            if (hailey == null)
                hailey = player.GetComponentInParent<HaileyHealth>();

            if (hailey != null)
            {
                hailey.TakeDamage(10f);
                Debug.Log("🧟 Zombie hit Hailey !");
            }
            else
            {
                Debug.LogWarning("⚠ DealDamage: HaileyHealth NOT found on player or its parents!");
            }
        }

        isAttacking = false;
    }


    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;

        NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, radius, NavMesh.AllAreas);

        return hit.position;
    }
}
