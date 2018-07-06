using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComputerIcon : MonoBehaviour, IPointerDownHandler {

    Computer computer;
    public string link;

    private void Start() {
        computer = GetComponentInParent<Computer>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        switch(link) {
            case "Shop":
                computer.shopOpen = true;
                computer.UpdateComputer();
                break;
            case "CloseShop":
                computer.shopOpen = false;
                computer.UpdateComputer();
                break;
        }
    }


}
