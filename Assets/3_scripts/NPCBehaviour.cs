using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NPCBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    // Assign this object in the inspector, the locations are the child objects of this transform
    public Transform directionsParent;
    // Array to set the time, in seconds, that the NPC stays in each point
    public float[] timeAtEachPoint;
    // This allows movement looping
    public bool loop = false;

    [Header("Panic Settings")]
    // In seconds
    public float panicDuration = 5f;
    public float panicSpeed = 10f;
    public int panicWaypoints = 15;
    public float panicRadius = 6f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent navMeshAgent;
    private float defaultSpeed;

    public enum NPCState
    {
        Idle,
        Moving,
        Panic
    }

    [Header("State Settings")]
    public NPCState currentState;
    public NPCState lastState;

    void Start()
    {
        SetState(NPCState.Moving);
        navMeshAgent = GetComponent<NavMeshAgent>();
        defaultSpeed = navMeshAgent.speed;
        waypoints = new Transform[directionsParent.childCount];

        for (int i = 0; i < directionsParent.childCount; i++)
        {
            waypoints[i] = directionsParent.GetChild(i);
        }

        StartCoroutine(MoveToNextWaypoint());
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (true)
        {
            switch (currentState)
            {
                case NPCState.Idle:
                    // Optionally, you can add some logic here for the Idle state
                    break;

                case NPCState.Moving:
                    // Move to the current waypoint
                    navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

                    while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                    {
                        if (currentState == NPCState.Panic)
                        {
                            goto ContinueWhileLoop; // Continue with the next iteration of the while loop
                        }
                        yield return null;
                    }

                    // Check if the index is within the bounds of the timeAtEachPoint array
                    if (currentWaypointIndex < timeAtEachPoint.Length)
                    {
                        // Wait for the specified time at the waypoint
                        yield return new WaitForSeconds(timeAtEachPoint[currentWaypointIndex]);
                    }

                    // Move to the next waypoint
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

                    // Check if we reached the last waypoint and should stop
                    if (currentWaypointIndex == 0 && !loop)
                    {
                        SetState(NPCState.Idle);
                    }
                    ContinueWhileLoop:
                    break;

                case NPCState.Panic:
                    navMeshAgent.speed = panicSpeed;

                    // Trigger panic movement with multiple waypoints
                    for (int i = 0; i < panicWaypoints; i++)
                    {
                        Vector3 randomDestination = GetRandomPositionAround(transform.position, panicRadius);
                        navMeshAgent.SetDestination(randomDestination);
                        yield return new WaitForSeconds(panicDuration / panicWaypoints);
                    }

                    // Go back to the last state
                    SetState(lastState);
                    navMeshAgent.speed = defaultSpeed;
                    break;
            }

            // Yield once per frame to allow other processes
            yield return null;
        }
    }

    // Function to set the NPC state
    void SetState(NPCState newState)
    {
        lastState = currentState;
        currentState = newState;
    }

    // Function to get a random position around a given point
    Vector3 GetRandomPositionAround(Vector3 center, float radius)
    {
        float angle = Random.Range(0f, 2f * Mathf.PI);
        float distance = Random.Range(0f, radius);
        Vector3 randomPosition = center + new Vector3(Mathf.Cos(angle) * distance, 0f, Mathf.Sin(angle) * distance);
        return randomPosition;
    }

    // DEBUG SECTION
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetState(NPCState.Panic);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, panicRadius);
    }

    void OnDrawGizmos()
    {
        if (waypoints != null)
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(waypoints[i].position, 0.5f);
            }
        }
    }
}