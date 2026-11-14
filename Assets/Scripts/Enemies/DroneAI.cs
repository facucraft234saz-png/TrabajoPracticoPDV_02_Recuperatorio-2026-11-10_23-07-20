
using UnityEngine;
using UnityEngine.AI;

public class DroneAI : MonoBehaviour
{
    [Header("Patrullaje")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float waitAtPoint = 2f;

    [Header("Velocidades")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Deteccion y ataque")]
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackCooldown = 2f;

    private NavMeshAgent agent;
    private PlayerHealth target;

    private PlayerInvisibility targetInvisibility;

    private int currentPoint;
    private float waitTimer;
    private float nextAttackTime;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if (agent == null)
        {
            Debug.LogError("DroneAI: falta NavMeshAgent.");
            enabled = false;
            return;
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError("DroneAI: no hay puntos de patrullaje.");
            enabled = false;
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError(
                "DroneAI: el dron no esta sobre un NavMesh valido."
            );
            enabled = false;
            return;
        }

        patrolSpeed = Mathf.Max(0.1f, patrolSpeed);
        chaseSpeed = Mathf.Max(0.1f, chaseSpeed);

        agent.speed = patrolSpeed;
        agent.stoppingDistance = 0f;

        GoToNextPoint();
    }

    private void Update()
    {
        if (agent == null || !agent.isOnNavMesh)
            return;

        if (targetInvisibility != null && targetInvisibility.IsInvisible)
        {
            target = null;
            targetInvisibility = null;

            agent.isStopped = false;
            agent.speed = patrolSpeed;

            GoToNextPoint();
            return;
        }

        if (target != null && target.IsDead)
        {
            target = null;
            GoToNextPoint();
        }

        if (target != null)
        {
            ChasePlayer();
        }
        else
        {
            Patrol();
        }
    }

    public void SetTarget(PlayerHealth player)
    {
        if (player == null || player.IsDead)
            return;

        PlayerInvisibility invisibility =
            player.GetComponent<PlayerInvisibility>();

        if (invisibility != null && invisibility.IsInvisible)
            return;

        target = player;
        targetInvisibility = invisibility;

        agent.speed = chaseSpeed;

        Debug.Log("Dron: jugador detectado.");
    }

    public void RemoveTarget(PlayerHealth player)
    {
        if (target != player)
            return;

        target = null;

        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = false;
            agent.speed = patrolSpeed;

            GoToNextPoint();
        }

        Debug.Log("Dron: jugador fuera del rango.");
    }

    private void Patrol()
    {
        agent.speed = patrolSpeed;

        if (patrolPoints.Length == 0 || agent.pathPending)
            return;

        if (agent.remainingDistance <= 0.2f)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= waitAtPoint)
            {
                GoToNextPoint();
            }
        }
    }

    private void GoToNextPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (agent == null || !agent.isOnNavMesh)
            return;

        waitTimer = 0f;

        Transform point = patrolPoints[currentPoint];

        currentPoint = (currentPoint + 1) % patrolPoints.Length;

        if (point == null)
        {
            Debug.LogWarning("DroneAI: hay un punto sin asignar.");
            return;
        }

        agent.isStopped = false;
        agent.speed = patrolSpeed;
        agent.SetDestination(point.position);
    }

    private void ChasePlayer()
    {
        if (target == null)
            return;

        agent.speed = chaseSpeed;

        Vector3 targetPosition = target.transform.position;

        float distance = Vector3.Distance(
            transform.position,
            targetPosition
        );

        agent.stoppingDistance = attackRange;
        agent.SetDestination(targetPosition);

        if (distance <= attackRange + 0.2f)
        {
            agent.isStopped = true;

            if (Time.time >= nextAttackTime)
            {
                target.TakeDamage(damage);
                nextAttackTime = Time.time + attackCooldown;

                Debug.Log("Dron: ataque realizado.");
            }
        }
        else
        {
            agent.isStopped = false;
        }
    }
}
