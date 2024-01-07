using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class SniperHandler : MonoBehaviour
{
    public int bullets;
    public GameObject shootPoint;
    private bool sniperShootable = true;
    public AudioSource sniperASource;
    public AudioClip shootSound;
    public AudioClip noBulletSound;

    // Start is called before the first frame update
    void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.activated.AddListener(x => ShootSniper());

        bullets = 5;
    }
    public void ShootSniper()
    {
        if (bullets > 0 && sniperShootable)
        {
            sniperASource.PlayOneShot(shootSound);
            //Implement delay damage github copilot
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {                
                if (hitInfo.collider.gameObject.tag == "Enemy")
                {
                    //kill enemy
                    
                }else if (hitInfo.collider.gameObject.tag == "Npc")
                {
                    //kill Npc
                    
                }
                //shooting sound
                //shooting animation
                
            }
            sniperShootable = false;
            bullets--;
        }
        else
        {
            //No bullets sound
            sniperASource.PlayOneShot(noBulletSound);
        }
    }
}