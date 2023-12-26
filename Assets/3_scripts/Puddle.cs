using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public string targetTag;
    public int iteration = 3;
    private bool cleaniteration = true;
    public MeshRenderer puddlemat;

    /*private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == targetTag && iteration >= 1)
        {            
            iteration -= 1;
        }
        else if(iteration == 0)
        {
            gameObject.SetActive(false);
        }
    }*/
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == targetTag && iteration >= 1 && cleaniteration)
        {
            cleaniteration = false;
            Invoke("Subtract", 1f);
            //iteration -= 1;
        }
        else if(iteration == 0)
        {
            gameObject.SetActive(false);
        }
    }
    private void Subtract()
    {
        /*Color color = puddlemat.material.color;
        color.a -= 50;
        puddlemat.material.color = color;*/
        iteration -= 1;        
        cleaniteration = true;
    }
}