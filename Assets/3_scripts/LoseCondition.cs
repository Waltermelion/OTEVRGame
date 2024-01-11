using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseCondition : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "LoseCondition")
        {
            //Lose
            Debug.Log("Player lost");
        }
    }
}
