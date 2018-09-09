using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ComputerMarket : MonoBehaviour {

    public Computer computer;
    public List<ShopAssetListing> shopListings = new List<ShopAssetListing>();
    public AssetData.Type? currentSortingType = null;
    public List<Button> sortTypeButtons = new List<Button>();
    public float shippingCost;
    public float shopCartTotalAmount;
    public ShopAssetListing assetListingPrototype;
    public TextMeshProUGUI shopCartTotalText;
    public TextMeshProUGUI shippingCostText;
    public RectScrollInput rectScrollInput;

    public void ChangeSortTab(int i) {

        switch(i) {
            case 0: currentSortingType = null; break;
            case 1: currentSortingType = AssetData.Type.Furniture; break;
            case 2: currentSortingType = AssetData.Type.Food; break;
            case 3: currentSortingType = AssetData.Type.RawMaterial; break;
            case 4: currentSortingType = AssetData.Type.Tech; break;
            case 5: currentSortingType = AssetData.Type.Item; break;
            case 6: currentSortingType = AssetData.Type.Wearable; break;
        }

        foreach(Button b in sortTypeButtons) {
            var cols = b.colors;
            if (b.transform.GetSiblingIndex() != i) cols.normalColor = Color.white;
            else cols.normalColor = C.c.data.colors[2];
            b.colors = cols;
        }

        foreach (ShopAssetListing l in shopListings) Destroy(l.gameObject);
        shopListings.Clear();

        rectScrollInput.SetScrollPoint(0);

        UpdateMarketplace();

    }

    public void UpdateMarketplace() {

        //update store
        if (computer.marketplaceOpen) {
            shippingCost = Mathf.Round(C.c.data.shippingCrate.currentValue);
            shippingCostText.text = "+$" + shippingCost;
            if (shopListings.Count == 0) {
                foreach (AssetData ad in C.c.data.assetData) {
                    if (currentSortingType == null || ad.type == currentSortingType) {
                        var inst = Instantiate(assetListingPrototype, assetListingPrototype.transform.parent);
                        shopListings.Add(inst);
                        inst.gameObject.SetActive(true);
                        inst.asset = ad;
                        inst.assetNametext.text = ad.name;
                        inst.assetCurrentValue.text = "$" + ad.currentValue.ToString("F0");
                        inst.assetBaseValue.text = "$" + ad.baseValue.ToString("F0");
                    }
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
                if (shopCartTotalAmount > 0) shopCartTotalAmount += shippingCost;
                shopCartTotalText.text = "Cart Total - $" + shopCartTotalAmount;
            }

            if (shopListings.Count > 0) {
                rectScrollInput.endY = rectScrollInput.startY + shopListings.Count * rectScrollInput.perListingScrollHeightValue;
            }

            //Init button color selection
            if (currentSortingType == null) {
                var cols = sortTypeButtons[0].colors;
                cols.normalColor = C.c.data.colors[2];
                sortTypeButtons[0].colors = cols;
            }

        }
        

    }

}
