using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FitPanelToText : MonoBehaviour {


    public TextMeshProUGUI text;
    public RectTransform rect;
    public float mult;
    public string txt;
    public bool updateNow;

    private void Start() {
        StartCoroutine(Fit());
    }

    /*private void OnValidate() {
        updateNow = false;
        if (txt != "") text.text = txt;
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(text.preferredWidth * mult + 30f, rect.sizeDelta.y);
    }*/

    IEnumerator Fit() {
        yield return null;
        rect = GetComponent<RectTransform>();
        rect.sizeDelta = new Vector2(text.preferredWidth * mult + 30f, rect.sizeDelta.y);
    }


}
