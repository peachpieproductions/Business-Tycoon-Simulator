﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using TMPro;
using UnityEngine.UI;

public class NPC : MonoBehaviour {

    Animator anim;
    Player player;
    public NPCData data;
    public List<AssetInventorySlot> inventory = new List<AssetInventorySlot>();
    public NavMeshAgent agent;
    public Asset interestedInAsset;
    public bool readyToCheckout;
    public bool goingHome;
    public bool goingToShop;
    public bool walkingToDestination;
    public bool atShop;
    public bool dining;
    public float owedAmount;
    public float timeSpentWaiting;
    public DiningTable.DiningSeat diningSeat;
    public DiningTable diningTable;
    public float loopDelay = .5f;

    [Header("UI")]
    public TextMeshProUGUI nameText;
    public Transform statusParent;
    public GameObject interactPanel;
    [HideInInspector] public CanvasGroup uiPopup;

    bool coroutineInteractActive;

    private void Start() {
        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        uiPopup = GetComponentInChildren<CanvasGroup>();
        uiPopup.alpha = 0;
        player = C.c.player[0];
    }

    private void OnEnable() {
        StartCoroutine(AILoop());
    }

    public void Set(NPCData d) {
        data = d;
        if (transform.childCount > 0) {
            Destroy(transform.GetChild(0).gameObject);
            var model = Instantiate(d.npcModelPrefab, transform);
            model.transform.localPosition = Vector3.zero;
        }
    }

