using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class VisualNodeLine : MonoBehaviour {

    public Transform from;
    public Transform to;

    private void Update() {
        if (from && to) {
            GetComponent<LineRenderer>().SetPosition(0, from.position + Vector3.right * 1.6f);
            GetComponent<LineRenderer>().SetPosition(1, to.position + Vector3.left * 1.6f);
        } else {
            DestroyImmediate(gameObject);
        }
    }




}
