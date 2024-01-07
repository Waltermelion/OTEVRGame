using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SniperHandler : MonoBehaviour
{
    public int bullets;
    public GameObject shootPoint;
    private bool sniperShootable;
    public AudioSource sniperASource;

    // Start is called before the first frame update
    void Start()
    {
        bullets = 5;
    }
    public void ShootSniper()
    {
        if (bullets > 0 && sniperShootable)
        {
            sniperASource.Play();
            //Implement delay damage github copilot
            RaycastHit hitInfo = new RaycastHit();
            bool hit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo);
            if (hit)
            {                
                if (hitInfo.collider.gameObject.tag == "Enemy")
                {
                    //kill enemy
                    sniperShootable = false;
                }else if (hitInfo.collider.gameObject.tag == "Npc")
                {
                    //kill Npc
                    sniperShootable = false;
                }
                //shooting sound
                //shooting animation
                
            }
            bullets--;
        }
        else
        {
            //No bullets sound
        }
    }
}