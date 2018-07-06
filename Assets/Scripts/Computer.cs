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
    public bool shopOpen;
    public GameObject shopPanel;
    public List<ShopAssetListing> shopListings = new List<ShopAssetListing>();
    public ShopAssetListing assetListingPrototype;
    public TextMeshProUGUI timeText;
    public Transform assetBeingSoldModel;
    public Transform moneySpawnPoint;
    public Vector2 mousePos;
    public Canvas screenCanvas;

    private void Start() {
        asset = GetComponentInParent<Asset>();
        UpdateComputer();
    }

    public void StartUsingComputer(Player p) {
        C.c.virtualInputModule.m_VirtualCursor = mouseSprite;
        playerUsing = p;
        Cursor.lockState = CursorLockMode.None;
        p.usingAsset = asset;
        p.FreeCamToggle();
        p.camOverridePos = asset.camOverride;
    }

    public void EndUsingComputer() {
        playerUsing = null;
    }

    public void UpdateComputer() {

        shopPanel.SetActive(shopOpen);

        //update store
        if (shopOpen) {
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
            } else {
                foreach(ShopAssetListing s in shopListings) {
                    s.assetCurrentValue.text = "$" + s.asset.currentValue.ToString("F0");
                    if (Mathf.Round(s.asset.currentValue) <= Mathf.Round(s.asset.baseValue)) s.assetCurrentValue.color = C.c.data.colors[0];
                    else s.assetCurrentValue.color = C.c.data.colors[1];
                }
            }
        }
        
    }

    float mouseSpd = 7f;
    private void Update() {

        timeText.text = C.c.timeString;

        if (assetBeingSoldModel.childCount > 0) {
            assetBeingSoldModel.Rotate(0, Time.deltaTime * 20f, 0);
        }

        if (playerUsing) {
            if (playerUsing.usingAsset != asset) { EndUsingComputer();  return; }

            Vector2 pos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(screenCanvas.transform as RectTransform, Input.mousePosition, screenCanvas.worldCamera, out pos);
            mouseSprite.position = screenCanvas.transform.TransformPoint(pos);
            mouseSprite.anchoredPosition = new Vector2(Mathf.Clamp(mouseSprite.anchoredPosition.x, 10, 1110), Mathf.Clamp(mouseSprite.anchoredPosition.y, 30, 770));


        }
    }



}
