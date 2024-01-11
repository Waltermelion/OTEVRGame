using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.AI;
using UnityEngine.Events;

public class SniperHandler : MonoBehaviour
{
    public int bullets;
    public GameObject shootPoint;
    public AudioSource sniperASource;
    public AudioClip shootSound;
    public AudioClip noBulletSound;
    public AudioClip reload1Sound;
    private bool hasSlide = true;
    public UnityEvent OnSniperShoot;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => ShootSniper());

        bullets = 5;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ShootSniper();
        }
    }
    public void ShootSniper()
    {
        if (bullets > 0 && hasSlide)
        {
            //shooting sound            
            sniperASource.PlayOneShot(shootSound);
            OnSniperShoot.Invoke();
            //shooting animation

            //Implement delay damage github copilot
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(shootPoint.gameObject.transform.position,shootPoint.transform.forward, out hitInfo);
            if (hit)
            {                
                if (hitInfo.collider.gameObject.tag == "Enemy")
                {
                    //kill enemy
                    Debug.Log("Enemy Killed");
                    //Invoke("KillTarget", 1f);
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().enabled = false;
                    hitInfo.collider.gameObject.GetComponent<NavMeshAgent>().speed = 0f;
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().animator.SetBool("Died", true);
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().panicSpeed = 0f;
                    transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    transform.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                }
                else if (hitInfo.collider.gameObject.tag == "Npc")
                {
                    //kill Npc
                    Debug.Log("Civilian Killed");
                    Invoke("KillTarget", 1f);
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().enabled = false;
                    hitInfo.collider.gameObject.GetComponent<NavMeshAgent>().speed = 0f;
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().animator.SetBool("Died", true);
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().panicSpeed = 0f;
                    transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    transform.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
                }
                Debug.Log("Hit: " + hitInfo.collider.name);
            }
            hasSlide = false;
            bullets--;
        }
        else
        {
            //No bullets sound
            sniperASource.PlayOneShot(noBulletSound);
        }
    }
    public void Slide()
    {
        hasSlide = true;
        sniperASource.PlayOneShot(reload1Sound);
    }
    private void KillTarget(RaycastHit infohit)
    {

    }
}