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
                List<AssetInventorySlot> newOrder = new List<AssetInventorySlot>();
                for (int i = 0; i < 10; i++) { newOrder.Add(new AssetInventorySlot()); }
                foreach (ShopAssetListing ass in market.shopListings) {
                    if (ass.inCart > 0) {
                        bool placedAsset = false;
                        foreach (AssetInventorySlot slot in newOrder) { //find existing asset stack
                            if (slot.amount > 0 && slot.asset == ass.asset) {
                                slot.amount += ass.inCart;
                                placedAsset = true;
                                break;
                            }
                        }
                        if (!placedAsset) {
                            foreach (AssetInventorySlot slot in newOrder) { //make new stack
                                if (slot.amount == 0) {
                                    slot.amount += ass.inCart;
                                    slot.asset = ass.asset;
                                    placedAsset = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                computer.playerUsing.upcomingDeliveries.Add(newOrder);
                computer.playerUsing.money -= market.shopCartTotalAmount;
                computer.playerUsing.pui.CreateInfoPopup("- $" + Mathf.Round(market.shopCartTotalAmount), C.c.data.colors[1]);
                if (computer.playerUsing.deliveryTimer <= 0) {
                    computer.playerUsing.deliveryTimer = 3 * 3;
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
