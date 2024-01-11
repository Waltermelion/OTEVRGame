using UnityEngine;

public class SecurityController : MonoBehaviour
{
    public float timeToSpawn;
    public float timeToDespawn;
    public float timeThreshold;
    private float elapsedTime = 0f;
    public Door door;

    private FieldOfView fov;
    private SecurityBehaviour secBehaviour;
    private AudioSource audioSource;

    void Start()
    {
        fov = GetComponent<FieldOfView>();
        secBehaviour = GetComponent<SecurityBehaviour>();
        audioSource = GetComponent<AudioSource>();

        gameObject.SetActive(false);

        Invoke("SpawnSecurityGuard", timeToSpawn);
    }

    void Update()
    {
        // Check if the player is within the field of view
        if (fov.canSeePlayer)
        {
            // If the player is seen for more than the threshold time, the player loses
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= timeThreshold)
            {
                PlayerLoses();
            }
        }
        else
        {
            // Reset the elapsed time
            elapsedTime = 0f;
        }
    }

    void SpawnSecurityGuard()
    {
        gameObject.SetActive(true);

        door.OpenDoor();

        Invoke("DespawnSecurityGuard", timeToDespawn);
    }

    void DespawnSecurityGuard()
    {
        transform.position = secBehaviour.directionsParent.GetChild(0).transform.position;

        door.CloseDoor();

        gameObject.SetActive(false);
    }

    void PlayerLoses()
    {
        
    }
}