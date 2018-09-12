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

    private void Update() {
        CurrentTimeText.text = C.c.timeString;

        MoneyText.text = "$" + player.money.ToString("F0");

        //DEBUG FIX
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

    public void GenerateCraftingList() {
        craftingPanel.SetActive(true);
        Button b = craftingPanel.GetComponentInChildren<Button>();
        foreach(Button butt in b.transform.parent.GetComponentsInChildren<Button>()) {
            if (butt == b) continue;
            Destroy(butt.gameObject);
        }
        if (C.c.data.craftingBlueprintList.Count > 0) {
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = C.c.data.craftingBlueprintList[0].name;
            foreach (AssetData d in C.c.data.craftingBlueprintList) {
                var newButton = Instantiate(b, b.transform.parent);
                newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.name;
            }
            Destroy(b.gameObject);
        } else {
            b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "No Crafting Recipies";
        }
        
        
    }

}
