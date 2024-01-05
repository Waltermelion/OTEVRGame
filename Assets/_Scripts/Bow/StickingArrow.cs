using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickingArrow : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private SphereCollider sphereCollider;

    [SerializeField] private GameObject stickigArrow;

    private void OnCollisionEnter(Collision collision) {
        rb.isKinematic = true;
        sphereCollider.isTrigger = true;

        GameObject arrow = Instantiate(stickigArrow);
        arrow.transform.position = transform.position;
        arrow.transform.forward = transform.forward;

        if (collision.collider.attachedRigidbody != null) {
            arrow.transform.parent = collision.collider.attachedRigidbody.transform;
        }

        collision.collider.transform.parent.GetComponent<IHittable>()?.GetHit();

        Destroy(gameObject);
    }
}
