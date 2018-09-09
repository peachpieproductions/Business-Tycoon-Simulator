using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ComputerMarket : MonoBehaviour {

    public Computer computer;
    public List<ShopAssetListing> shopListings = new List<ShopAssetListing>();
    public float shopCartTotalAmount;
    public ShopAssetListing assetListingPrototype;
    public TextMeshProUGUI shopCartTotalText;

    public void UpdateMarketplace() {

        //update store
        if (computer.marketplaceOpen) {
            if (shopListings.Count == 0) {
                foreach (AssetData ad in C.c.data.assetData) {
                    var inst = Instantiate(assetListingPrototype, assetListingPrototype.transform.parent);
                    shopListings.Add(inst);
                    inst.gameObject.SetActive(true);
                    inst.asset = ad;
                    inst.assetNametext.text = ad.name;
                    inst.assetCurrentValue.text = "$" + ad.currentValue.ToString("F0");
                    inst.assetBaseValue.text = "$" + ad.baseValue.ToString("F0");
                }
                assetListingPrototype.gameObject.SetActive(false);
            }
            if (shopListings.Count > 0) {
                shopCartTotalAmount = 0;
                foreach (ShopAssetListing s in shopListings) {
                    s.assetCurrentValue.text = "$" + s.asset.currentValue.ToString("F0");
                    s.assetTotalCost.text = "$" + (Mathf.Round(s.asset.currentValue) * s.inCart).ToString("F0");
                    shopCartTotalAmount += Mathf.Round(s.asset.currentValue) * s.inCart;
                    if (Mathf.Round(s.asset.currentValue) <= Mathf.Round(s.asset.baseValue)) s.assetCurrentValue.color = C.c.data.colors[0];
                    else s.assetCurrentValue.color = C.c.data.colors[1];
                }
                shopCartTotalText.text = "Cart Total - $" + shopCartTotalAmount;
            }
        }
        

    }

}
