using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopOrderButton : MonoBehaviour, IPointerDownHandler {

    ShopAssetListing listing;

    private void Start() {
        listing = GetComponentInParent<ShopAssetListing>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        listing.BuyAsset();
    }

}
