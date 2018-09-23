using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CraftingButton : MonoBehaviour {

    public PlayerUI pui;
    public WorkBench craftingTable;
    public AssetData craftBlueprintData;
    public int invSlotForBreakdown;

    private void Start() {
        pui = GetComponentInParent<PlayerUI>();
        if (pui) {
            craftingTable = pui.player.usingAsset.model.GetComponent<WorkBench>();
        }
    }

    public void HoverButton() {
        var index = transform.GetSiblingIndex();
        if (craftingTable) {
            if (!craftingTable.breakingDown) {
                if (craftBlueprintData == null && C.c.data.craftingBlueprintList.Count > 0) craftBlueprintData = C.c.data.craftingBlueprintList[index];
                if (craftBlueprintData) {
                    pui.invRender.UpdateAssetPreview(craftBlueprintData);
                    string txt = "";
                    foreach (AssetData a in craftBlueprintData.craftingMaterials) {
                        txt += "x1 " + a.name + "\n";
                    }
                    pui.craftingPanel.transform.Find("RequiredComponentsPanel").GetChild(0).GetComponent<TextMeshProUGUI>().text = txt;
                } else { pui.invRender.UpdateAssetPreview(null); }
            }
            else {
                if (pui.player.inventory[index].amount > 0) {
                    invSlotForBreakdown = index;
                    pui.invRender.UpdateAssetPreview(pui.player.inventory[index].asset);
                } else pui.invRender.UpdateAssetPreview(null);
            }
        } 
    }

    public void LogCraftingData() {
        if (craftingTable) craftingTable.Set(this);
    }


}
