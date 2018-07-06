using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingIcon : MonoBehaviour {

    public Collider follow;

    private void Update() {
        if (follow) {
            transform.position = follow.transform.position + Vector3.up * follow.bounds.extents.y * 1.5f;
            transform.Rotate(0, Time.deltaTime * 30, 0);
        } else {
            Destroy(gameObject);
        }
    }


}
