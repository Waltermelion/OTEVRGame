using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CopyMotion : MonoBehaviour
{
    public Transform targetLimb;
    ConfigurableJoint configurableJoint;

    private void Start() {
        configurableJoint = GetComponent<ConfigurableJoint>();
    }

    private void Update() {
        configurableJoint.targetRotation = transform.rotation;
    }
}
