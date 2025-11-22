using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class CopperFetch : MonoBehaviour
{
    [Header("References")]
    public NavMeshAgent agent;
    public Animator dogAnimator;
    public Transform player;                // Hailey
    public Transform copperMouth;           // Empty où l'objet sera tenu

    [Header("Settings")]
    public float pickupDistance = 1.2f;
    public float returnDistance = 2f;
    public float walkSpeed = 1.5f;
    public float runSpeed = 3f;

    private Transform fetchTarget;
    private bool isFetching = false;

    public void ThrowObject(Transform obj)
    {
        fetchTarget = obj;
        isFetching = true;

        dogAnimator.SetFloat("Movement_f", 1f);  // run anim
        agent.speed = runSpeed;
    }

    void Update()
    {
        if (!isFetching || fetchTarget == null) return;

        float dist = Vector3.Distance(transform.position, fetchTarget.position);

        // Se dirige vers l’objet
        agent.SetDestination(fetchTarget.position);

        // Quand Copper atteint l'objet → il le ramasse
        if (dist < pickupDistance)
        {
            StartCoroutine(PickupRoutine());
        }
    }

    IEnumerator PickupRoutine()
    {
        isFetching = false;

        // Stop + animation de ramassage
        agent.ResetPath();
        dogAnimator.SetFloat("Movement_f", 0f);
        dogAnimator.SetInteger("ActionType_int", 4); // pickup animation (mettre à ton index)

        yield return new WaitForSeconds(1f);

        // Ramasse l’objet
        fetchTarget.SetParent(copperMouth);
        fetchTarget.localPosition = Vector3.zero;

        // Retourne au joueur
        StartCoroutine(ReturnRoutine());
    }

    IEnumerator ReturnRoutine()
    {
        dogAnimator.SetInteger("ActionType_int", 0); // idle
        dogAnimator.SetFloat("Movement_f", 0.5f);     // walk
        agent.speed = walkSpeed;

        while (Vector3.Distance(transform.position, player.position) > returnDistance)
        {
            agent.SetDestination(player.position);
            yield return null;
        }

        // Arrivé → il lâche l'objet
        agent.ResetPath();
        dogAnimator.SetFloat("Movement_f", 0f);

        fetchTarget.SetParent(null);
        fetchTarget.position = player.position + player.forward * 1f;

        fetchTarget = null;
    }
}
