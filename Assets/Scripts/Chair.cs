using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chair : MonoBehaviour {

    public Asset asset;
    public Player playerUsing;

    private void Start() {
        asset = GetComponentInParent<Asset>();
    }

    public void Use(Player p) {
        p.usingAsset = asset;
        p.FreeCamToggle();
        p.freeCamFreeRot = true;
        p.camOverridePos = asset.camOverride;
    }

}
