using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkBench : MonoBehaviour {

    public Asset asset;
    public Player playerUsing;
    public bool breakingDown;

    private void Start() {
        asset = GetComponentInParent<Asset>();
    }

    public void StartAssembling(Player p) {
        playerUsing = p;
        breakingDown = false;
        StartUsing();
    }
    public void StartBreakingDown(Player p) {
        playerUsing = p;
        breakingDown = true;
        StartUsing();
    }

    public void StartUsing() {
        playerUsing.usingAsset = asset;
        playerUsing.FreeCamToggle();
        playerUsing.freeCamFreeRot = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        playerUsing.pui.craftingPanel.SetActive(true);
    }

    private void Update() {

        if (playerUsing) {

            if(breakingDown) {

            } else {

            }

        }

    }


}