    private void Update() {

        if (walkingToDestination) { //Walking Somewhere
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
                    else if (dining) {
                        if (diningSeat.seatTrans == null || diningSeat.npcUsing) { dining = false; return; }
                        transform.position = diningSeat.seatTrans.position;
                        transform.rotation = diningSeat.seatTrans.rotation;
                        diningTable = diningSeat.seatTrans.GetComponentInParent<DiningTable>();
                        diningSeat.npcUsing = this;
                        anim.SetBool("Sitting", true);
                    }
                    else if (goingToShop) {
                        goingToShop = false;
                        atShop = true;
                    }
                }
            }
        }
        else {

            if (dining) {
                transform.position = diningSeat.seatTrans.position;
                transform.rotation = diningSeat.seatTrans.rotation;
            }
            if (readyToCheckout) {
                if (C.c.currentShop.npcCheckoutLine.Count > 0 && C.c.currentShop.npcCheckoutLine[0] == this) {
                    OverrideLookAt(C.c.player[0].transform);
                }
            }

        }

        anim.SetFloat("Speed", agent.velocity.magnitude);

    }

    public IEnumerator InteractWithAI() {

        if (coroutineInteractActive) yield break;
        else coroutineInteractActive = true;

        UpdateUI();

        while(player.lookingAtNpc == this && player.interactingWithNpc == null) {
            if (uiPopup.alpha < 1) uiPopup.alpha += Time.deltaTime * 10f;
            UILookAtPlayer(player);

            if (InputManager.InteractInput(0)) {
                interactPanel.gameObject.SetActive(true);
                player.interactingWithNpc = this;
                uiPopup.alpha = 1;
                agent.isStopped = true;
            }

            yield return null;
        }

        while (player.interactingWithNpc == this) {
            if (InputManager.Cancel(0)) {
                player.interactingWithNpc = null;
            }
            if (InputManager.JumpInput(0)) { //start Talking
                
            }
            OverrideLookAt(C.c.player[0].transform);
            UILookAtPlayer(player);
            yield return null;
        }

        while (uiPopup.alpha > 0) {
            uiPopup.alpha -= Time.deltaTime * 10f;
            yield return null;
        }

        interactPanel.SetActive(false);
        agent.isStopped = false;
        coroutineInteractActive = false;
    }

    public IEnumerator AILoop() {

        yield return null;

        while (true) {

            if (inventory[0].amount > 0 && !readyToCheckout && !goingHome) {
                if (Random.value < .01f) {
                    ReadyToCheckOut();
                    yield return null;
                }
            }

            if (!walkingToDestination) {

                if (readyToCheckout) {
                    timeSpentWaiting += loopDelay;
                    if (Random.value < .01f) UpdateInWaitingLine();
                    if (C.c.currentShop.npcCheckoutLine.Count > 0 && C.c.currentShop.npcCheckoutLine[0] == this) {
                        if (C.c.currentShop.register.assetBeingSoldModel.childCount == 0) {
                            var inst = Instantiate(inventory[0].asset.modelPrefab, C.c.currentShop.register.assetBeingSoldModel);
                            var rb = inst.GetComponent<Rigidbody>(); if (rb) rb.isKinematic = true;
                            inst.transform.localPosition = Vector3.zero + Vector3.up * inventory[0].asset.modelYOffset;
                            var coll = inst.AddComponent<SphereCollider>();
                            coll.radius *= 1.5f;
                            inst.transform.name = "AssetBeingSoldModel";
                            inst.transform.localScale *= inventory[0].asset.miniModelScaleMultiplier;
                        }
                    }
                }

                else { //At Shop
                    if (dining) {
                        if (diningSeat.ordered == null) {
                            yield return new WaitForSeconds(Random.Range(3f, 10f));
                            if (C.c.currentShop.foodMenu.Count > 0) {
                                diningSeat.ordered = C.c.currentShop.foodMenu[Random.Range(0, C.c.currentShop.foodMenu.Count)];
                                diningSeat.orderedModel = Instantiate(diningSeat.ordered.modelPrefab, diningSeat.seatTrans).transform;
                                diningSeat.orderedModel.localPosition = new Vector3(0, 2.5f, 0);
                                diningSeat.orderedModel.eulerAngles = new Vector3(30, 0, 30);
                                diningSeat.orderedModel.localScale *= diningSeat.ordered.invModelScaleMultiplier * .5f;
                            } else { timeSpentWaiting += 10f; }
                        } else {
                            if (!diningSeat.hasBeenServed) {
                                if (!C.c.currentShop.foodMenu.Contains(diningSeat.ordered)) { diningSeat.ordered = null; Destroy(diningSeat.orderedModel.gameObject); }
                                timeSpentWaiting += loopDelay;
                            }
                        }
                        if (diningSeat.finishedEating) {
                            yield return new WaitForSeconds(Random.Range(3f, 8f));
                            StopDining();
                            
                        }
                        else if (diningSeat.hasBeenServed) {
                            yield return new WaitForSeconds(Random.Range(5f, 10f));
                            diningSeat.finishedEating = true;
                            Destroy(diningSeat.orderedModel.gameObject);
                            diningSeat.plateNull.GetChild(0).gameObject.SetActive(false);
                            diningSeat.plateNull.GetChild(1).gameObject.SetActive(true);
                            if (!diningSeat.ordered.liquid) diningSeat.dirty = true;
                        }
                        if (timeSpentWaiting > 5 * 60) {
                            StopDining();
                        }
                    }
                    else if (interestedInAsset) {
                        if (!C.c.currentShop.assetsSelling.Contains(interestedInAsset)) { interestedInAsset = null; } //cancel if taken not selling anymore
                        else {
                            if (Random.value < .15f) { //get item
                                AddAssetToInventory(interestedInAsset.data);
                                C.c.currentShop.assetsSelling.Remove(interestedInAsset);
                                Destroy(interestedInAsset.gameObject);
                                interestedInAsset = null;
                            }
                            else if (Random.value < .15f) interestedInAsset = null; //lose interest in item
                        }
                    }
                    else {
                        if (atShop) {
                            if (Random.value < .06f && C.c.currentShop.assetsSelling.Count > 0) {
                                interestedInAsset = C.c.currentShop.assetsSelling[Random.Range(0, C.c.currentShop.assetsSelling.Count)];
                                agent.SetDestination(interestedInAsset.transform.position);
                                walkingToDestination = true;
                            }
                            if (Random.value < .06f) {
                                if (C.c.currentShop.availableDiningSeats.Count > 0 && C.c.currentShop.foodMenu.Count > 0) {
                                    dining = true;
                                    diningSeat = C.c.currentShop.availableDiningSeats[Random.Range(0, C.c.currentShop.availableDiningSeats.Count - 1)];
                                    C.c.currentShop.availableDiningSeats.Remove(diningSeat);
                                    agent.SetDestination(diningSeat.seatTrans.position);
                                    walkingToDestination = true;
                                }
                            }
                        }
                        else {
                            goingToShop = true;
                            agent.SetDestination(C.c.currentShop.transform.position + new Vector3(Random.Range(-C.c.currentShop.boxCastSize.x * .5f, C.c.currentShop.boxCastSize.x * .5f), 0,
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
            }
            
            yield return new WaitForSeconds(loopDelay);
        }

    }

    public void UpdateUI() {
        nameText.text = data.name;
        statusParent.GetComponentInChildren<Image>().gameObject.SetActive(false);
        if (data.relationshipPercent < 20) statusParent.Find("StatusHate").gameObject.SetActive(true); 
        else if (data.relationshipPercent < 40) statusParent.Find("StatusDislike").gameObject.SetActive(true); 
        else if (data.relationshipPercent < 60) statusParent.Find("StatusNeutral").gameObject.SetActive(true); 
        else if (data.relationshipPercent < 85) statusParent.Find("StatusLike").gameObject.SetActive(true); 
        else statusParent.Find("StatusLove").gameObject.SetActive(true); 
    }

    public void UILookAtPlayer(Player p) {
        uiPopup.transform.LookAt(p.transform);
        uiPopup.transform.eulerAngles = new Vector3(0, uiPopup.transform.eulerAngles.y, 0);
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

    public void ReadyToCheckOut() {
        readyToCheckout = true;
        C.c.currentShop.npcCheckoutLine.Add(this);
        UpdateInWaitingLine();
    }

    public void StopDining() {
        diningTable.ResetSeat(diningSeat);
        diningSeat = null;
        diningTable = null;
        dining = false;
        anim.SetBool("Sitting", false);

        if (owedAmount > 0) {
            AddAssetToInventory(C.c.data.bill);
            ReadyToCheckOut();
        } else {
            GoHome();
        }
    }

    public void GoHome() {
        goingHome = true;
        readyToCheckout = false;
        agent.SetDestination(data.home.position);
        walkingToDestination = true;
        timeSpentWaiting = 0;
    }

    public void OverrideLookAt(Transform lookAt) {
        transform.forward = Vector3.Slerp(transform.forward, lookAt.position - transform.position, Time.deltaTime * 4);
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
    }

    public void UpdateInWaitingLine() {
        if (C.c.currentShop.npcCheckoutLine.IndexOf(this) == 0) agent.SetDestination(C.c.currentShop.register.transform.position + -C.c.currentShop.register.transform.forward);
        else agent.SetDestination(C.c.currentShop.npcCheckoutLine[C.c.currentShop.npcCheckoutLine.IndexOf(this) - 1].transform.position);
        walkingToDestination = true;
    }

}
