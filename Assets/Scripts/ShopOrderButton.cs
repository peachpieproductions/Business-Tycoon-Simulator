using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopOrderButton : MonoBehaviour, IPointerDownHandler {

    public Computer computer;

    public void OnPointerDown(PointerEventData eventData) {
        //listing.BuyAsset();
        if (computer.shopCartTotalAmount > 0) {
            if (computer.playerUsing.money >= computer.shopCartTotalAmount) {
                computer.playerUsing.money -= computer.shopCartTotalAmount;

            }
        }
    }

}
