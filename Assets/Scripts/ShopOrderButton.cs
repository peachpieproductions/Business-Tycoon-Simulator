using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopOrderButton : MonoBehaviour, IPointerDownHandler {

    public Computer computer;
    public ComputerMarket market;

    public void OnPointerDown(PointerEventData eventData) {
        if (market.shopCartTotalAmount > 0) {
            if (computer.playerUsing.money >= market.shopCartTotalAmount) {
                foreach (ShopAssetListing ass in market.shopListings) {
                    computer.playerUsing.AddToUpcomingDelivery(ass.asset,ass.inCart);
                }
                computer.playerUsing.money -= market.shopCartTotalAmount;
                computer.playerUsing.pui.CreateInfoPopup("- $" + Mathf.Round(market.shopCartTotalAmount), C.c.data.colors[1]);
                if (computer.playerUsing.deliveryTimer <= 0) {
                    computer.playerUsing.deliveryTimer = 60 * 3;
                }
                foreach(ShopAssetListing l in market.shopListings) {
                    l.ResetListing();
                }
            } else {
                computer.playerUsing.pui.CreateInfoPopup("You don't have enough money.", Color.white, 3f);
            }
        } else {
            computer.playerUsing.pui.CreateInfoPopup("Your cart is empty.", Color.white, 3f);
        }
    }

}
