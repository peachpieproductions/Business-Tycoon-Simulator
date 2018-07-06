using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour {

    public bool dumpster;
    public int timesUsed;
    public bool usedUp;

    public void Use() {
        if (usedUp) return;
        timesUsed++;
        if (dumpster) {
            if (timesUsed == 3) usedUp = true;
            if (Random.value < .5f) { //get asset
                if (Random.value < .2) { //get good asset
                    var foundAsset = C.c.GetRandomAsset(2);
                    C.c.player[0].pui.CreateInfoPopup("You Found " + foundAsset.name + "!",Color.white);
                    C.c.player[0].AddAssetToInventory(foundAsset);
                } else { //get regular asset
                    var foundAsset = C.c.GetRandomAsset(1);
                    C.c.player[0].pui.CreateInfoPopup("You Found " + foundAsset.name + "!", Color.white);
                    C.c.player[0].AddAssetToInventory(foundAsset);
                }
            } else C.c.player[0].pui.CreateInfoPopup("You didn't find anything of value.",Color.white);
        }
    }



}
