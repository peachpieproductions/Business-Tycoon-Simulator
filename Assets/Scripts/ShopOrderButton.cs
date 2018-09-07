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
                foreach (ShopAssetListing ass in computer.shopListings) {
                    computer.playerUsing.AddToUpcomingDelivery(ass.asset,ass.inCart);
                }
                computer.playerUsing.money -= computer.shopCartTotalAmount;
                computer.playerUsing.pui.CreateInfoPopup("- $" + Mathf.Round(computer.shopCartTotalAmount), C.c.data.colors[1]);
                if (computer.playerUsing.deliveryTimer <= 0) {
                    computer.playerUsing.deliveryTimer = 60 * 3;
                }
            }
        }
    }

}
