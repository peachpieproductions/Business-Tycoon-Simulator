using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour {

    public Asset asset;
    public List<AssetInventorySlot> inventory = new List<AssetInventorySlot>();

    private void Awake() {
        asset = GetComponentInParent<Asset>();
    }

    public void Open(Player p) {
        p.pui.invRender.OpenStorage(this);
    }

    public bool IsEmpty() {
        foreach(AssetInventorySlot slot in inventory) {
            if (slot.amount > 0) return false;
        }
        return true;
    }

    public bool AddAssetToInventory(AssetData asset, int amount = 1) {
        bool placedAsset = false;
        foreach (AssetInventorySlot slot in inventory) { //find existing asset stack
            if (slot.amount > 0 && slot.asset == asset) {
                slot.amount += amount;
                placedAsset = true;
                break;
            }
        }
        if (!placedAsset) {
            foreach (AssetInventorySlot slot in inventory) { //make new stack
                if (slot.amount == 0) {
                    slot.amount += amount;
                    slot.asset = asset;
                    placedAsset = true;
                    break;
                }
            }
        }
        return placedAsset;
    }

}
