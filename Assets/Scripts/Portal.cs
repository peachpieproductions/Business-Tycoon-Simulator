using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour {

    public Portal goToPortal;
    public bool enterance;
    public Interior interior;

    private void Update() {
        transform.Rotate(0, Time.deltaTime * 30, 0);
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            other.GetComponent<Player>().nearbyPortal = this;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.transform.CompareTag("Player")) {
            other.GetComponent<Player>().nearbyPortal = null;
        }
    }


}
