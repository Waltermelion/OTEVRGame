using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Content.Interaction;

public class TripeMovement : MonoBehaviour
{
    private Vector3 currentpos;
    public float moveSpeed;
    public GameObject joystickObject;

    private void Start()
    {
        currentpos = transform.position;
    }
    public void OnJoystickChanged()
    {
        if (joystickObject.GetComponent<XRJoystick>().value.y > 1)
        {
            currentpos.y += moveSpeed;
            transform.position = currentpos;
        }else if (joystickObject.GetComponent<XRJoystick>().value.y < -1)
        {
            currentpos.y -= moveSpeed;
            transform.position = currentpos;
        }
    }
}
