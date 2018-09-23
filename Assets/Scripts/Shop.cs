using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    public Vector3 boxCastOffset;
    public Vector3 boxCastSize;
    public List<Asset> assetsSelling;
    public List<AssetData> foodMenu = new List<AssetData>();
    public List<DiningTable.DiningSeat> availableDiningSeats = new List<DiningTable.DiningSeat>();
    public List<NPC> npcCheckoutLine;
    public Register register;
    public Computer computer;
    public GameObject currentShop;
    

    private void Start() {
        var coll = GetComponent<BoxCollider>();
        boxCastOffset = coll.center;
        boxCastSize = coll.size;
        UpdateShop();
    }

    public void UpdateShop() {

        assetsSelling.Clear();
        availableDiningSeats.Clear();
        var hits = Physics.BoxCastAll(transform.position + boxCastOffset, boxCastSize * .5f, transform.forward);
        foreach (RaycastHit t in hits) {
            if (register == null) { register = t.transform.GetComponent<Register>(); }
            if (computer == null) { computer = t.transform.GetComponent<Computer>(); }
            if (computer && computer.shopController == null) computer.shopController = this;
            var a = t.transform.GetComponentInParent<Asset>();
            if (a) {
                if (a.selling) assetsSelling.Add(a);
            }
            if (t.transform.GetComponent<DiningTable>()) {
                foreach(DiningTable.DiningSeat seat in t.transform.GetComponent<DiningTable>().diningSeats) {
                    availableDiningSeats.Add(seat);
                }
            }
        }

    }






}
