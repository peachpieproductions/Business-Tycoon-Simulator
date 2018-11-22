using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    public int playerId = 0;
    internal Rigidbody rb;
    public float playerAcceleration = 2;
    public float playerMaxSpeed = 6;
    public float jumpHeight = 8;
    public float lookSpeed = 1.5f;
    public Transform cam;
    public bool buildMode;
    public bool buildPlacementValid;
    public Asset currentBuildAsset;
    BoxCollider currentBuildCollider;
    public Asset assetHovering;
    public Asset usingAsset;
    public int inventoryCurrentIndex;
    public List<AssetInventorySlot> inventory = new List<AssetInventorySlot>();
    public List<List<AssetInventorySlot>> upcomingDeliveries;
    public float deliveryTimer;
    public bool freeCam;
    public bool freeCamFreeRot; //if true, allows you to look around while freecam is active
    public Transform camOverridePos;
    public Vector3 camOffset;
    public PlayerUI pui;
    public float money = 100;
    public Portal nearbyPortal;
    public bool teleporting;
    public bool isSleeping;
    public bool inventoryOpen;
    public NPC lookingAtNpc;
    public NPC interactingWithNpc;

    private void Awake() {
        cam.gameObject.SetActive(true);
        C.c.player.Add(this);
        camOffset = cam.transform.localPosition;
        SetCameraMode(true);
        rb = GetComponent<Rigidbody>();
        upcomingDeliveries = new List<List<AssetInventorySlot>>();        
    }

    public IEnumerator TeleportSequence(Portal p) {
        teleporting = true;
        while (pui.blackOutPanel.alpha < 1) {
            pui.blackOutPanel.alpha += .05f;
            yield return null;
        }

        p.interior.SetInterior(p.enterance);

        yield return new WaitForSeconds(.2f);

        transform.position = p.goToPortal.transform.position + Vector3.up * .9f;

        while (pui.blackOutPanel.alpha > 0) {
            pui.blackOutPanel.alpha -= .05f;
            yield return null;
        }
        teleporting = false;
    }

    public IEnumerator Sleep() {
        if (isSleeping) yield break;
        isSleeping = true;
        while (pui.blackOutPanel.alpha < 1) {
            pui.blackOutPanel.alpha += .005f;
            yield return null;
        }

        yield return new WaitForSeconds(3f);

        C.c.time += 6 * 60;

        while (pui.blackOutPanel.alpha > 0) {
            pui.blackOutPanel.alpha -= .004f;
            yield return null;
        }
        isSleeping = false;
    }

    private void Update() {

        MoveAndLook();

        //DEBUG CODE
        /*if (Input.GetKeyDown(KeyCode.Alpha9)) {
            foreach (AssetInventorySlot s in inventory) {
                s.asset = C.c.data.assetData[Random.Range(0, C.c.data.assetData.Count)];
                s.amount = 10;
            }
            pui.invRender.UpdateInventoryRender();
        }*/

        //virtual mouse with controller
        if (Cursor.lockState == CursorLockMode.None) {
            HardwareCursor.SetPosition(HardwareCursor.GetPosition() + new Vector2(InputManager.MovementInput(playerId).x * 600 * Time.deltaTime, -InputManager.MovementInput(playerId).y * 600 * Time.deltaTime));
            if (InputManager.JumpInput(playerId)) { HardwareCursor.LeftClick(); }
        }

        

        //deliveries
        if (deliveryTimer > 0) {
            deliveryTimer -= Time.deltaTime;
            if (deliveryTimer <= 0) {
                C.c.StartCoroutine(C.c.Delivery());
                if (upcomingDeliveries.Count > 0) deliveryTimer = 60 * 3;
            }
        }

        //hover assets for interaction
        assetHovering = null;
        lookingAtNpc = null;
        RaycastHit hit;
        if (!buildMode && usingAsset == null) {
            if (Physics.Raycast(cam.transform.position + cam.transform.forward * .6f, cam.transform.forward, out hit, 2f)) {
                if (hit.transform.CompareTag("Asset")) {
                    var asset = hit.transform.parent.GetComponent<Asset>();
                    if (asset) {
                        assetHovering = asset;
                        asset.Hovering(this);
                    }
                }
                else if (hit.transform.CompareTag("NPC")) {
                    var npc = hit.transform.GetComponent<NPC>();
                    if (npc) {
                        lookingAtNpc = npc;
                        StartCoroutine(npc.InteractWithAI());
                    }
                }
                else if (hit.transform.GetComponent<Interactable>()) {
                    if (InputManager.InteractInput(playerId)) hit.transform.GetComponent<Interactable>().Use();
                }
            }
        }

        //Interacting with NPC
        if (interactingWithNpc) {
            rb.velocity = Vector3.zero;
            cam.LookAt(interactingWithNpc.interactPanel.transform);
        }

        //build mode
        BuildMode();

        //use asset
        if (InputManager.InteractInput(playerId) && !isSleeping && !teleporting) {

            if (assetHovering) {
                if (usingAsset) StopUsingAsset();
                assetHovering.Use();
            }
            else if (usingAsset) {
                StopUsingAsset();
            }
            else if (nearbyPortal) {
                if (!teleporting && !isSleeping) StartCoroutine(TeleportSequence(nearbyPortal));
            }
            else {
                if (Physics.Raycast(cam.transform.position + cam.transform.forward * .6f, cam.transform.forward, out hit, 2f)) {
                    if (hit.transform.GetComponent<Money>()) {
                        float moneyValue = Mathf.Max(1, Mathf.Round(hit.transform.GetComponent<Money>().value));
                        money += moneyValue;
                        if (hit.transform.GetComponent<Money>().isTip) pui.CreateInfoPopup("+ $" + moneyValue + " (tip)", C.c.data.colors[0]);
                        else pui.CreateInfoPopup("+ $" + Mathf.Round(moneyValue), C.c.data.colors[0]);
                        Destroy(hit.transform.gameObject);
                    }
                    else if (hit.transform.name == "AssetBeingSoldModel") {
                        var npcAtRegister = C.c.currentShop.npcCheckoutLine[0];
                        AssetData assetBeingSold = npcAtRegister.inventory[0].asset;
                        var chargeAmount = Mathf.Max(1f, Mathf.Round(assetBeingSold.currentValue));
                        if (assetBeingSold == C.c.data.bill) chargeAmount = Mathf.Max(1f, Mathf.Round(npcAtRegister.owedAmount));
                        var m = C.c.SpawnMoney(C.c.currentShop.register.moneySpawnPoint.position, chargeAmount);
                        if (assetBeingSold == C.c.data.bill) {
                            npcAtRegister.owedAmount = 0;
                            var tip = Mathf.Round(2f - npcAtRegister.timeSpentWaiting / 30);
                            if (tip > 0) {
                                var tipMoney = C.c.SpawnMoney(C.c.currentShop.register.moneySpawnPoint.position, tip - Random.Range(0, .5f));
                                tipMoney.isTip = true;
                            }
                        }

                        if (--npcAtRegister.inventory[0].amount == 0) {
                            npcAtRegister.inventory.Add(new AssetInventorySlot());
                            npcAtRegister.inventory.RemoveAt(0);
                        }
                        if (npcAtRegister.inventory[0].amount == 0) {
                            npcAtRegister.GoHome();
                            C.c.currentShop.npcCheckoutLine.RemoveAt(0);
                            foreach (NPC n in C.c.currentShop.npcCheckoutLine) n.UpdateInWaitingLine();
                        } 

                        Destroy(hit.transform.gameObject);
                        
                    }
                }
            }
        }

        //Inventory
        if (InputManager.ToggleInventory(playerId) && !buildMode) {
            ToggleInventory();
        }

        //use asset secondary use
        if (InputManager.SecondaryInteractInput(playerId)) {
            if (assetHovering) {
                if (usingAsset) StopUsingAsset();
                assetHovering.UseSecondary();
            }
            else if (usingAsset) {
                StopUsingAsset();
            }
        }

        //put asset on sale
        if (InputManager.SecondaryInteractInput(playerId)) {
            if (assetHovering && assetHovering.playerUsing == null) {
                assetHovering.ToggleSelling();
            }
        }

        //pick up asset
        if (InputManager.PickUpInput(playerId)) { 
            if (assetHovering && !assetHovering.forceCantPickUp) {
                if (assetHovering.useTag == "Storage") {
                    var stor = assetHovering.model.GetComponent<Storage>();
                    if (stor) {
                        if (!stor.IsEmpty()) { stor.asset.Use(); return; }
                    }
                }
                else if (assetHovering.useTag == "RestaurantTable") {
                    var diningTable = assetHovering.model.GetComponent<DiningTable>();
                    if (diningTable) {
                        if (diningTable.isBeingUsed()) {
                            return;
                        } else {
                            diningTable.PickUpTable();
                        }
                    }
                }
                if (assetHovering.selling) assetHovering.ToggleSelling();
                var added = AddAssetToInventory(assetHovering.data);
                if (added) Destroy(assetHovering.gameObject);
                pui.invRender.UpdateInventoryRender();
            }
        }

        //Cycle active asset
        if (InputManager.CycleHotbar(playerId) != 0 && usingAsset == null) {
            inventoryCurrentIndex += (int)InputManager.CycleHotbar(playerId);
            if (inventoryCurrentIndex < 0) inventoryCurrentIndex = 0;
            if (inventoryCurrentIndex >= 8) inventoryCurrentIndex = 7;
            if (currentBuildAsset) Destroy(currentBuildAsset.gameObject);
            pui.invRender.UpdateInventoryRender();
        }
    }

    public void ToggleInventory() {
        if (!inventoryOpen && Cursor.lockState == CursorLockMode.None) return;
        inventoryOpen = !inventoryOpen;
        pui.invRender.OpenMainInventory(inventoryOpen);
        if (inventoryOpen) {
            SetCameraMode(false);
        }
        else {
            if (pui.invRender.storageOpen) pui.invRender.CloseStorage();
            SetCameraMode(true);
        }
    }

    //Stop using asset
    public void StopUsingAsset() {
        if (usingAsset) {
            usingAsset.StopUsing();
            SetCameraMode(true);
            usingAsset = null;
            pui.SetUI(true);
        }
    }

    public void SetCameraMode(bool standardFpsMode,bool showCursor = true, Transform camOverride = null, bool freeRot = false) {
        if (standardFpsMode) {
            FreeCamToggle(false);
            camOverridePos = null;
            freeCamFreeRot = false;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        } else {
            FreeCamToggle(true);
            camOverridePos = camOverride;
            freeCamFreeRot = freeRot;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = showCursor;
        }
    }

    public void FreeCamToggle(bool free) {
        freeCam = free;
        //if (!freeCam) { cam.parent = transform;  }
    }


    public void BuildMode() {
        if (buildMode) {
            if (currentBuildAsset == null) {
                if (inventory[inventoryCurrentIndex].amount > 0) {
                    currentBuildAsset = Instantiate(C.c.data.assetPrefab);
                    currentBuildAsset.Set(inventory[inventoryCurrentIndex].asset, "",true);
                    buildPlacementValid = false;
                    currentBuildAsset.model.GetComponent<MeshRenderer>().material = C.c.data.placingMats[1];
                    currentBuildCollider = currentBuildAsset.model.AddComponent<BoxCollider>();
                    currentBuildCollider.isTrigger = true;
                }
            }
            if (currentBuildAsset) {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position + cam.transform.forward, cam.transform.forward, out hit, 10f)) {
                    var placementValid = buildPlacementValid;
                    if (Mathf.Abs(hit.normal.x) < .2f && Mathf.Abs(hit.normal.y) > .9f && Mathf.Abs(hit.normal.z) < .2f) placementValid = true;
                    RaycastHit[] hits;
                    hits = Physics.BoxCastAll(currentBuildCollider.bounds.center, Vector3.Scale(currentBuildCollider.size, currentBuildAsset.model.transform.localScale) * .48f, Vector3.up, currentBuildAsset.transform.rotation, .01f);
                    foreach(RaycastHit h in hits) {
                        if (h.transform != currentBuildAsset.model.transform && h.transform != transform) placementValid = false;
                    }

                    if (placementValid) { if (!buildPlacementValid) currentBuildAsset.model.GetComponent<MeshRenderer>().material = C.c.data.placingMats[0]; buildPlacementValid = true; }
                        else { if (buildPlacementValid) currentBuildAsset.model.GetComponent<MeshRenderer>().material = C.c.data.placingMats[1]; buildPlacementValid = false; }

                    var ydiff = currentBuildAsset.transform.position.y - (currentBuildCollider.bounds.center.y - currentBuildCollider.bounds.extents.y);
                    if (currentBuildAsset.physicsAsset) currentBuildAsset.transform.position = hit.point + Vector3.up * + (ydiff + .05f);
                    else currentBuildAsset.transform.position = hit.point + Vector3.up * ydiff;

                    if (InputManager.RotatePlacement(playerId) != 0) { currentBuildAsset.transform.Rotate(0, .7f * InputManager.RotatePlacement(playerId), 0);
                        if (Input.GetKey(KeyCode.LeftShift)) currentBuildAsset.transform.Rotate(0, .7f * InputManager.RotatePlacement(playerId), 0); }
                    if ((InputManager.PlaceAsset(playerId) || InputManager.JumpInput(playerId)) && buildPlacementValid) {
                        currentBuildAsset.placing = false;
                        currentBuildAsset.Set(currentBuildAsset.data);
                        if (InputManager.AutoSellWhilePlacing(playerId)) currentBuildAsset.ToggleSelling();
                        currentBuildAsset = null;
                        inventory[inventoryCurrentIndex].amount--;
                        pui.invRender.UpdateInventoryRender();
                        C.c.currentShop.UpdateShop();
                    }
                }
            }
        }

        //Start / stop Build Mode
        if (InputManager.BuildModeInput(playerId) || (buildMode && InputManager.Cancel(playerId))) {
            buildMode = !buildMode;
            pui.modeStatusText.transform.parent.gameObject.SetActive(buildMode);
            if (!buildMode) {
                if (currentBuildAsset) Destroy(currentBuildAsset.gameObject);
            }
        }
    }

    public bool AddAssetToInventory(AssetData asset, int amount = 1) {
        bool placedAsset = false;
        foreach(AssetInventorySlot slot in inventory) { //find existing asset stack
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



    public void MouseLook(bool rotPlayer) {
        cam.Rotate(-InputManager.LookInput(playerId).y * lookSpeed * 135 * Time.deltaTime, 0, 0);
        cam.localEulerAngles = new Vector3(cam.localEulerAngles.x, cam.localEulerAngles.y, 0);
        if (rotPlayer) transform.Rotate(0, InputManager.LookInput(playerId).x * lookSpeed * 135 * Time.deltaTime, 0);
        else cam.transform.Rotate(0, InputManager.LookInput(playerId).x * lookSpeed * 135 * Time.deltaTime, 0);
    }

    public void MoveAndLook() {

        //camera
        if (freeCam) {
            if (camOverridePos) {
                cam.transform.position = Vector3.Lerp(cam.transform.position, camOverridePos.position, .02f);
                if (!freeCamFreeRot) cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camOverridePos.rotation, .02f); 
            }
            if (freeCamFreeRot) MouseLook(false);
        }
        else {
            //lerp cam back to head
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camOffset, .2f);

            //aim body to cam
            if (cam.localEulerAngles.y != 0) {
                var diff = cam.localEulerAngles.y;
                transform.Rotate(0, diff, 0,Space.World);
                cam.Rotate(0, -diff, 0,Space.World);
            }

            //Mouse Look
            MouseLook(true);

            //Keyboard inputs
            rb.velocity += InputManager.MovementInput(playerId).y * transform.forward * playerAcceleration +
                InputManager.MovementInput(playerId).x * transform.right * playerAcceleration;

            if (InputManager.JumpInput(playerId) && !buildMode) {
                rb.velocity += Vector3.up * jumpHeight;
            }

        }

        //Max Speed
        var maxSpd = playerMaxSpeed;
        if (Input.GetKey(KeyCode.LeftShift)) maxSpd *= 4;

        var vel = rb.velocity;
        vel.y = 0;
        if (vel.magnitude > maxSpd) vel = vel.normalized * maxSpd;
        vel *= .8f;
        vel.y = rb.velocity.y;
        rb.velocity = vel;
    }


}
