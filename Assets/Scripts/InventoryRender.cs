using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using cakeslice;

public class InventoryRender : MonoBehaviour {

    public Transform itemsParent;
    public Transform invSlotParent;
    public List<Transform> items = new List<Transform>();
    public List<SpriteRenderer> invSlots = new List<SpriteRenderer>();
    public Color invSlotColor;
    public Color invSlotSelectedColor;

    private void Start() {
        for (int i = 0; i < 8; i++) {
            items.Add(itemsParent.GetChild(i));
            invSlots.Add(invSlotParent.GetChild(i).GetComponent<SpriteRenderer>());
        }
        UpdateInventoryRender();
    }

    private void Update() {

        itemsParent.GetChild(C.c.player[0].inventoryCurrentIndex).Rotate(0, .3f, 0);

    }

    public void UpdateInventoryRender() {

        for (int i = 0; i < 8; i++) {

            if (C.c.player[0].inventoryCurrentIndex == i) invSlots[i].color = invSlotSelectedColor;
                else invSlots[i].color = invSlotColor;

            for (int j = 0; j < items[i].childCount; j++) {
                Destroy(items[i].GetChild(j).gameObject);
            }

            if (C.c.player[0].inventory[i].amount > 0) {
                itemsParent.GetChild(i).rotation = Quaternion.identity;
                var inst = Instantiate(C.c.player[0].inventory[i].asset.modelPrefab,items[i]);
                inst.transform.localPosition = new Vector3 (0, C.c.player[0].inventory[i].asset.invModelYOffset, 0);
                inst.transform.localEulerAngles = new Vector3(10, 120, -15);
                inst.transform.localScale = C.c.player[0].inventory[i].asset.invModelScaleMultiplier * Vector3.one;
                var rb = inst.GetComponent<Rigidbody>();
                if (rb) rb.isKinematic = true;
                inst.layer = 9;
                Destroy(inst.GetComponent<Outline>());
            } 
        }

    }

}
