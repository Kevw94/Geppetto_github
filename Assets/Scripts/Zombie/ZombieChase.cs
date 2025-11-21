using UnityEngine;
using UnityEngine.AI;
using System.Collections;

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
    private float nextWanderTime = 0f;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttacking = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (player == null)
        {
            Camera mainCam = Camera.main;
            if (mainCam != null)
                player = mainCam.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);
        // - CHASE & ATTACK
        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);

            if (distance <= attackRange)
            {
                // - ATTACK
                agent.isStopped = true;
                FacePlayer();

                if (!isAttacking)
                    StartCoroutine(AttackRoutine());
            }
            else
            {
                // - CHASE
                if (isAttacking)
                {
                    StopCoroutine(nameof(AttackRoutine));
                    isAttacking = false;
                }

                agent.isStopped = false;
                animator.ResetTrigger("attack");
                animator.SetBool("isWalking", true);
            }
        }
        else
        {
            // - WANDER

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

    private IEnumerator AttackRoutine()
    {
        isAttacking = true;
        animator.SetBool("isWalking", false);
        animator.SetTrigger("attack");

        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
    }

    public void DealDamage()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            Debug.Log("🧟 Zombie hit the player!");
            // player.GetComponent<PlayerHealth>()?.TakeDamage(10);
        }
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;

        randomDirection += transform.forward * Random.Range(-radius, radius);

        randomDirection += transform.position;

        NavMeshHit hit;

        NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas);

        return hit.position;
    }
}
