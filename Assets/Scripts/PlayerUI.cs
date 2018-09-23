using System.Collections;
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
    public GameObject foodMenu;
    public GameObject crosshair;

    private void Awake() {
        invRender.player = player;
        invRender.pui = this;
    }

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

    public void SetUI(bool resetUI,bool showCrosshair = true) {
        if (resetUI) {
            craftingPanel.SetActive(false);
            foodMenu.SetActive(false);
            invRender.StopAssetPreview();
        }
        crosshair.SetActive(showCrosshair);
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

    

    

}
