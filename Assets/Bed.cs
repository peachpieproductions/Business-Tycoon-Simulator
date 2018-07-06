using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour {

    public Asset asset;
    public Player playerUsing;

    private void Start() {
        asset = GetComponentInParent<Asset>();
    }

    public void Use(Player p) {
        //Cursor.lockState = CursorLockMode.None;
        p.usingAsset = asset;
        p.FreeCamToggle();
        p.camOverridePos = asset.camOverride;
        p.StartCoroutine(p.Sleep());
    }



}
