using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class InventoryRender : MonoBehaviour {

    public bool inventoryOpen;
    public bool storageOpen;
    public PlayerUI pui;
    public Player player;
    public Camera invCam;
    public Transform inventoryNull;
    public Transform mainInventory;
    public Transform hotbarItemsParent;
    public Transform hotbarSlotParent;
    public List<Transform> hotbarItems = new List<Transform>();
    public List<InventorySlotRender> hotbarSlots = new List<InventorySlotRender>();
    public Color invSlotColor;
    public Color invSlotSelectedColor;
    public Transform mainInvItemsParent;
    public Transform mainInvSlotsParent;
    public List<Transform> mainInvItems = new List<Transform>();
    public List<InventorySlotRender> mainInvSlots = new List<InventorySlotRender>();
    public Storage currentStorageObject;
    public Transform storageNull;
    public Transform storageItemsParent;
    public Transform storageSlotsParent;
    public List<Transform> storageItems = new List<Transform>();
    public List<InventorySlotRender> storageSlots = new List<InventorySlotRender>();

    public int currentSlotHoverId;
    public int currentSlotDraggedId;
    public float currentSlotDraggedYOffset;
    public Transform currentDraggedItem;

    public GameObject assetPreviewNull;
    public Transform assetPreviewItem;

    private void Start() {
        mainInventory.gameObject.SetActive(false);
        for (int i = 0; i < 8; i++) {
            hotbarItems.Add(hotbarItemsParent.GetChild(i));
            hotbarSlots.Add(hotbarSlotParent.GetChild(i).GetComponent<InventorySlotRender>());
        }
        for (int i = 0; i < 24; i++) {
            mainInvItems.Add(mainInvItemsParent.GetChild(i));
            mainInvSlots.Add(mainInvSlotsParent.GetChild(i).GetComponent<InventorySlotRender>());
        }
        for (int i = 0; i < 32; i++) {
            storageItems.Add(storageItemsParent.GetChild(i));
            storageSlots.Add(storageSlotsParent.GetChild(i).GetComponent<InventorySlotRender>());
        }
        UpdateInventoryRender();
    }

    private void Update() {

        var worldMousePos = invCam.ScreenToWorldPoint(Input.mousePosition);
        worldMousePos.z = 45;

        if (currentDraggedItem) {
            if (Input.GetMouseButton(0) || InputManager.AButtonHeld(player.playerId)) {
                currentDraggedItem.GetChild(0).position = worldMousePos + Vector3.up * currentSlotDraggedYOffset;
                currentDraggedItem.GetChild(0).Rotate(0,1,0,Space.World);
            }
            else {
                if (currentSlotHoverId != -1) {
                    AssetData swapAsset = null;
                    int swapAssetAmount = 0;
                    AssetInventorySlot hover;
                    if (currentSlotHoverId > 31) hover = currentStorageObject.inventory[currentSlotHoverId-32];
                    else hover = player.inventory[currentSlotHoverId];
                    AssetInventorySlot dragged;
                    if (currentSlotDraggedId > 31) dragged = currentStorageObject.inventory[currentSlotDraggedId-32];
                    else dragged = player.inventory[currentSlotDraggedId];

                    if (hover.amount > 0) {
                        swapAsset = hover.asset;
                        swapAssetAmount = hover.amount;
                    }
                    hover.asset = dragged.asset;
                    hover.amount = dragged.amount;
                    dragged.asset = swapAsset;
                    dragged.amount = swapAssetAmount;
                    UpdateInventoryRender();
                }
                else { 
                    if (currentSlotDraggedId < 32) currentDraggedItem.GetChild(0).localPosition = new Vector3(0, player.inventory[currentSlotDraggedId].asset.invModelYOffset, 0);
                    else currentDraggedItem.GetChild(0).localPosition = new Vector3(0, currentStorageObject.inventory[currentSlotDraggedId - 32].asset.invModelYOffset, 0);
                }
                currentDraggedItem.localScale = Vector3.one * .6f;
                currentDraggedItem = null;
                currentSlotDraggedId = -1;
            }
        }

        hotbarItemsParent.GetChild(C.c.player[0].inventoryCurrentIndex).Rotate(0, .3f, 0);
        assetPreviewItem.Rotate(0, .3f, 0, Space.World);

        if (inventoryOpen) {
            inventoryNull.localPosition = Vector3.Lerp(inventoryNull.localPosition, Vector3.up * 1.6f, Time.deltaTime * 12);
        } else {
            inventoryNull.localPosition = Vector3.Lerp(inventoryNull.localPosition, Vector3.zero, Time.deltaTime * 8);
            if (mainInventory.gameObject.activeSelf) { if (inventoryNull.localPosition.y < .1f) mainInventory.gameObject.SetActive(false); }
        }

    }

    public void OpenMainInventory(bool open) {
        inventoryOpen = open;
        currentSlotDraggedId = -1;
        if (open) mainInventory.gameObject.SetActive(true);
        UpdateInventoryRender();
    }

    public void OpenStorage(Storage storage) {
        if (!storageOpen) {
            if (!inventoryOpen) player.ToggleInventory();
            storageOpen = true;
            currentStorageObject = storage;
            storageNull.gameObject.SetActive(true);
        } else {
            if (inventoryOpen) player.ToggleInventory();
            storageOpen = false;
            currentStorageObject = null;
            storageNull.gameObject.SetActive(false);
        }
        UpdateInventoryRender();
    }

    public void CloseStorage() {
        if (storageOpen && currentStorageObject) {
            OpenStorage(currentStorageObject);
        }
    }

    public void StopAssetPreview() {
        assetPreviewNull.SetActive(false);
    }

    public void UpdateAssetPreview(AssetData data) {

        assetPreviewNull.SetActive(true);

        if (assetPreviewItem.childCount > 0) Destroy(assetPreviewItem.GetChild(0).gameObject);

        if (data) {
            var inst = Instantiate(data.modelPrefab, assetPreviewItem);
            inst.transform.localPosition = new Vector3(0, data.invModelYOffset, 0);
            inst.transform.localEulerAngles = new Vector3(10, 120, -15);
            inst.transform.localScale = data.invModelScaleMultiplier * inst.transform.localScale;
            var rb = inst.GetComponent<Rigidbody>();
            if (rb) rb.isKinematic = true;
            inst.layer = 9;
            foreach (Transform t in inst.transform) t.gameObject.layer = 9;
            Destroy(inst.GetComponent<Outline>());
        }
    }

    public void UpdateInventoryRender() {


        //Hotbar
        for (int i = 0; i < 8; i++) {

            if (C.c.player[0].inventoryCurrentIndex == i) hotbarSlots[i].spr.color = invSlotSelectedColor;
                else hotbarSlots[i].spr.color = invSlotColor;

            for (int j = 0; j < hotbarItems[i].childCount; j++) {
                Destroy(hotbarItems[i].GetChild(j).gameObject);
            }

            if (C.c.player[0].inventory[i].amount > 0) {
                hotbarItemsParent.GetChild(i).rotation = Quaternion.identity;
                var inst = Instantiate(C.c.player[0].inventory[i].asset.modelPrefab,hotbarItems[i]);
                inst.transform.localPosition = new Vector3 (0, C.c.player[0].inventory[i].asset.invModelYOffset, 0);
                inst.transform.localEulerAngles = new Vector3(10, 120 + C.c.player[0].inventory[i].asset.invModelRotOffset, -15);
                inst.transform.localScale = C.c.player[0].inventory[i].asset.invModelScaleMultiplier * inst.transform.localScale;
                var rb = inst.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;
                inst.layer = 9;
                foreach (Transform t in inst.transform) t.gameObject.layer = 9;
                Destroy(inst.GetComponent<Outline>());
            } 
        }

        //Inventory
        if (inventoryOpen) {

            for (int i = 0; i < 24; i++) {
                for (int j = 0; j < mainInvItems[i].childCount; j++) {
                    Destroy(mainInvItems[i].GetChild(j).gameObject);
                }

                if (C.c.player[0].inventory[i + 8].amount > 0) {
                    mainInvItemsParent.GetChild(i).rotation = Quaternion.identity;
                    var inst = Instantiate(C.c.player[0].inventory[i + 8].asset.modelPrefab, mainInvItems[i]);
                    inst.transform.localPosition = new Vector3(0, C.c.player[0].inventory[i + 8].asset.invModelYOffset, 0);
                    inst.transform.localEulerAngles = new Vector3(10, 120 + C.c.player[0].inventory[i + 8].asset.invModelRotOffset, -15);
                    inst.transform.localScale = C.c.player[0].inventory[i + 8].asset.invModelScaleMultiplier * inst.transform.localScale;
                    var rb = inst.GetComponent<Rigidbody>();
                    if (rb) rb.isKinematic = true;
                    inst.layer = 9;
                    foreach (Transform t in inst.transform) t.gameObject.layer = 9;
                    Destroy(inst.GetComponent<Outline>());
                }
            }
        }

        //Storage
        if (storageOpen) {

            for (int i = 0; i < 32; i++) {
                for (int j = 0; j < storageItems[i].childCount; j++) {
                    Destroy(storageItems[i].GetChild(j).gameObject);
                }

                if (i < currentStorageObject.inventory.Count) {
                    storageSlots[i].gameObject.SetActive(true);
                    if (currentStorageObject.inventory[i].amount > 0) {
                        storageItemsParent.GetChild(i).rotation = Quaternion.identity;
                        var inst = Instantiate(currentStorageObject.inventory[i].asset.modelPrefab, storageItems[i]);
                        inst.transform.localPosition = new Vector3(0, currentStorageObject.inventory[i].asset.invModelYOffset, 0);
                        inst.transform.localEulerAngles = new Vector3(10, 120 + currentStorageObject.inventory[i].asset.invModelRotOffset, -15);
                        inst.transform.localScale = currentStorageObject.inventory[i].asset.invModelScaleMultiplier * inst.transform.localScale;
                        var rb = inst.GetComponent<Rigidbody>();
                        if (rb) rb.isKinematic = true;
                        inst.layer = 9;
                        foreach (Transform t in inst.transform) t.gameObject.layer = 9;
                        Destroy(inst.GetComponent<Outline>());
                    }
                }
                else storageSlots[i].gameObject.SetActive(false);
            }

        }

    }

}
