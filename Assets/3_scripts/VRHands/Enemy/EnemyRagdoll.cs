using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRagdoll : MonoBehaviour
{

    public void DestroyThisObject() {
        Destroy(this.gameObject, 3f);
    }
}
