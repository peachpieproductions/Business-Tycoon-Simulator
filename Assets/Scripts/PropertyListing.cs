using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PropertyListing : MonoBehaviour {

    Computer computer;
    public GameObject shopModel;
    public float cost;
    public bool owned;
    public TextMeshProUGUI buyButtonText;
    public TextMeshProUGUI nameText;

    private void Start() {
        computer = GetComponentInParent<Computer>();
    }

    public void BuyProperty() {

        if (shopModel) {
            if (owned) { computer.playerUsing.pui.CreateInfoPopup("You already own this Property.", Color.white, 3f); return; }
            if (computer.playerUsing.money < cost) { computer.playerUsing.pui.CreateInfoPopup("You don't have enough money to buy this Property.", Color.white, 3f); return; }

            computer.propertyWebsite.resetListings();
            buyButtonText.text = "Owned";
            owned = true;
            computer.playerUsing.money -= cost;
            if (computer.shopController.currentShop) computer.shopController.currentShop.gameObject.SetActive(false);
            shopModel.SetActive(true);
            computer.shopController.currentShop = shopModel;
            computer.playerUsing.pui.CreateInfoPopup("You purchased " + nameText.text + " Property for $" + cost + ".", Color.white, 8f);
        }

    }

}
