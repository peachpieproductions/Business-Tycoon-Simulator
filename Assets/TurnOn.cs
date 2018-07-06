using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOn : MonoBehaviour {


    public Asset asset;
    public Player playerUsing;

    private void Start() {
        asset = GetComponentInParent<Asset>();
    }

    public void Use() {
        if (asset.goToActivateWhenOn) {
            asset.goToActivateWhenOn.gameObject.SetActive(!asset.goToActivateWhenOn.gameObject.activeSelf);
            asset.turnedOn = asset.goToActivateWhenOn.gameObject.activeSelf;
        }
    }



}
