using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {

    public Vector3 boxCastOffset;
    public Vector3 boxCastSize;
    public List<Asset> assetsSelling;
    public List<NPC> npcCheckoutLine;
    public Computer computer;

    private void Start() {
        var coll = GetComponent<BoxCollider>();
        boxCastOffset = coll.center;
        boxCastSize = coll.size;
        UpdateShop();
    }

    public void UpdateShop() {

        assetsSelling.Clear();
        var hits = Physics.BoxCastAll(transform.position + boxCastOffset, boxCastSize * .5f, transform.forward);
        foreach (RaycastHit t in hits) {
            var a = t.transform.GetComponentInParent<Asset>();
            if (a) {
                if (computer == null) { computer = t.transform.GetComponent<Computer>(); }
                if (a.selling) assetsSelling.Add(a);
            }
        }

    }






}
