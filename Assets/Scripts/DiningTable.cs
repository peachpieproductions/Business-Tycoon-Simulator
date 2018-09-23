using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DiningTable : MonoBehaviour {

    public Asset asset;
    public List<DiningSeat> diningSeats = new List<DiningSeat>();
    public Player playerUsing;
    internal bool addingMenuItem;
    internal int editMenuItemIndex;

    [System.Serializable]
    public class DiningSeat {
        public Transform seatTrans;
        public Transform plateNull;
        public NPC npcUsing;
        public AssetData ordered;
        public Transform orderedModel;
        public bool hasBeenServed;
        public bool finishedEating;
        public bool dirty;
    }

    private void Start() {
        asset = GetComponentInParent<Asset>();
    }

    private void Update() {
        
        for (int i = 0; i < diningSeats.Count; i++) {
            if (diningSeats[i].npcUsing && diningSeats[i].ordered != null && !diningSeats[i].hasBeenServed) {
                diningSeats[i].orderedModel.Rotate(0, 50 * Time.deltaTime, 0, Space.World);
            }
        }

    }

    public void Use(Player p) {
        playerUsing = p;
        OpenFoodMenu();
        playerUsing.pui.invRender.UpdateAssetPreview(null);
        playerUsing.usingAsset = asset;
        playerUsing.SetCameraMode(false);
    }

    public void ResetSeat(DiningSeat seat) {
        seat.npcUsing = null;
        seat.ordered = null;
        seat.hasBeenServed = false;
        seat.finishedEating = false;
        if (seat.orderedModel) Destroy(seat.orderedModel.gameObject);
        seat.orderedModel = null;
        if (!seat.dirty) C.c.currentShop.availableDiningSeats.Add(seat);
    }

    public void Serve(Player p) {
        foreach(DiningSeat seat in diningSeats) {
            if (seat.dirty) {
                seat.plateNull.GetChild(1).gameObject.SetActive(false);
                seat.plateNull.GetChild(0).gameObject.SetActive(true);
                seat.plateNull.gameObject.SetActive(false);
                seat.dirty = false;
                if (seat.npcUsing == null) C.c.currentShop.availableDiningSeats.Add(seat);
            }
            else if (seat.ordered != null && !seat.hasBeenServed) {
                foreach(AssetInventorySlot slot in p.inventory) {
                    if (slot.amount > 0 && slot.asset == seat.ordered) {
                        seat.npcUsing.owedAmount += seat.ordered.currentValue;
                        slot.amount--;
                        p.pui.invRender.UpdateInventoryRender();
                        seat.hasBeenServed = true;
                        if (!seat.ordered.liquid) {
                            seat.plateNull.gameObject.SetActive(true);
                        }
                        seat.orderedModel.localScale = seat.ordered.modelPrefab.transform.localScale;
                        seat.orderedModel.localEulerAngles = new Vector3(0, 0, 0);
                        var coll = seat.orderedModel.gameObject.AddComponent<BoxCollider>();
                        seat.orderedModel.transform.position = seat.plateNull.position + new Vector3(0,seat.orderedModel.transform.position.y - (coll.bounds.center.y - coll.bounds.extents.y), 0);
                        if (seat.ordered.liquid) seat.orderedModel.localPosition += Vector3.right * .375f;
                        else seat.orderedModel.position += Vector3.up * .02f;
                        Destroy(coll);
                    }
                }
            }
        }
    }

    public bool isBeingUsed() {
        bool beingUsed = false;
        foreach (DiningSeat seat in diningSeats) {
            if (seat.npcUsing) beingUsed = true;
        }
        return beingUsed;
    }

    public void PickUpTable() {
        foreach (DiningSeat seat in diningSeats) {
            C.c.currentShop.availableDiningSeats.Remove(seat);
        }
    }

    public void OpenFoodMenu() {
        playerUsing.pui.foodMenu.SetActive(true);
        addingMenuItem = false;
        //playerUsing.pui.UpdateCraftingPanelScroll(true);
        playerUsing.pui.foodMenu.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "FOOD MENU";
        Button b = playerUsing.pui.foodMenu.GetComponentInChildren<Button>();
        b.GetComponent<FoodMenuButton>().addNewMenuItemButton = false;
        foreach (Button butt in b.transform.parent.GetComponentsInChildren<Button>()) {
            if (butt == b) continue;
            Destroy(butt.gameObject);
        }
        foreach (AssetData d in C.c.currentShop.foodMenu) {
            var newButton = Instantiate(b, b.transform.parent);
            newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.name;
        }
        b.transform.SetAsLastSibling();
        b.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Add a New Menu Item...";
        b.GetComponent<FoodMenuButton>().addNewMenuItemButton = true;
        b.GetComponent<FoodMenuButton>().UnHover();

    }

    public void AddMenuItem() {
        //playerUsing.pui.UpdateCraftingPanelScroll(true);
        addingMenuItem = true;
        playerUsing.pui.foodMenu.SetActive(true);
        playerUsing.pui.foodMenu.transform.Find("Title").GetComponent<TextMeshProUGUI>().text = "Add a New Menu Item...";
        Button b = playerUsing.pui.foodMenu.GetComponentInChildren<Button>();
        b.GetComponent<FoodMenuButton>().addNewMenuItemButton = false;
        foreach (Button butt in b.transform.parent.GetComponentsInChildren<Button>()) {
            if (butt == b) continue;
            Destroy(butt.gameObject);
        }
        foreach (AssetInventorySlot d in playerUsing.inventory) {
            if (d.amount > 0) {
                var newButton = Instantiate(b, b.transform.parent);
                newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = d.asset.name;
            }
            else {
                var newButton = Instantiate(b, b.transform.parent);
                newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "(EMPTY)";
            }
        }
        Destroy(b.gameObject);
    }

    public void SetNewMenuItem() {
        if (playerUsing.inventory[editMenuItemIndex].amount > 0) {
            if (!C.c.currentShop.foodMenu.Contains(playerUsing.inventory[editMenuItemIndex].asset)) {
                C.c.currentShop.foodMenu.Add(playerUsing.inventory[editMenuItemIndex].asset);
            }
        }
        OpenFoodMenu();
    }

    public void RemoveMenuItem() {
        C.c.currentShop.foodMenu.RemoveAt(editMenuItemIndex);
        OpenFoodMenu();
    }



}
