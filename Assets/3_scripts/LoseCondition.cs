using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    private GameObject loseCanvas;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LoseCondition")
        {
            //Lose
            loseCanvas.SetActive(true);
            Debug.Log("Player lost");
        }
    }
}
