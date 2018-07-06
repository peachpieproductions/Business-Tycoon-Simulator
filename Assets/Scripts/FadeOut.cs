using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOut : MonoBehaviour {

    public float delay;
    CanvasGroup cg;

    private void Start() {
        cg = GetComponent<CanvasGroup>();
    }

    private void Update() {
        delay -= Time.deltaTime;
        if (delay <= 0) {
            cg.alpha -= Time.deltaTime;
            if (cg.alpha <= 0) Destroy(gameObject);
        }
    }

}
