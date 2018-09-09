﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RectScrollInput : MonoBehaviour {

    public float scrollSpeed = 1;
    public float startY;
    public float endY;
    public float perListingScrollHeightValue = 31.5f;
    RectTransform rect;
    public Slider scrollbar;


    private void Start() {
        rect = transform as RectTransform;
        startY = rect.anchoredPosition.y;
    }

    public void SetScrollPoint(float point = -1) {
        if (point == -1) {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, C.RemapFloat(scrollbar.value, 0, 1, startY, endY));
            scrollbar.value = C.RemapFloat(rect.anchoredPosition.y, startY, endY, 0, 1);
        } else {
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, C.RemapFloat(point, 0, 1, startY, endY));
            scrollbar.value = C.RemapFloat(rect.anchoredPosition.y, startY, endY, 0, 1);
        }
    }

    private void Update() {
        if (Input.mouseScrollDelta.y != 0) {
            rect.anchoredPosition -= new Vector2(0, Input.mouseScrollDelta.y * scrollSpeed);
            rect.anchoredPosition = new Vector2(rect.anchoredPosition.x, Mathf.Clamp(rect.anchoredPosition.y, startY, endY));
            scrollbar.value = C.RemapFloat(rect.anchoredPosition.y, startY, endY, 0, 1);
        }
    }

}