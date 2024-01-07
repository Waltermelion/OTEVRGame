using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;

public class ConstrainedRotation : MonoBehaviour
{
    public XRGrabInteractable grabInteractable;
    public Transform attachPoint;

    private Quaternion initialRotation;

    /*void Start()
    {
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        if (grabInteractable.isSelected)
        {
            // Apply rotation around the Y axis only
            Vector3 direction = attachPoint.position - transform.position;
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = Quaternion.Euler(0f, rotation.eulerAngles.y, 0f);
        }
        else
        {
            // Reset to initial rotation when not grabbed
            transform.localRotation = initialRotation;
        }
    }*/
}
