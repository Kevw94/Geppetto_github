using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;
using MikeNspired.XRIStarterKit;
using UnityEngine.SceneManagement;

public class Zombies : MonoBehaviour, IEnemy
{
    [Header("Menu Logic")]
    [Tooltip("Si ce zombie est utilisé dans le menu.")]
    public bool isMenu = false;

    [Header("References")]
    public EnemyHealth enemyHealth;
    public NPCSoundController soundController;
    public Renderer dissolveRenderer;
    public Animator animator;
    public NavMeshAgent agent;

    [Header("Player / Target")]
    public Transform player;

    [Header("Detection & Attack")]
    public float detectionRange = 15f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    private float lastAttackTime = -999f;

    [Header("Wandering")]
    public float wanderRadius = 6f;
    public float wanderInterval = 3f;
    private float nextWanderTime = 0f;

    [Header("Emergence & Sink")]
    public float emergeDuration = 2f;
    public float sinkDuration = 2f;
    public float sinkDistance = 2f;
    public float startAnimationDelay = 1f;

    [Header("Zombie Behavior")]
    public float screamChance = 0.05f;
    public float hitAnimationChance = 0.1f;

    private bool willScream;
    private bool hasScreamed;
    private bool isAttacking = false;
    private bool isDead = false;
    private bool isEmerging = false;
    private bool isSinking = false;

    private static readonly int StartAnim = Animator.StringToHash("Start");
    private static readonly int Hit = Animator.StringToHash("Hit");
    private static readonly int Scream = Animator.StringToHash("Scream");
    private static readonly int Attack = Animator.StringToHash("attack");
    private static readonly int Walk = Animator.StringToHash("isWalking");
    private static readonly int DissolveAmount = Shader.PropertyToID("_DissolveAmount");

    private void Start()
    {
        if (agent == null) agent = GetComponent<NavMeshAgent>();
        if (animator == null) animator = GetComponent<Animator>();

        if (player == null && !isMenu)
        {
            Camera cam = Camera.main;
            if (cam) player = cam.transform;
        }

        if (enemyHealth)
        {
            enemyHealth.OnTakeDamage += _ => soundController.PlayImpact();
            enemyHealth.OnTakeDamage += OnEnemyDamage;
        }

        willScream = UnityEngine.Random.value <= screamChance;

        BeginEmergence();
    }

    private void Update()
    {
        if (isEmerging || isSinking || isDead) return;

        // MENU ZOMBIE ⇒ ne bouge pas, ne chasse pas
        if (isMenu)
        {
            animator.SetBool(Walk, false);
            return;
        }

        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);

        // DETECT PLAYER
        if (dist <= detectionRange)
        {
            ChasePlayer(dist);
        }
        else
        {
            Wander();
        }
    }

    void BeginEmergence()
    {
        isEmerging = true;
        agent.enabled = false;

        Vector3 below = transform.position;
        below.y -= sinkDistance;
        transform.position = below;

        StartCoroutine(EmergeRoutine());
    }

    IEnumerator EmergeRoutine()
    {
        Vector3 start = transform.position;
        Vector3 end = new(start.x, start.y + sinkDistance, start.z);

        float time = 0f;
        soundController.PlaySpawn();

        while (time < emergeDuration)
        {
            float t = time / emergeDuration;
            float ease = t * (2f - t);
            transform.position = Vector3.Lerp(start, end, ease);

            if (time > startAnimationDelay)
                animator.SetBool(StartAnim, true);

            time += Time.deltaTime;
            yield return null;
        }

        transform.position = end;

        agent.enabled = !isMenu;
        isEmerging = false;
    }

    void ChasePlayer(float distance)
    {
        if (agent.isStopped) agent.isStopped = false;
        agent.SetDestination(player.position);
        animator.SetBool(Walk, true);

        // SCREAM
        if (willScream && !hasScreamed && distance < detectionRange * 0.7f)
        {
            animator.SetTrigger(Scream);
            soundController.PlayScream();
            hasScreamed = true;
        }

        // ATTACK
        if (distance <= attackRange)
        {
            TryAttack();
        }
    }

    void Wander()
    {
        if (Time.time >= nextWanderTime)
        {
            Vector3 newPos = RandomNavmeshLocation(wanderRadius);
            agent.SetDestination(newPos);

            nextWanderTime = Time.time + wanderInterval + UnityEngine.Random.Range(-1f, 1f);
        }

        animator.SetBool(Walk, true);
    }

    Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 random = UnityEngine.Random.insideUnitSphere * radius + transform.position;
        NavMesh.SamplePosition(random, out NavMeshHit hit, radius, NavMesh.AllAreas);
        return hit.position;
    }

    void TryAttack()
    {
        agent.isStopped = true;
        FacePlayer();

        if (!isAttacking && Time.time >= lastAttackTime + attackCooldown)
        {
            isAttacking = true;
            lastAttackTime = Time.time;

            animator.SetTrigger(Attack);
            animator.SetBool(Walk, false);
        }
    }

    // Animation Event
    public void DealDamage()
    {
        if (player == null) return;

        float dist = Vector3.Distance(transform.position, player.position);
        if (dist <= attackRange + 0.1f)
        {
            HaileyHealth h = player.GetComponent<HaileyHealth>();
            if (h == null) h = player.GetComponentInParent<HaileyHealth>();

            if (h != null)
                h.TakeDamage(10f);
        }

        isAttacking = false;
    }

    void FacePlayer()
    {
        Vector3 dir = player.position - transform.position;
        dir.y = 0;
        if (dir.sqrMagnitude > 0.01f)
        {
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, 8f * Time.deltaTime);
        }
    }

    void OnEnemyDamage(float dmg)
    {
        if (isDead) return;

        if (UnityEngine.Random.value <= hitAnimationChance)
            animator.SetTrigger(Hit);
    }

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        soundController.PlayDeath();
        soundController.SetRandomVocalEnabled(false);

        animator.SetTrigger(UnityEngine.Random.value > 0.5f ? "Death1" : "Death2");

        if (!isMenu) agent.isStopped = true;

        StartCoroutine(SinkRoutine());
    }

    IEnumerator SinkRoutine()
    {
        yield return new WaitForSeconds(3f);

        isSinking = true;
        agent.enabled = false;

        Vector3 start = transform.position;
        Vector3 end = new Vector3(start.x, start.y - sinkDistance, start.z);

        float t = 0f;
        while (t < sinkDuration)
        {
            transform.position = Vector3.Lerp(start, end, t / sinkDuration);
            t += Time.deltaTime;
            yield return null;
        }

        if (isMenu)
        {
            Debug.Log("Zombie mort dans le MENU → chargement scène City");
            SceneManager.LoadScene("City", LoadSceneMode.Single);
            yield break;
        }

        StartCoroutine(FadeAndDestroy());
    }

    IEnumerator FadeAndDestroy()
    {
        float duration = 2f;
        float time = 0f;
        Material mat = dissolveRenderer.material;

        while (time < duration)
        {
            float v = Mathf.Lerp(0, 1, time / duration);
            mat.SetFloat(DissolveAmount, v);
            time += Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }

    // MENU animation event
    public void PlayScreamEvent()
    {
        if (isDead) return;

        Debug.Log("Zombie Menu Scream Event Triggered!");
        soundController.PlayScream();
    }
}
