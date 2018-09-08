using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ShopAssetListing : MonoBehaviour {

    Computer computer;
    public AssetData asset;
    public TextMeshProUGUI assetNametext;
    public TextMeshProUGUI assetBaseValue;
    public TextMeshProUGUI assetCurrentValue;
    public TextMeshProUGUI assetInCart;
    public TextMeshProUGUI assetTotalCost;
    public int inCart;

    private void Start() {
        computer = GetComponentInParent<Computer>();
    }

    public void AddToCart() {
        inCart++;
        assetInCart.text = inCart.ToString();
        computer.UpdateComputer();
    }

    public void RemoveFromCart() {
        if (inCart > 0) inCart--;
        assetInCart.text = inCart.ToString();
        computer.UpdateComputer();
    }

    public void ResetListing() {
        inCart = 0;
        assetInCart.text = inCart.ToString();
        computer.UpdateComputer();
    }

    /*public void BuyAsset() {
        if (Mathf.Round(computer.playerUsing.money) >= Mathf.Round(asset.currentValue)) {
            computer.playerUsing.AddToUpcomingDelivery(asset);
            computer.playerUsing.money -= Mathf.Round(asset.currentValue);
            computer.playerUsing.pui.CreateInfoPopup("- $" + Mathf.Round(asset.currentValue), C.c.data.colors[1]);
            if (computer.playerUsing.deliveryTimer <= 0) {
                computer.playerUsing.deliveryTimer = 60 * 3;
            }
        }
    }*/


}
