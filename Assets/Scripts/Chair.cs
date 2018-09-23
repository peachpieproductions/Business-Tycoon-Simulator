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
        p.SetCameraMode(false, false, asset.camOverride,true);
    }

}
