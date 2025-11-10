using UnityEngine;

public class CopperBehaviour : MonoBehaviour
{
    [Header("References")]
    public Transform player; // Référence au Player-Hailey
    public Transform foodBag; // Référence au sac de nourriture
    public Animator dogAnimator; // Référence à l'Animator du chien

    [Header("Settings")]
    public float barkDistance = 8f; // Distance à laquelle le chien commence à aboyer
    public float calmDistance = 3f; // Distance à laquelle le sac le calme

    private bool isAggressive = false;
    private bool isCalm = false;
    private float checkInterval = 0.3f; // Moins fréquent pour l’optimisation
    private float checkTimer;

    void Update()
    {
        checkTimer -= Time.deltaTime;
        if (checkTimer > 0f) return;
        checkTimer = checkInterval;

        float distToPlayer = Vector3.Distance(transform.position, player.position);
        float distToFood = Vector3.Distance(transform.position, foodBag.position);

        // Si la nourriture est proche = calme
        if (distToFood < calmDistance)
        {
            if (isAggressive)
            {
                StopAggression();
            }
            return;
        }

        if (distToPlayer < barkDistance && !isAggressive)
        {
            StartAggression();
        }
        else if (distToPlayer >= barkDistance && isAggressive)
        {
            StopAggression();
        }
    }

    void StartAggression()
    {
        isAggressive = true;
        dogAnimator.SetBool("AttackReady_b", true);
        dogAnimator.SetInteger("ActionType_int", 1);
    }

    void StopAggression()
    {
        isAggressive = false;
        dogAnimator.SetBool("AttackReady_b", false);
        dogAnimator.SetInteger("ActionType_int", 0);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, barkDistance);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, calmDistance);
    }
}
