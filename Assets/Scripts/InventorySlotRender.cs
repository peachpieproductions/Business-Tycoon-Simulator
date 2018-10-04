using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InventorySlotRender : MonoBehaviour {

    public InventoryRender invRender;
    public Player player;
    public Storage storage;
    public int slotId;
    public Transform slotRenderItem;
    public SpriteRenderer spr;
    public TextMeshPro amountText;

    private void Awake() {
        spr = GetComponent<SpriteRenderer>();
        invRender = GetComponentInParent<InventoryRender>();
        player = invRender.player;
        if (transform.parent == invRender.hotbarSlotParent) {
            slotId = transform.GetSiblingIndex();
            slotRenderItem = invRender.hotbarItemsParent.GetChild(slotId);
        }
        else if (transform.parent == invRender.mainInvSlotsParent) {
            slotId = transform.GetSiblingIndex() + 8;
            slotRenderItem = invRender.mainInvItemsParent.GetChild(slotId - 8);
        }
        CreateAmountText();
    }

    private void OnEnable() {
        if (transform.parent == invRender.storageSlotsParent) {
            slotId = transform.GetSiblingIndex() + 32;
            slotRenderItem = invRender.storageItemsParent.GetChild(slotId - 32);
        }
        if (slotId > 31) storage = invRender.currentStorageObject;
    }

    private void Start() {
        player = invRender.player;
    }

    private void OnMouseDown() {

        if (Input.GetKey(KeyCode.LeftShift) && invRender.storageOpen) {
            if (storage) {
                if (player.AddAssetToInventory(invRender.currentStorageObject.inventory[slotId - 32].asset, invRender.currentStorageObject.inventory[slotId - 32].amount)) {
                    invRender.currentStorageObject.inventory[slotId - 32].amount = 0;
                    invRender.currentStorageObject.inventory[slotId - 32].asset = null;
                }
            } else {
                if (invRender.currentStorageObject.AddAssetToInventory(player.inventory[slotId].asset, player.inventory[slotId].amount)) {
                    player.inventory[slotId].amount = 0;
                    player.inventory[slotId].asset = null;
                }
            }
            invRender.UpdateInventoryRender();
        }
        else {
            AssetInventorySlot slot;
            if (storage) {
                slot = storage.inventory[slotId - 32];
            }
            else {
                slot = player.inventory[slotId];
            }
            if (slot.amount > 0) {
                amountText.gameObject.SetActive(false);
                invRender.currentDraggedItem = slotRenderItem;
                invRender.currentSlotDraggedId = slotId;
                slotRenderItem.localScale = Vector3.one * .8f;
                var coll = slotRenderItem.GetChild(0).gameObject.AddComponent<BoxCollider>();
                invRender.currentSlotDraggedYOffset = slotRenderItem.GetChild(0).transform.position.y - coll.bounds.center.y;
                Destroy(coll);
            }
        }
    }

    private void OnMouseEnter() {
        invRender.currentSlotHoverId = slotId;
    }

    private void OnMouseExit() {
        invRender.currentSlotHoverId = -1;
    }

    public void CreateAmountText() {
        var inst = Instantiate(invRender.amountTextPrototype, transform);
        inst.transform.localPosition = Vector3.forward * -6;
        amountText = inst.GetComponent<TextMeshPro>();
    }






}
