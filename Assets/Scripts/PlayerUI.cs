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

}
