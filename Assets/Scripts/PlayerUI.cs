﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

    public Player player;

    public InventoryRender invRender;
    public TextMeshProUGUI CurrentAssetText;
    public TextMeshProUGUI CurrentTimeText;
    public TextMeshProUGUI MoneyText;
    public GameObject infoPopup;
    public CanvasGroup blackOutPanel;
    public TextMeshProUGUI modeStatusText;
    public GameObject craftingPanel;
    public RectTransform craftingPanelButtonsNull;

    private void Update() {
        CurrentTimeText.text = C.c.timeString;

        MoneyText.text = "$" + player.money.ToString("F0");

        if (craftingPanel.activeSelf) {
            if (InputManager.Scroll(player.playerId) != 0 && craftingPanelButtonsNull.childCount > 7) {
                UpdateCraftingPanelScroll();
            }
        }

        //Current selected hotbar item text
        if (C.c.player[0].inventory[C.c.player[0].inventoryCurrentIndex].amount > 0)
            CurrentAssetText.text = C.c.player[0].inventory[C.c.player[0].inventoryCurrentIndex].asset.name + " (" + C.c.player[0].inventory[C.c.player[0].inventoryCurrentIndex].amount+")";
        else CurrentAssetText.text = "";

    }

    public void CreateInfoPopup(string str, Color col, float time = 6f) {
        var inst = Instantiate(infoPopup, infoPopup.transform.parent);
        inst.SetActive(true);
        inst.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = str;
        inst.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = col;
        inst.GetComponent<FadeOut>().delay = time;
    }

    public void UpdateCraftingPanelScroll(bool reset = false) {
        var pos = craftingPanelButtonsNull.anchoredPosition;
        pos.y = Mathf.Clamp(pos.y - InputManager.Scroll(player.playerId) * 74, -50, -50 + (craftingPanelButtonsNull.childCount - 6) * 74);
        if (reset) pos.y = -50;
        craftingPanelButtonsNull.anchoredPosition = pos;
    }

    public void GenerateCraftingList() {
        UpdateCraftingPanelScroll(true);
        WorkBench station = player.usingAsset.model.GetComponent<WorkBench>();
        craftingPanel.SetActive(true);
        craftingPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Assembly Blueprints";
        Button b = craftingPanel.GetComponentInChildren<Button>();
        foreach(Button butt in b.transform.parent.GetComponentsInChildren<Button>()) {
            if (butt == b) continue;
            Destroy(butt.gameObject);
        }
        if (station.blueprintsUnlocked) {
            foreach (AssetData d in C.c.data.assetData) {
                if (d.workbenchNeeded != station.type || d.craftingMaterials.Count == 0) continue;
                var newButton = Instantiate(b, b.transform.parent);
                if (d.craftingOutput == 1) newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.name;
                else newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.name + " (" + d.craftingOutput + ")";
                newButton.GetComponent<CraftingButton>().craftBlueprintData = d;
            }
            Destroy(b.gameObject);
        }
        else if (C.c.data.craftingBlueprintList.Count > 0) {
            foreach (AssetData d in C.c.data.craftingBlueprintList) {
                if (d.workbenchNeeded != station.type) continue;
                var newButton = Instantiate(b, b.transform.parent);
                if (d.craftingOutput == 1) newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.name;
                else newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.name + " (" + d.craftingOutput + ")";
            }
            Destroy(b.gameObject);
        } else {
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "No Assembly Blueprints";
        }
    }

    public void GenerateBreakdownList() {
        UpdateCraftingPanelScroll(true);
        craftingPanel.SetActive(true);
        craftingPanel.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Inventory List";
        Button b = craftingPanel.GetComponentInChildren<Button>();
        foreach (Button butt in b.transform.parent.GetComponentsInChildren<Button>()) {
            if (butt == b) continue;
            Destroy(butt.gameObject);
        }
        foreach (AssetInventorySlot d in player.inventory) {
            if (d.amount > 0) {
                var newButton = Instantiate(b, b.transform.parent);
                newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.asset.name + " (" + d.amount + ")";
            } else {
                var newButton = Instantiate(b, b.transform.parent);
                newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "(EMPTY)";
            }
        }
        Destroy(b.gameObject);
    }

}
