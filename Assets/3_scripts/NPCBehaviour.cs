using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

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
    public GameObject panicWaypoints;
    public float panicSpeed = 10f;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent navMeshAgent;
    private float defaultSpeed;
    private bool canPanic;
    public Animator animator;
    public AudioSource npcAudio;
    public AudioClip[] npcAudioClips;

    public enum NPCState
    {
        Idle,
        Moving,
        Panic
    }

    public enum NPCIdleState
    {
        RegularIdle,
        Eat,
        Talk,
        Exercise,
        Smoke
    }

    [Header("State Settings")]
    public NPCState currentState;
    public NPCIdleState currentIdleState;
    public NPCIdleState[] npcIdleStates;

    private NPCState lastState;

    void Start()
    {
        SetState(NPCState.Moving);
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        canPanic = true;
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
                case NPCState.Moving:
                    // Move to the current waypoint
                    navMeshAgent.speed = defaultSpeed;
                    navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);
                    string animationString1 = npcIdleStates[currentWaypointIndex].ToString();
                    animator.SetBool(animationString1, false);
                    animator.SetBool("Moving", true);

                    while (navMeshAgent.pathPending || navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                    {
                        if (currentState == NPCState.Panic)
                        {
                            goto ContinueWhileLoop;
                        }
                        yield return null;
                    }

                    // Check if the index is within the bounds of the timeAtEachPoint array
                    if (currentWaypointIndex < timeAtEachPoint.Length)
                    {
                        SetState(NPCState.Idle);

                        // Wait for the specified time at the waypoint
                        yield return new WaitForSeconds(timeAtEachPoint[currentWaypointIndex]);

                        SetState(NPCState.Moving);

                        string animationString = npcIdleStates[currentWaypointIndex].ToString();
                        animator.SetBool(animationString, false);
                        animator.SetBool("Moving", true);
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
                    // Trigger panic movement with multiple waypoints
                    foreach (Transform child in panicWaypoints.transform)
                    {
                        navMeshAgent.SetDestination(child.position);

                        navMeshAgent.speed = panicSpeed;

                            while (navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance)
                            {
                                yield return null;
                            }
                    }

                    // Go back to the last state
                    //SetState(lastState);
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
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (currentState != NPCState.Panic)
            {
                SetState(NPCState.Panic);
            }
        }

        if (currentState == NPCState.Panic && canPanic)
        {
            canPanic = false;
            StopAllCoroutines();
            StartCoroutine(MoveToNextWaypoint());
        }
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
    #endregion

    public void HandleIdleStates()
    {
        string animationString = npcIdleStates[currentWaypointIndex].ToString();
        animator.SetBool(animationString, true);
        animator.SetBool("Moving", false);
        Debug.Log(animationString);
    }

    public void ShotsFired()
    {
        int i = Random.Range(0,npcAudioClips.Length);
        npcAudio.PlayOneShot(npcAudioClips[i]);
        SetState(NPCState.Panic); 
        animator.SetBool("Moving", false);
        animator.SetBool("Panic", true);
        Debug.Log("shots fired function");
    }
}