
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public NavMeshAgent agent;

    public Transform player;
    public LayerMask isGround, isPlayer;
    //Patroling
    public Vector3 walkPoint;
    bool walkPointSet;
    public float walkPointRange;
    
    //States
    public float sightRange;
    public bool playerInSightRange;

    private void Awake()
    {
        player = GameObject.Find("Player").transform;
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Check for sight
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, isPlayer);

        if (!playerInSightRange)
        {
            Patroling();
        }
        else
        {
            ChasePlayer();
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

        if (Physics.Raycast(walkPoint, -transform.up, 2f, isGround))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        agent.SetDestination(player.position);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position,walkPointRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }
}
