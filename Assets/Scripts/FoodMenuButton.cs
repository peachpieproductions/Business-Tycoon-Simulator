using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FoodMenuButton : MonoBehaviour {

    public PlayerUI pui;
    public DiningTable diningTable;
    public bool addNewMenuItemButton;
    public GameObject removeText;

    private void Start() {
        pui = GetComponentInParent<PlayerUI>();
        if (pui) {
            diningTable = pui.player.usingAsset.model.GetComponent<DiningTable>();
        }
    }

    public void OnHover() {
        if (!addNewMenuItemButton && !diningTable.addingMenuItem) removeText.SetActive(true);
        if (diningTable.addingMenuItem) {
            if (diningTable.playerUsing.inventory[transform.GetSiblingIndex()].amount > 0) pui.invRender.UpdateAssetPreview(diningTable.playerUsing.inventory[transform.GetSiblingIndex()].asset);
            else pui.invRender.UpdateAssetPreview(null);
        }
        else {
            if (!addNewMenuItemButton) pui.invRender.UpdateAssetPreview(C.c.currentShop.foodMenu[transform.GetSiblingIndex()]);
            else pui.invRender.UpdateAssetPreview(null);
        }
    }

    public void UnHover() {
        removeText.SetActive(false);
    }

    public void Clicked() {
        if (diningTable.addingMenuItem) {
            if (diningTable.playerUsing.inventory[transform.GetSiblingIndex()].amount > 0) {
                if (diningTable.playerUsing.inventory[transform.GetSiblingIndex()].asset.type == AssetData.Type.Food) {
                    diningTable.editMenuItemIndex = transform.GetSiblingIndex();
                    diningTable.SetNewMenuItem();
                }
            }
        }
        else {
            if (addNewMenuItemButton) {
                diningTable.AddMenuItem();
            }
            else {
                diningTable.editMenuItemIndex = transform.GetSiblingIndex();
                diningTable.RemoveMenuItem();
            }
        }
    }


}
