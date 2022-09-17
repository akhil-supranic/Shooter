
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    [SerializeField]private NavMeshAgent agent;
    [SerializeField]private Transform player;
    [SerializeField]private LayerMask isGround, isPlayer;
    //Patroling
    [SerializeField]private Vector3 walkPoint;
    bool walkPointSet;
    [SerializeField]private float walkPointRange;

    //Attacking
     [SerializeField]private float timeBetweenAttacks;
    [SerializeField]private bool alreadyAttacked;
    
    //States
    [SerializeField]private float sightRange;
    [SerializeField]private float attackRange;
    [SerializeField]private bool playerInSightRange;
    [SerializeField]private bool playerInAttackRange;
    [SerializeField]private Shoot shoot;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);
         playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, isPlayer);

        if (!playerInSightRange && !playerInAttackRange)
        {
            Patroling();
        }
        else if(playerInSightRange && !playerInAttackRange)
        {
            ChasePlayer();
        }
        else if(playerInAttackRange && playerInAttackRange)
        {
            AttackPlayer();
        }
       
    }

    private void Patroling()
    {
        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);

        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        //Walkpoint reached
        if (distanceToWalkPoint.magnitude < 1f)
            walkPointSet = false;
    }
    private void SearchWalkPoint()
    {
        //Calculate random point in range
         float randomX = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x+ randomX, transform.position.y, transform.position.z);

        if (Physics.Raycast(walkPoint, -transform.up, 1f, isGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void AttackPlayer()
    {
        agent.SetDestination(transform.position);

        transform.LookAt(player);
        if (!alreadyAttacked)
        {
            shoot.Shooting();
             alreadyAttacked = true;
            Invoke(nameof(ResetAttack), timeBetweenAttacks);
        }

    }

     private void ResetAttack()
    {
        alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,walkPointRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
