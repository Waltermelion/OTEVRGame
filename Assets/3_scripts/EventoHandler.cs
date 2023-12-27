using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventoHandler : MonoBehaviour
{
    public Puddle[] Puddles;
    public int currentEvento;
    // Start is called before the first frame update
    void Start()
    {
        currentEvento = 1;
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentEvento)
        {
            case 1:
                AreAllPuddlesSolved();
                if (AreAllPuddlesSolved() == true)
                {
                    currentEvento = 2;
                }
                break;
            case 2:
                break;
            default:
                break;
        } 
    }
    bool AreAllPuddlesSolved()
    {
        foreach (Puddle currPuddle in Puddles)
        {
            if (currPuddle.isActiveAndEnabled)
            {
                return false;
            }
        }
        return true;
    }
}