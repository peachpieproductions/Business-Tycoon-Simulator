using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ComputerIcon : MonoBehaviour, IPointerDownHandler {

    public Computer computer;
    public string link;

    private void Start() {
        computer = GetComponentInParent<Computer>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        switch(link) {
            case "Shop":
                computer.marketplaceOpen = true;
                break;
            case "Property":
                computer.propertyWebsiteOpen = true;
                break;
            case "CloseShop":
                computer.marketplaceOpen = false;
                break;
            case "CloseProperty":
                computer.propertyWebsiteOpen = false;
                break;
            case "Exit":
                computer.playerUsing.StopUsingAsset();
                break;
        }
        computer.UpdateComputer();
    }


}
