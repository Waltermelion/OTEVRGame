using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class policecar : MonoBehaviour
{
    public AudioSource policeAudio;    
    void soundPoliceCar()
    {
        if (!policeAudio.isPlaying)
        {
            policeAudio.Play();
        }        
    }
}
