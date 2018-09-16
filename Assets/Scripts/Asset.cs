using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[System.Serializable]
public class AssetInventorySlot {

    public AssetData asset;
    public int amount;

    public AssetInventorySlot Clone() {
        var slotClone = new AssetInventorySlot();
        slotClone.asset = asset;
        slotClone.amount = amount;
        return slotClone;
    }

}

public class Asset : MonoBehaviour {

    public AssetData data;
    public string assetName;
    public string useTag;
    public Player playerHovering;
    public Player playerUsing;
    public GameObject model;
    public Canvas overlayCanvas;
    public CanvasGroup cg;
    public Collider coll;
    public Outline outline;
    public bool placing;
    public Transform camOverride;
    public bool physicsAsset;
    public bool selling;
    public bool forceCantSell;
    public GameObject sellSymbol;
    public bool turnedOn;
    public Transform goToActivateWhenOn;
    public Rigidbody rb;

    private void Awake() {
        outline = model.GetComponent<Outline>();
        if (outline == null) outline = model.AddComponent<Outline>();
        outline.enabled = false;

        if (overlayCanvas == null) overlayCanvas = GetComponentInChildren<Canvas>();
        if (overlayCanvas) cg = overlayCanvas.GetComponent<CanvasGroup>();
    }

    public void Use() {
        playerUsing = playerHovering;
        switch(useTag) {
            case "Computer":
                transform.GetComponentInChildren<Computer>().StartUsingComputer(C.c.player[0]);
                break;
            case "Workbench":
                transform.GetComponentInChildren<WorkBench>().StartAssembling(C.c.player[0]);
                break;
            case "WorkbenchNoBreakdown":
                transform.GetComponentInChildren<WorkBench>().StartAssembling(C.c.player[0]);
                break;
            case "Storage":
                playerUsing = null;
                transform.GetComponentInChildren<Storage>().Open();
                break;
            case "Bed":
                transform.GetComponentInChildren<Bed>().Use(C.c.player[0]);
                break;
            case "Lamp":
                playerUsing = null;
                transform.GetComponentInChildren<TurnOn>().Use();
                break;
            case "Chair":
                transform.GetComponentInChildren<Chair>().Use(C.c.player[0]);
                break;
            default:
                playerUsing = null;
                break;
        }
    }

    public void UseSecondary() {
        playerUsing = playerHovering;
        switch (useTag) {
            case "Workbench":
                transform.GetComponentInChildren<WorkBench>().StartBreakingDown(C.c.player[0]);
                break;
            default:
                playerUsing = null;
                break;
        }
    }

    public void Hovering(Player p) {
        if (!placing && playerHovering == null && playerUsing == null) {
            playerHovering = p;
            if (outline) outline.enabled = true;
        }
    }

    private void Update() {
        HoverOutline();

        if (playerHovering && overlayCanvas && cg) {
            overlayCanvas.transform.parent.LookAt(playerHovering.transform);
            overlayCanvas.transform.parent.eulerAngles = new Vector3(0, overlayCanvas.transform.parent.eulerAngles.y, 0);
            cg.alpha += Time.deltaTime * 2;
        }
    }

    public void HoverOutline() {
        if (outline) {
            if (outline.enabled) {
                if (playerHovering) {
                    if (playerUsing || playerHovering.assetHovering != this) {
                        playerHovering = null;
                        outline.enabled = false;
                        if (overlayCanvas && cg) cg.alpha = 0;
                    }
                }
                else {
                    playerHovering = null;
                    outline.enabled = false;
                }
            }
        }
    }

    public void ToggleSelling() {
        if (forceCantSell) { return; }
        selling = !selling;
        if (selling) {
            C.c.currentShop.assetsSelling.Add(this);
            if (sellSymbol == null) {
                sellSymbol = Instantiate(C.c.data.prefabs[0]);
                sellSymbol.GetComponent<FloatingIcon>().follow = coll;
            }
        } else {
            C.c.currentShop.assetsSelling.Remove(this);
            if (sellSymbol) Destroy(sellSymbol);
        }
    }

    public void Set(AssetData assetData = null, string loadByName = "",bool isPlacing = false) {
        data = assetData;
        if (loadByName != "") { foreach(AssetData ad in C.c.data.assetData) if (ad.name == loadByName) { data = ad; break; } }
        if (model) { Destroy(model); }
        model = Instantiate(data.modelPrefab, transform);
        model.transform.localPosition = Vector3.zero;
        model.tag = "Asset";
        outline = model.GetComponent<Outline>();
        overlayCanvas = model.GetComponentInChildren<Canvas>();
        if (overlayCanvas) cg = overlayCanvas.GetComponent<CanvasGroup>();
        if (overlayCanvas && cg == null) cg = overlayCanvas.gameObject.AddComponent<CanvasGroup>();
        if (outline == null) outline = model.AddComponent<Outline>();
        coll = model.GetComponent<Collider>();
        camOverride = model.transform.Find("CamOverride");
        goToActivateWhenOn = model.transform.Find("ActivateWhenOn");
        assetName = data.name;
        useTag = data.useTag;
        physicsAsset = data.physicsAsset;
        if (physicsAsset) {
            rb = model.AddComponent<Rigidbody>();
            rb.mass = data.mass;
        }
        if (isPlacing) {
            coll.enabled = false;
            if (physicsAsset)
                rb.isKinematic = true;
        } else {
            coll.enabled = true;
            if (physicsAsset)
                rb.isKinematic = false;
        }
        
        name = assetName;
    }

    

}
