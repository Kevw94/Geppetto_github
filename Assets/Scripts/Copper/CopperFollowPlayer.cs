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

    [Header("Sounds")]
    public AudioSource bark;
    public AudioSource breath;

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
            PlayBarkAnimation();
            LookAtPlayer(force: true);
            return;
        }

        FollowPlayer();
        LookAtPlayer();
        PlayBreathLoop();
    }

    void UpdateCopperTarget()
    {
        if (copperTarget == null || player == null) return;

        if (NavMesh.SamplePosition(player.position, out var hit, 5f, NavMesh.AllAreas))
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
            dogAnimator.SetInteger("ActionType_int", 0); // idle
        }
    }

    void PlayBarkAnimation()
    {
        dogAnimator.SetInteger("ActionType_int", 1);
    }

    void PlayBark()
    {
            bark.Play();
    }

    void PlayBreathLoop()
    {
        if (breath == null) return;

        if (dogAnimator.GetInteger("ActionType_int") != 1)
        {
            if (!breath.isPlaying)
                breath.Play();
        }
        else
        {
            if (breath.isPlaying)
                breath.Stop();
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
}
