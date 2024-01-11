using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

[RequireComponent(typeof(NavMeshAgent))]
public class SecurityBehaviour : MonoBehaviour
{
    [Header("Movement Settings")]
    // Assign this object in the inspector, the locations are the child objects of this transform
    public Transform directionsParent;
    // Array to set the time, in seconds, that the NPC stays in each point
    public float[] timeAtEachPoint;
    // This allows movement looping
    public bool loop = false;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent navMeshAgent;
    public Animator animator;

    public enum NPCState
    {
        Idle,
        Moving,
    }

    public enum NPCIdleState
    {
        RegularIdle,
        LookOut
    }

    [Header("State Settings")]
    public NPCState currentState;
    public NPCIdleState currentIdleState;
    public NPCIdleState[] npcIdleStates;

    private NPCState lastState;

    void OnEnable()
    {
        SetState(NPCState.Moving);
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
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
                case NPCState.Moving:
                    // Move to the current waypoint
                    navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
                    animator.SetBool("Moving", true);

                    while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                    {
                        yield return null;
                    }

                    // Check if the index is within the bounds of the timeAtEachPoint array
                    if (currentWaypointIndex < timeAtEachPoint.Length)
                    {
                        SetState(NPCState.Idle);

                        // Wait for the specified time at the waypoint
                        yield return new WaitForSeconds(timeAtEachPoint[currentWaypointIndex]);

                        SetState(NPCState.Moving);

                        animator.SetBool("Moving", true);
                    }

                    // Move to the next waypoint
                    currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;

                    // Check if we reached the last waypoint and should stop
                    if (currentWaypointIndex == 0 && !loop)
                    {
                        SetState(NPCState.Idle);
                    }
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

        if (currentState == NPCState.Idle)
        {
            HandleIdleStates();
        }
    }

    #region DEBUG
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
    #endregion

    public void HandleIdleStates()
    {
        string animationString = npcIdleStates[currentWaypointIndex].ToString();
        animator.SetTrigger(animationString);
        animator.SetBool("Moving", false);
        Debug.Log(animationString);
    }
}