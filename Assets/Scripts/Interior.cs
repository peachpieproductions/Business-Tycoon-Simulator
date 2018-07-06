using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interior : MonoBehaviour {

    public List<GameObject> geo = new List<GameObject>();
    public Portal[] enterPortals;
    public Portal[] exitPortals;

    private void Start() {
        SetInterior(false);
    }

    public void SetInterior(bool enabled) {
        foreach(Portal p in exitPortals) {
            p.gameObject.SetActive(enabled);
        }
        foreach(GameObject g in geo) {
            g.SetActive(enabled);
        }
    }


}
