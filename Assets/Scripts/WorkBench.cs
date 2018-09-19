using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum WorkbenchType { Workbench, FoodPrepTable, Grill, Oven, Toaster, Fryer }

public class WorkBench : MonoBehaviour {

    public WorkbenchType type;  
    public Asset asset;
    public Player playerUsing;
    public bool breakingDown;
    public Canvas workshopCanvas;
    public Image progressBar;
    public bool blueprintsUnlocked;
    public AssetData.Type includedBlueprints;
    bool working;
    float workTimer;
    float workTimerTotal;
    int breakdownIndex;
    AssetData craftData;

    private void Start() {
        asset = GetComponentInParent<Asset>();
    }

    public void StartAssembling(Player p) {
        playerUsing = p;
        StartUsing();
        if (workTimer == 0) {
            breakingDown = false;
            playerUsing.pui.GenerateCraftingList();
        }
    }
    public void StartBreakingDown(Player p) {
        playerUsing = p;
        StartUsing();
        if (workTimer == 0) {
            breakingDown = true;
            playerUsing.pui.GenerateBreakdownList();
        }
    }

    public void StartUsing() {
        playerUsing.usingAsset = asset;
        playerUsing.FreeCamToggle();
        playerUsing.freeCamFreeRot = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Set(CraftingButton cb) {
        
        if (breakingDown) {
            if (playerUsing.inventory[cb.invSlotForBreakdown].asset.breakdownMaterials.Count > 0) {
                if (playerUsing.inventory[cb.invSlotForBreakdown].amount > 0) {
                    breakdownIndex = cb.invSlotForBreakdown;
                    working = true;
                    workTimerTotal = 3f;
                }
            }
        }
        else {
            craftData = cb.craftBlueprintData;
            bool canCraft = true;
            List<AssetInventorySlot> inventoryCopy = new List<AssetInventorySlot>();
            foreach(AssetInventorySlot slot in playerUsing.inventory) {
                var clone = slot.Clone();
                inventoryCopy.Add(clone);
            }
            foreach(AssetData a in craftData.craftingMaterials) {
                bool foundAsset = false;
                foreach(AssetInventorySlot s in inventoryCopy) {
                    if (s.asset == a && s.amount > 0) {
                        s.amount--;
                        foundAsset = true;
                    }
                }
                if (!foundAsset) {
                    canCraft = false;
                    break;
                }
            }
            if (canCraft) {
                working = true;
                workTimerTotal = Mathf.Max(3,craftData.baseValue * .6f);
            }
        }

        if (working) {
            workshopCanvas.gameObject.SetActive(true);
            playerUsing.pui.craftingPanel.SetActive(false);
        }
    }

    private void Update() {

        if (playerUsing && playerUsing.usingAsset != asset) playerUsing = null;
        if (playerUsing) {
            
            if (working) {

                if (workTimer < workTimerTotal) {
                    workTimer += Time.deltaTime;
                    progressBar.rectTransform.localScale = new Vector3(workTimer / workTimerTotal, 1, 1);
                }
                if (workTimer >= workTimerTotal) { //finished work
                    workshopCanvas.gameObject.SetActive(false);
                    playerUsing.pui.craftingPanel.SetActive(true);
                    if (breakingDown) { //Break down
                        var ass = playerUsing.inventory[breakdownIndex].asset;
                        playerUsing.inventory[breakdownIndex].amount--;
                        foreach (AssetData a in ass.breakdownMaterials) {
                            playerUsing.AddAssetToInventory(a);
                        }
                        if (!C.c.data.craftingBlueprintList.Contains(ass) && ass.craftingMaterials.Count > 0) C.c.data.craftingBlueprintList.Add(ass);
                        playerUsing.pui.invRender.UpdateInventoryRender();
                        playerUsing.pui.GenerateBreakdownList();
                        working = false;
                        workTimer = 0;
                    }
                    else { //Craft
                        foreach (AssetData a in craftData.craftingMaterials) {
                            foreach (AssetInventorySlot s in playerUsing.inventory) {
                                if (s.asset == a) {
                                    s.amount--;
                                    break;
                                }
                            }
                        }
                        playerUsing.AddAssetToInventory(craftData,craftData.craftingOutput);
                        playerUsing.pui.invRender.UpdateInventoryRender();
                        working = false;
                        workTimer = 0;
                    }

                }
            }

        }

    }


}
