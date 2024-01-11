using UnityEngine;

public class Door : MonoBehaviour
{
    // 
    public float rotationSpeed = 2.0f;
    public Quaternion openRotation;
    public Quaternion closeRotation;
    public AudioClip[] audioClips;

    //
    private AudioSource audioSource;
    private bool isOpen = false;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        closeRotation = transform.rotation;

        openRotation = Quaternion.Euler(0, 90f, 0);
    }

    private void Update()
    {
        if (isOpen)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation , openRotation, Time.deltaTime * rotationSpeed);
        }
        else
        {
            transform.rotation = Quaternion.Lerp(transform.rotation , closeRotation, Time.deltaTime * rotationSpeed);
        }
    }

    public void OpenDoor()
    {
        isOpen = true;
        audioSource.PlayOneShot(audioClips[0]);
    }

    public void CloseDoor()
    {
        audioSource.PlayOneShot(audioClips[1]);
        isOpen = false;
    }
}
