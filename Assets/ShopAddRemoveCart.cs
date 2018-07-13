using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopAddRemoveCart : MonoBehaviour, IPointerDownHandler {

    public bool addToCart;
    ShopAssetListing listing;

    private void Start() {
        listing = GetComponentInParent<ShopAssetListing>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        if (addToCart) listing.AddToCart();
        else listing.RemoveFromCart();
    }

}
