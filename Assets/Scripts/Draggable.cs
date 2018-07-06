using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour, IPointerDownHandler, IPointerUpHandler {

    public RectTransform parentToMove;
    public bool isDragging;
    public Vector2 offset;
    public Vector2 mousePos;

    public void OnPointerDown(PointerEventData ped) {
        isDragging = true;
        offset = parentToMove.anchoredPosition - ped.position;
    }

    private void Update() {
        if (isDragging) {
            
        }
    }

    public void OnPointerUp(PointerEventData ped) {
        isDragging = false;
    }

}
