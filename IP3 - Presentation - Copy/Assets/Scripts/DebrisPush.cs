using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebrisPush : MonoBehaviour {

    [SerializeField] float force;
    [SerializeField] float verticalPuch;

    private void OnTriggerStay(Collider hit)
    {
        Rigidbody rig = hit.gameObject.GetComponent<Rigidbody>();
        if (rig == null || rig.isKinematic)
            return;

        Vector3 hitPos = new Vector3(hit.transform.position.x, 0, hit.transform.position.z);
        Vector3 charPos = new Vector3(transform.position.x, 0, transform.position.z);
        Vector3 dir = (hitPos - charPos + new Vector3(0, verticalPuch, 0)).normalized;

        // Apply the push
        rig.AddForce(dir * force);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Decoration")
        {
            Destroy(other.gameObject);
        }
    }
}
