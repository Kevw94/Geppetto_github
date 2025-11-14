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

        if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);

            // Distance is within attack range, so attack, state == attacking
            if (distance <= attackRange)
            {
                agent.isStopped = true;
                FacePlayer();


                if (!isAttacking)
                    StartCoroutine(AttackRoutine());
            }
            else
            {
                // Outside attack range, so the zomboie chase the player, state == walking
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
            // Player is out of detection range of zombie, state == idle
            agent.isStopped = true;
            animator.SetBool("isWalking", false);
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

    // Implement the attack routine for the zombie because it needs to wait between attacks
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
            // Example --> player.GetComponent<PlayerHealth>()?.TakeDamage(10);
        }
    }
}
