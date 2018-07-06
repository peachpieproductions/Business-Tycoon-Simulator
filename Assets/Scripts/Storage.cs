using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : MonoBehaviour {


    public List<AssetInventorySlot> inventory = new List<AssetInventorySlot>();

    public void Open() {
        foreach(AssetInventorySlot slot in inventory) {
            if (slot.asset) {
                C.c.player[0].AddAssetToInventory(slot.asset, slot.amount);
            }
        }
        inventory.Clear();
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
