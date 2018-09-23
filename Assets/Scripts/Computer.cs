using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Computer : MonoBehaviour {

    public Asset asset;
    public Player playerUsing;
    public RectTransform mouseSprite;
    public bool marketplaceOpen;
    public bool propertyWebsiteOpen;
    public ComputerMarket marketplace;
    public ComputerProperty propertyWebsite;
    public TextMeshProUGUI timeText;
    public Canvas screenCanvas;
    public Shop shopController;

    private void Start() {
        asset = GetComponentInParent<Asset>();
        screenCanvas.worldCamera = Camera.main;
        UpdateComputer();
    }

    public void StartUsingComputer(Player p) {
        playerUsing = p;
        p.SetCameraMode(false, false, asset.camOverride);
        playerUsing.pui.SetUI(false, false);
        p.usingAsset = asset;
        UpdateComputer();
    }

    public void EndUsingComputer() {
        playerUsing = null;
    }

    public void UpdateComputer() {

        if (asset && !asset.placing) {

            //Marketplace
            marketplace.gameObject.SetActive(marketplaceOpen);
            marketplace.UpdateMarketplace();

            //Properties
            propertyWebsite.gameObject.SetActive(propertyWebsiteOpen);
            propertyWebsite.UpdatePropertyWebsite();

        }

    }

    float mouseSpd = 7f;
    private void Update() {

        timeText.text = C.c.timeString;

        if (playerUsing) {
            if (playerUsing.usingAsset != asset) { EndUsingComputer();  return; }

            //Virual Mouse
            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(screenCanvas.transform as RectTransform, Input.mousePosition, screenCanvas.worldCamera, out pos);
            mouseSprite.position = screenCanvas.transform.TransformPoint(pos);
            mouseSprite.anchoredPosition = new Vector2(Mathf.Clamp(mouseSprite.anchoredPosition.x, 10, 1110), Mathf.Clamp(mouseSprite.anchoredPosition.y, 30, 770));


        }
    }



}




