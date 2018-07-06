using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;

public class NPC : MonoBehaviour {

    Animator anim;
    public NPCData data;
    public NavMeshAgent agent;
    public Asset interestedInAsset;
    public bool readyToCheckout;
    public bool goingHome;
    public bool goingToShop;
    public bool walkingToDestination;
    public bool atShop;
    public List<AssetInventorySlot> inventory = new List<AssetInventorySlot>();

    private void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
    }

    public void UpdateInWaitingLine() {
        if (C.c.currentShop.npcCheckoutLine.IndexOf(this) == 0) agent.SetDestination(C.c.currentShop.computer.transform.position + -C.c.currentShop.computer.transform.forward);
        else agent.SetDestination(C.c.currentShop.npcCheckoutLine[C.c.currentShop.npcCheckoutLine.IndexOf(this) - 1].transform.position);
        walkingToDestination = true;
    }

    public void Set(NPCData d) {
        data = d;
        if (transform.childCount > 0) {
            Destroy(transform.GetChild(0).gameObject);
            var model = Instantiate(d.npcModelPrefab, transform);
            model.transform.localPosition = Vector3.zero;
        }
    }

    public void GoHome() {
        goingHome = true;
        readyToCheckout = false;
        agent.SetDestination(data.home.position);
        walkingToDestination = true;
    }

    public void OverrideLookAt(Transform lookAt) {
        transform.forward = Vector3.Slerp(transform.forward, lookAt.position - transform.position, Time.deltaTime * 4);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    private void Update() {

        if (inventory[0].amount > 0 && !readyToCheckout && !goingHome) {
            if (Random.value < .005f) {
                readyToCheckout = true;
                C.c.currentShop.npcCheckoutLine.Add(this);
                UpdateInWaitingLine();
                return;
            }
        }

        transform.GetComponentInChildren<TextMeshPro>().text = inventory[0].amount.ToString();

        if (!walkingToDestination) {

            if (readyToCheckout) {
                if (Random.value < .01f) UpdateInWaitingLine();
                if (C.c.currentShop.npcCheckoutLine.Count > 0 && C.c.currentShop.npcCheckoutLine[0] == this) {
                    OverrideLookAt(C.c.player[0].transform);
                    if (C.c.currentShop.computer.assetBeingSoldModel.childCount == 0) {
                        var inst = Instantiate(inventory[0].asset.modelPrefab, C.c.currentShop.computer.assetBeingSoldModel);
                        var rb = inst.GetComponent<Rigidbody>(); if (rb) rb.isKinematic = true;
                        inst.transform.localPosition = Vector3.zero;
                        inst.transform.localScale *= inventory[0].asset.miniModelScaleMultiplier;
                    }
                }
            }

            else { //Shopping
                if (interestedInAsset) {
                    if (!C.c.currentShop.assetsSelling.Contains(interestedInAsset)) { interestedInAsset = null; return; } //cancel if taken not selling anymore
                    if (Random.value < .01f) { //get item
                        AddAssetToInventory(interestedInAsset.data);
                        C.c.currentShop.assetsSelling.Remove(interestedInAsset);
                        Destroy(interestedInAsset.gameObject);
                        interestedInAsset = null;
                    }
                    else if (Random.value < .01f) interestedInAsset = null; //lose interest in item
                    }
                else {
                    if (atShop && C.c.currentShop.assetsSelling.Count > 0) {
                        if (Random.value < .0025f) {
                            interestedInAsset = C.c.currentShop.assetsSelling[Random.Range(0, C.c.currentShop.assetsSelling.Count)];
                            agent.SetDestination(interestedInAsset.transform.position);
                            walkingToDestination = true;
                        }
                    } else {
                        goingToShop = true;
                        agent.SetDestination(C.c.currentShop.transform.position + new Vector3 (Random.Range(-C.c.currentShop.boxCastSize.x * .5f, C.c.currentShop.boxCastSize.x * .5f),0,
                            Random.Range(-C.c.currentShop.boxCastSize.z * .5f, C.c.currentShop.boxCastSize.z * .5f)));
                        walkingToDestination = true;
                    }
                }
                //go home without anything
                /*if (atShop) {
                    if (inventory[0].amount == 0) {
                        if (Random.value < .0025f) {
                            GoHome();
                        }
                    }
                }*/
            }
        } else { //Walking Somewhere
            if (!agent.pathPending) {
                if (Vector3.Distance(transform.position, agent.destination) < 1f) {
                    walkingToDestination = false;
                    if (goingHome) {
                        goingHome = false;
                        data.isHome = true;
                        C.c.data.npcsOut.Remove(this);
                        C.c.data.npcsAtHome.Add(this);
                        gameObject.SetActive(false);
                    }
                    if (goingToShop) {
                        goingToShop = false;
                        atShop = true;
                    }
                }
            }
        }

        anim.SetFloat("Speed", agent.velocity.magnitude);
    }

    public bool AddAssetToInventory(AssetData asset, int amount = 1) {
        bool placedAsset = false;
        foreach (AssetInventorySlot slot in inventory) { //find existing asset stack
            if (slot.amount > 0 && slot.asset == asset) {
                slot.amount += amount;
                placedAsset = true;
                break;
            }
        }
        if (!placedAsset) {
            foreach (AssetInventorySlot slot in inventory) { //make new stack
                if (slot.amount == 0) {
                    slot.amount += amount;
                    slot.asset = asset;
                    placedAsset = true;
                    break;
                }
            }
        }
        return placedAsset;
    }

}
