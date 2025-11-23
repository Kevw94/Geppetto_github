using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CopperFetchManager : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Animator dogAnimator;
    public Transform player;          // Ellie
    public Transform copperMouth;     // Empty dans la gueule

    [Header("Settings")]
    public float detectionRadius = 10f;
    public float pickupDistance = 1.2f;
    public float returnDistance = 2f;
    public float releaseDistance = 1.5f;
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;

    private Transform targetObject;
    private bool isFetching = false;
    private bool hasObjectInMouth = false;

    void Update()
    {
        if (!isFetching && targetObject != null && !hasObjectInMouth)
        {
            // Objet prêt à être récupéré
            float dist = Vector3.Distance(transform.position, targetObject.position);
            if (dist > pickupDistance)
                GoToObject();
            else
                StartCoroutine(PickupRoutine());
        }

        if (isFetching && hasObjectInMouth)
        {
            // Retour vers Ellie
            GoToPlayer();
        }
    }

    // ==========================================================
    // Début de la récupération : Copper va vers l'objet
    // ==========================================================
    public void StartFetch(Transform obj)
    {
        if (obj == null) return;

        targetObject = obj;
        isFetching = true;
        hasObjectInMouth = false;

        agent.speed = runSpeed;
        dogAnimator.SetFloat("Movement_f", 1f); // run animation
        agent.isStopped = false;
    }

    void GoToObject()
    {
        if (targetObject == null) return;

        agent.SetDestination(targetObject.position);
        dogAnimator.SetFloat("Movement_f", 1f);
    }

    // ==========================================================
    // Ramassage de l'objet
    // ==========================================================
    IEnumerator PickupRoutine()
    {
        if (targetObject == null) yield break;

        isFetching = false;

        // Stop Copper complètement
        agent.isStopped = true;
        agent.ResetPath();
        dogAnimator.SetFloat("Movement_f", 0f);

        // Animation de ramassage sur place
        dogAnimator.Play("13_Standing_ShakeToy");
        float shakeDuration = dogAnimator.GetCurrentAnimatorStateInfo(0).length;
        yield return new WaitForSeconds(shakeDuration);

        // Attache objet dans la bouche
        targetObject.SetParent(copperMouth);
        targetObject.localPosition = Vector3.zero;
        targetObject.localRotation = Quaternion.identity;

        hasObjectInMouth = true;

        // Reprend le mouvement vers Ellie
        agent.isStopped = false;
        agent.speed = walkSpeed;
        dogAnimator.SetFloat("Movement_f", 0.5f);
    }

    void GoToPlayer()
    {
        if (player == null || targetObject == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        if (dist > returnDistance)
        {
            agent.SetDestination(player.position);
            LookAtPlayer();
            dogAnimator.SetFloat("Movement_f", 0.5f); // marche
        }
        else
        {
            // Arrivé → rendre l'objet
            StartCoroutine(ReleaseObject());
        }
    }

    IEnumerator ReleaseObject()
    {
        agent.ResetPath();
        dogAnimator.SetFloat("Movement_f", 0f);

        // Détache l'objet devant Ellie
        if (targetObject != null)
        {
            targetObject.SetParent(null);
            targetObject.position = player.position + player.forward * 0.5f;
        }

        // Reset
        targetObject = null;
        isFetching = false;
        hasObjectInMouth = false;

        yield return null;
    }

    // ==========================================================
    // Méthode pour détecter collision avec la main
    // ==========================================================
    public void ObjectTouchedHand(Transform obj)
    {
        Debug.Log($"Object {obj.name} touched hand");
        // Copper s'assoit, happy
        dogAnimator.Play("Sit");
    }

    public void ObjectTouchedGround(Transform obj)
    {
        Debug.Log($"Object {obj.name} touched ground");
        // Copper commence la récupération seulement si objet a été touché par la main avant
        if (!isFetching && obj != null)
            StartFetch(obj);
    }


    void LookAtPlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 5f);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
