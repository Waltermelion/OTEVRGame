using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class NPCMovement : MonoBehaviour
{
    // This transform holds all the directions/destinations of an NPC
    public Transform directionsParent;
    // This array holds the time in seconds that an NPC stays in a determined location (per order of the array above)
    public float[] timeAtEachPoint;

    private Transform[] waypoints;
    private int currentWaypointIndex = 0;
    private NavMeshAgent navMeshAgent;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();

        // Get all child waypoints from the directionsParent
        waypoints = new Transform[directionsParent.childCount];
        for (int i = 0; i < directionsParent.childCount; i++)
        {
            waypoints[i] = directionsParent.GetChild(i);
        }

        // Start NPC movement
        StartCoroutine(MoveToNextWaypoint());
    }

    IEnumerator MoveToNextWaypoint()
    {
        while (true)
        {
            // Move to the current waypoint
            navMeshAgent.SetDestination(waypoints[currentWaypointIndex].position);

            // Wait for the specified time at the waypoint
            yield return new WaitForSeconds(timeAtEachPoint[currentWaypointIndex]);

            // Move to the next waypoint
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }
}