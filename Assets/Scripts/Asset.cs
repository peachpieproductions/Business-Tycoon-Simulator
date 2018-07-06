using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

[System.Serializable]
public class AssetInventorySlot {

    public AssetData asset;
    public int amount;

}

public class Asset : MonoBehaviour {

    public AssetData data;
    public string assetName;
    public string useTag;
    public Player playerHovering;
    public GameObject model;
    public Collider coll;
    public Outline outline;
    public bool placed;
    public Transform camOverride;
    public bool physicsAsset;
    public bool selling;
    public GameObject sellSymbol;
    public bool turnedOn;
    public Transform goToActivateWhenOn;

    public void Use() {
        switch(useTag) {
            case "Computer":
                transform.GetComponentInChildren<Computer>().StartUsingComputer(C.c.player[0]);
                break;
            case "Storage":
                transform.GetComponentInChildren<Storage>().Open();
                break;
            case "Bed":
                transform.GetComponentInChildren<Bed>().Use(C.c.player[0]);
                break;
            case "Lamp":
                transform.GetComponentInChildren<TurnOn>().Use();
                break;
            case "Chair":
                transform.GetComponentInChildren<Chair>().Use(C.c.player[0]);
                break;
        }
    }

    public void Hovering(Player p) {
        if (placed && playerHovering == null) {
            playerHovering = p;
            outline.enabled = true;
        }
    }

    private void Update() {
        HoverOutline();
    }

    public void HoverOutline() {
        if (outline.enabled) {
            if (playerHovering) {
                if (playerHovering.assetHovering != this) {
                    playerHovering = null;
                    outline.enabled = false;
                }
            }
            else {
                playerHovering = null;
                outline.enabled = false;
            }
        }
    }

    public void ToggleSelling() {
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

    public void Set(AssetData assetData = null, string loadByName = "",bool placing = false) {
        data = assetData;
        if (loadByName != "") { foreach(AssetData ad in C.c.data.assetData) if (ad.name == loadByName) { data = ad; break; } }
        if (model) { Destroy(model); }
        model = Instantiate(data.modelPrefab, transform);
        model.transform.localPosition = Vector3.zero;
        outline = model.GetComponent<Outline>();
        coll = model.GetComponent<Collider>();
        camOverride = model.transform.Find("CamOverride");
        goToActivateWhenOn = model.transform.Find("ActivateWhenOn");
        assetName = data.name;
        useTag = data.useTag;
        physicsAsset = data.physicsAsset;
        if (placing) {
            coll.enabled = false;
            if (physicsAsset)
                GetComponentInChildren<Rigidbody>().isKinematic = true;
        } else {
            coll.enabled = placed || physicsAsset;
            if (physicsAsset)
                GetComponentInChildren<Rigidbody>().isKinematic = placed;
        }
        
        name = assetName;
    }

    private void Start() {
        if (outline) outline.enabled = false;
    }

}
