using UnityEngine;
using UnityEngine.AI;

public class CopperFollowPlayer : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform foodBag;
    public Animator dogAnimator;
    public NavMeshAgent agent;
    public Transform copperTarget;

    [Header("Distances")]
    public float calmDistance = 1f;
    public float followDistance = 2f;
    public float runDistance = 5f;
    public float stopDistance = 1f;

    [Header("Speeds")]
    public float walkSpeed = 2f;
    public float runSpeed = 4f;
    public float acceleration = 5f;
    public float rotationSpeed = 10f;

    [Header("States")]
    public bool hasFood = false;

    [Header("Audio")]
    public AudioSource barkSource;
    public AudioClip barkClip;
    public float barkInterval = 2f;

    private float barkTimer = 0f;

    void Start()
    {
        agent.updateRotation = false;
        agent.acceleration = acceleration;
        agent.angularSpeed = 120f;

        if (copperTarget == null)
            Debug.LogError("CopperTarget not assigned!");
    }

    void Update()
    {
        UpdateCopperTarget();

        if (!hasFood)
        {
            FoodCheck();
            BarkCheck();               // <--- RETOUR DU SYSTÈME ORIGINAL
            LookAtPlayer(force: true);
            return;
        }

        FollowPlayer();
        LookAtPlayer();
    }

    void UpdateCopperTarget()
    {
        if (copperTarget == null || player == null) return;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, 5f, NavMesh.AllAreas))
            copperTarget.position = hit.position;
    }

    void FoodCheck()
    {
        if (Vector3.Distance(transform.position, foodBag.position) < calmDistance)
        {
            hasFood = true;
            foodBag.gameObject.SetActive(false);

            agent.ResetPath();
            dogAnimator.SetFloat("Movement_f", 0f);

            dogAnimator.SetInteger("ActionType_int", 0); // stop barking
        }
    }

    void FollowPlayer()
    {
        if (copperTarget == null || !agent.isOnNavMesh) return;

        Vector3 dir = copperTarget.position - transform.position;
        float dist = dir.magnitude;

        if (dist < stopDistance)
        {
            agent.ResetPath();
            dogAnimator.SetFloat("Movement_f", 0f);
            return;
        }

        float targetSpeed = (dist > runDistance) ? runSpeed : walkSpeed;
        agent.speed = Mathf.Lerp(agent.speed, targetSpeed, Time.deltaTime * acceleration);

        float animSpeed = agent.speed / runSpeed;
        dogAnimator.SetFloat("Movement_f", animSpeed);

        agent.SetDestination(copperTarget.position);
    }

    void LookAtPlayer(bool force = false)
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (force || dir.sqrMagnitude > 0.01f)
        {
            Quaternion targetRot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * rotationSpeed);
        }
    }

    // -------------------------------------------------------
    // --------- 🔵 SYSTÈME D’ABOIEMENT ORIGINAL -------------
    // -------------------------------------------------------

    void BarkCheck()
    {
        barkTimer -= Time.deltaTime;

        if (barkTimer <= 0f)
        {
            PlayBark();

            // Animation d’aboiement EXACTE du script original
            dogAnimator.SetInteger("ActionType_int", 1);

            // Retour au Idle après 1s
            Invoke(nameof(ResetBarkAnimation), 1f);

            barkTimer = barkInterval;
        }
    }

    void PlayBark()
    {
        if (barkSource && barkClip)
            barkSource.PlayOneShot(barkClip);
    }

    void ResetBarkAnimation()
    {
        dogAnimator.SetInteger("ActionType_int", 0);
    }
}
