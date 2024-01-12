using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.AI;
using UnityEngine.Events;
//using UnityEditor.PackageManager;

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
    public GameObject securityObj;
    public GameObject bloodPrefab;

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

            if (securityObj.activeSelf)
            {
                // Lose Condition
                Debug.Log("Lose");
                Invoke("ShowDefeatScreen", 3f);
            }

            //Implement delay damage github copilot
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(shootPoint.gameObject.transform.position,shootPoint.transform.forward, out hitInfo);
            if (hit)
            {                
                if (hitInfo.collider.gameObject.tag == "Enemy")
                {
                    //kill enemy
                    Debug.Log("Enemy Killed");
                    ShowVictoryScreen();
                    KillTarget(hitInfo);                    
                    //Invoke("ShowVictoryScreen", 3f);
                    /*hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().enabled = false;
                    hitInfo.collider.gameObject.GetComponent<NavMeshAgent>().speed = 0f;
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().animator.SetBool("Died", true);
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().panicSpeed = 0f;
                    transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    transform.gameObject.GetComponent<XRGrabInteractable>().enabled = false;*/
                }
                else if (hitInfo.collider.gameObject.tag == "Npc")
                {
                    //kill Npc
                    Debug.Log("Civilian Killed");
                    ShowDefeatScreen();
                    KillTarget(hitInfo);                    
                    //Invoke("ShowDefeatScreen", 3f);
                    /*hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().enabled = false;
                    hitInfo.collider.gameObject.GetComponent<NavMeshAgent>().speed = 0f;
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().animator.SetBool("Died", true);
                    hitInfo.collider.gameObject.GetComponent<NPCBehaviour>().panicSpeed = 0f;
                    transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
                    transform.gameObject.GetComponent<XRGrabInteractable>().enabled = false;*/
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
        infohit.collider.gameObject.GetComponent<NPCBehaviour>().enabled = false;
        infohit.collider.gameObject.GetComponent<NavMeshAgent>().speed = 0f;
        infohit.collider.gameObject.GetComponent<NPCBehaviour>().animator.SetBool("Died", true);
        infohit.collider.gameObject.GetComponent<NPCBehaviour>().panicSpeed = 0f;
        transform.gameObject.GetComponent<Rigidbody>().isKinematic = true;
        transform.gameObject.GetComponent<XRGrabInteractable>().enabled = false;
        Instantiate(bloodPrefab,infohit.collider.transform.position, Quaternion.identity);
    }

    public void ShowDefeatScreen()
    {
        GameManager.Instance.DefeatScreen();
    }

    public void ShowVictoryScreen()
    {
        GameManager.Instance.VictoryScreen();
    }
}