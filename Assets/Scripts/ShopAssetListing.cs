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
        if (Input.GetKey(KeyCode.LeftShift)) inCart += 10;
        else inCart++;
        assetInCart.text = inCart.ToString();
        computer.UpdateComputer();
    }

    public void RemoveFromCart() {
        if (Input.GetKey(KeyCode.LeftShift)) inCart -= 10;
        else inCart--;
        if (inCart < 0) inCart = 0;
        assetInCart.text = inCart.ToString();
        computer.UpdateComputer();
    }

    public void ResetListing() {
        inCart = 0;
        assetInCart.text = inCart.ToString();
        computer.UpdateComputer();
    }



}
