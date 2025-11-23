using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CopperBehaviour : MonoBehaviour
{
    [Header("References")]
    public Transform player;         // XR Origin
    public Transform foodBag;
    public Animator dogAnimator;
    public NavMeshAgent agent;
    public Transform copperTarget;   // Empty placé sur le NavMesh

    [Header("Distances")]
    public float barkDistance = 8f;
    public float calmDistance = 3f;
    public float followDistance = 2f;
    public float runDistance = 5f;
    public float petDistance = 2f;
    public float stopDistance = 1f;

    [Header("Speeds")]
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;

    [Header("States")]
    public bool hasFood = false;
    public bool isAggressive = false;
    public bool isFetching = false;

    private Transform fetchTarget;
    private float checkInterval = 0.2f;
    private float checkTimer;

    public AudioSource barkSource;
    public AudioClip barkClip;

    void Start()
    {
        agent.updateRotation = false;
        agent.updatePosition = true;

        Debug.Log("Copper Start: agent enabled=" + agent.enabled + " position=" + transform.position);
        Debug.Log("CopperTarget initial pos=" + (copperTarget ? copperTarget.position.ToString() : "NULL"));
    }

    void Update()
    {
            checkTimer -= Time.deltaTime;
            if (checkTimer > 0f) return;
            checkTimer = checkInterval;

            UpdateCopperTarget();

            if (!hasFood)
            {
                FoodCheck();
                AggressionCheck();
                LookAtPlayer();   // <--- IMPORTANT
                return;
            }

            if (isFetching)
            {
                FetchLogic();
                LookAtPlayer();   // <--- IMPORTANT
                return;
            }

            FollowPlayer();
            PetCheck();

            LookAtPlayer();       // <--- TOUJOURS appeler ici

    }

    void UpdateCopperTarget()
    {
        if (copperTarget == null) return;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(player.position, out hit, 5f, NavMesh.AllAreas))
        {
            copperTarget.position = hit.position;
            Debug.Log("CopperTarget updated on NavMesh: " + copperTarget.position);
        }
        else
        {
            Debug.LogWarning("CopperTarget could not sample NavMesh at player position!");
        }
    }

    void FoodCheck()
    {
        float distToFood = Vector3.Distance(transform.position, foodBag.position);
        if (distToFood < calmDistance)
        {
            hasFood = true;
            foodBag.gameObject.SetActive(false);
            StopAggression();
            Debug.Log("Food received. hasFood=" + hasFood);
        }
    }

    void AggressionCheck()
    {
        if (hasFood) return; // plus d'agression après le foodbag

        float distToPlayer = Vector3.Distance(transform.position, player.position);
        if (distToPlayer < barkDistance && !isAggressive)
        {
            StartAggression();
        }
        else if (distToPlayer >= barkDistance && isAggressive)
        {
            StopAggression();
        }

        Debug.Log("AggressionCheck: distToPlayer=" + distToPlayer + " isAggressive=" + isAggressive);
    }

    void StartAggression()
    {
        isAggressive = true;
        dogAnimator.SetBool("AttackReady_b", true);
        dogAnimator.SetInteger("ActionType_int", 1);
        Debug.Log("StartAggression called");
    }

    void StopAggression()
    {
        isAggressive = false;
        dogAnimator.SetBool("AttackReady_b", false);
        dogAnimator.SetInteger("ActionType_int", 0);
        Debug.Log("StopAggression called");
    }

    void FollowPlayer()
    {
        if (copperTarget == null || !agent.isOnNavMesh) return;

        float dist = Vector3.Distance(transform.position, copperTarget.position);

        // ⛔ Si Copper est trop proche du joueur → STOP
        if (dist < stopDistance)
        {
            agent.ResetPath();
            dogAnimator.SetFloat("Movement_f", 0f);
            return;
        }

        // Ajuste vitesse selon distance
        agent.speed = (dist > runDistance) ? runSpeed : walkSpeed;

        // Animation marche/course
        float targetAnimSpeed = (dist > runDistance) ? 1f : 0.5f;
        dogAnimator.SetFloat("Movement_f",
            Mathf.Lerp(dogAnimator.GetFloat("Movement_f"), targetAnimSpeed, Time.deltaTime * 3f)
        );

        // Destination
        agent.SetDestination(copperTarget.position);
    }


    void PetCheck()
    {
        float dist = Vector3.Distance(transform.position, player.position);
        if (dist < petDistance && Input.GetKeyDown(KeyCode.E))
        {
            StartCoroutine(PetRoutine());
        }
    }

    IEnumerator PetRoutine()
    {
        agent.SetDestination(transform.position);
        dogAnimator.SetFloat("Movement_f", 0f);
        dogAnimator.SetInteger("ActionType_int", 11);
        yield return new WaitForSeconds(2f);
        dogAnimator.SetInteger("ActionType_int", 0);
        Debug.Log("PetRoutine finished");
    }

    public void ThrowObject(Transform obj)
    {
        fetchTarget = obj;
        isFetching = true;
        Debug.Log("ThrowObject called: target=" + obj.name);
    }

    void FetchLogic()
    {
        if (fetchTarget == null || !agent.isOnNavMesh)
        {
            isFetching = false;
            Debug.LogWarning("FetchLogic stopped: fetchTarget null or agent not on NavMesh");
            return;
        }

        agent.SetDestination(fetchTarget.position);
        dogAnimator.SetFloat("Movement_f", 0.5f);
        float dist = Vector3.Distance(transform.position, fetchTarget.position);

        Debug.Log("FetchLogic: dist=" + dist);

        if (dist < 1f)
        {
            StartCoroutine(FetchPickupRoutine());
        }
    }

    IEnumerator FetchPickupRoutine()
    {
        dogAnimator.SetFloat("Movement_f", 0f);
        dogAnimator.SetInteger("ActionType_int", 4);
        yield return new WaitForSeconds(1f);

        fetchTarget.SetParent(transform);
        fetchTarget.localPosition = new Vector3(0, 0.4f, 0.5f);

        StartCoroutine(ReturnObjectRoutine());
    }

    IEnumerator ReturnObjectRoutine()
    {
        dogAnimator.SetInteger("ActionType_int", 0);

        while (Vector3.Distance(transform.position, copperTarget.position) > followDistance)
        {
            agent.SetDestination(copperTarget.position);
            dogAnimator.SetFloat("Movement_f", 0.5f);
            yield return null;
        }

        dogAnimator.SetFloat("Movement_f", 0f);
        fetchTarget.SetParent(null);
        fetchTarget.position = player.position + player.forward * 1f;

        fetchTarget = null;
        isFetching = false;
        Debug.Log("ReturnObjectRoutine finished");
    }

    void LookAtPlayer()
    {
        if (player == null) return;

        Vector3 dir = player.position - transform.position;
        dir.y = 0f;

        if (dir.sqrMagnitude < 0.01f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 5f);
    }

    public void PlayBark()
    {
        if (barkSource != null && barkClip != null)
            barkSource.PlayOneShot(barkClip);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, barkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, calmDistance);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, followDistance);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, runDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, petDistance);
    }
}
