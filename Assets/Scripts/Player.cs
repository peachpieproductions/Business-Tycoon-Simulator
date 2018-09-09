using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {


    internal Rigidbody rb;
    public float playerAcceleration = 2;
    public float playerMaxSpeed = 6;
    public float jumpHeight = 8;
    public float lookSpeed = 1.5f;
    public Transform cam;
    public bool buildMode;
    public Asset currentBuildAsset;
    public Asset assetHovering;
    public Asset usingAsset;
    public int inventoryCurrentIndex;
    public List<AssetInventorySlot> inventory = new List<AssetInventorySlot>();
    public List<AssetInventorySlot> upcomingDelivery = new List<AssetInventorySlot>();
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

    private void Awake() {
        cam.gameObject.SetActive(true);
        C.c.player.Add(this);
        camOffset = cam.transform.localPosition;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
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
            pui.blackOutPanel.alpha -= .001f;
            yield return null;
        }
        isSleeping = false;
    }

    private void Update() {

        MoveAndLook();

        //deliveries
        if (deliveryTimer > 0) {
            deliveryTimer -= Time.deltaTime;
            if (deliveryTimer <= 0) {
                C.c.StartCoroutine(C.c.Delivery());
            }
        }

        //hover assets for interaction
        assetHovering = null;
        RaycastHit hit;
        if (Physics.Raycast(cam.transform.position + cam.transform.forward * .6f, cam.transform.forward, out hit,2f)) { 
            if (hit.transform.CompareTag("Asset")) {
                var asset = hit.transform.parent.GetComponent<Asset>();
                if (asset) {
                    assetHovering = asset;
                    asset.Hovering(this);
                }
            } 
            else if (hit.transform.GetComponent<Interactable>()) {
                if (Input.GetKeyDown(KeyCode.E)) hit.transform.GetComponent<Interactable>().Use();
            }
        }

        //build mode
        BuildMode();

        //use asset
        if (Input.GetKeyDown(KeyCode.E)) {
            
            if (usingAsset) {
                StopUsingAsset();
            }
            else if (assetHovering) {
                assetHovering.Use(); 
            }
            else if (nearbyPortal) {
                if (!teleporting) StartCoroutine(TeleportSequence(nearbyPortal));
            }
            else {
                if (Physics.Raycast(cam.transform.position + cam.transform.forward * .6f, cam.transform.forward, out hit, 2f)) {
                    if (hit.transform.GetComponent<Money>()) {
                        money += Mathf.Round(hit.transform.GetComponent<Money>().value);
                        pui.CreateInfoPopup("+ $" + Mathf.Round(hit.transform.GetComponent<Money>().value), C.c.data.colors[0]);
                        Destroy(hit.transform.gameObject);
                    }
                    else if (hit.transform.name == "AssetBeingSoldModel") {
                        if (hit.transform.childCount > 0) {

                            var m = Instantiate(C.c.data.moneyPrefabs[1], C.c.currentShop.register.moneySpawnPoint.position +
                                new Vector3(Random.Range(-.2f, .2f), 0, Random.Range(-.2f, .2f)), Quaternion.Euler(0, Random.Range(0, 359), 0)).GetComponent<Money>();
                            m.value = Mathf.Round(C.c.currentShop.npcCheckoutLine[0].inventory[0].asset.currentValue);

                            if (--C.c.currentShop.npcCheckoutLine[0].inventory[0].amount == 0) {
                                C.c.currentShop.npcCheckoutLine[0].inventory.Add(new AssetInventorySlot());
                                C.c.currentShop.npcCheckoutLine[0].inventory.RemoveAt(0);
                            }
                            if (C.c.currentShop.npcCheckoutLine[0].inventory[0].amount == 0) {
                                C.c.currentShop.npcCheckoutLine[0].GoHome();
                                C.c.currentShop.npcCheckoutLine.RemoveAt(0);
                                foreach (NPC n in C.c.currentShop.npcCheckoutLine) n.UpdateInWaitingLine();
                            } 

                            Destroy(hit.transform.GetChild(0).gameObject);
                        }
                    }
                }
            }
        } 

        //put asset on sale
        if (Input.GetKeyDown(KeyCode.F)) {
            if (assetHovering) {
                assetHovering.ToggleSelling();
            }
        }

        //pick up asset
        if (Input.GetMouseButtonDown(1)) { 
            if (assetHovering) {
                if (assetHovering.useTag == "Storage") {
                    var stor = assetHovering.model.GetComponent<Storage>();
                    if (stor) {
                        if (stor.inventory[0].amount > 0) { stor.Open(); }
                    }
                }
                var added = AddAssetToInventory(assetHovering.data);
                if (added) Destroy(assetHovering.gameObject);
                pui.invRender.UpdateInventoryRender();
            }
        }

        //Cycle active asset
        if (Input.mouseScrollDelta.y != 0) {
            inventoryCurrentIndex += (int)Input.mouseScrollDelta.y;
            if (inventoryCurrentIndex < 0) inventoryCurrentIndex = 0;
            if (inventoryCurrentIndex >= inventory.Count) inventoryCurrentIndex = inventory.Count-1;
            if (currentBuildAsset) Destroy(currentBuildAsset.gameObject);
            pui.invRender.UpdateInventoryRender();
        }
    }

    //Stop using asset
    public void StopUsingAsset() {
        if (usingAsset) {
            usingAsset = null;
            freeCamFreeRot = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (freeCam) FreeCamToggle();
        }
    }

    public void FreeCamToggle() {
        freeCam = !freeCam;
        if (!freeCam) { cam.parent = transform; cam.transform.localEulerAngles = Vector3.zero; }
    }

    public void BuildMode() {
        if (buildMode) {
            if (currentBuildAsset == null) {
                if (inventory[inventoryCurrentIndex].amount > 0) {
                    currentBuildAsset = Instantiate(C.c.data.assetPrefab);
                    currentBuildAsset.Set(inventory[inventoryCurrentIndex].asset, "",true);
                }
            }
            if (currentBuildAsset) {
                RaycastHit hit;
                if (Physics.Raycast(cam.transform.position + cam.transform.forward, cam.transform.forward, out hit, 10f)) {
                    currentBuildAsset.transform.position = hit.point;
                    if (Input.GetKey(KeyCode.R)) { currentBuildAsset.transform.Rotate(0, .6f, 0); }
                    if (Input.GetMouseButtonDown(0)) {
                        currentBuildAsset.placed = true;
                        currentBuildAsset.Set(currentBuildAsset.data);
                        if (Input.GetKey(KeyCode.LeftShift)) currentBuildAsset.ToggleSelling();
                        currentBuildAsset = null;
                        inventory[inventoryCurrentIndex].amount--;
                        pui.invRender.UpdateInventoryRender();
                    }
                }
            }
        }

        //Start / stop Build Mode
        if (Input.GetKeyDown(KeyCode.B)) {
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

    public bool AddToUpcomingDelivery(AssetData asset, int amount) {
        bool placedAsset = false;
        foreach (AssetInventorySlot slot in upcomingDelivery) { //find existing asset stack
            if (slot.amount > 0 && slot.asset == asset) {
                slot.amount += amount;
                placedAsset = true;
                break;
            }
        }
        if (!placedAsset) {
            foreach (AssetInventorySlot slot in upcomingDelivery) { //make new stack
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
        cam.Rotate(-Input.GetAxis("Mouse Y") * lookSpeed, 0, 0);
        cam.localEulerAngles = new Vector3(cam.localEulerAngles.x, cam.localEulerAngles.y, 0);
        if (rotPlayer) transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        else cam.transform.Rotate(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
    }

    public void MoveAndLook() {

        //camera
        if (freeCam) {
            cam.transform.position = Vector3.Lerp(cam.transform.position, camOverridePos.position, .02f);
            if (!freeCamFreeRot) cam.transform.rotation = Quaternion.Slerp(cam.transform.rotation, camOverridePos.rotation, .02f);
            else MouseLook(false);
        }
        else {
            //lerp cam back to head
            cam.transform.localPosition = Vector3.Lerp(cam.transform.localPosition, camOffset, .02f);

            //Mouse Look
            MouseLook(true);

            //Keyboard inputs
            if (Input.GetKey(KeyCode.W)) {
                rb.velocity += transform.forward * playerAcceleration;
            }
            if (Input.GetKey(KeyCode.S)) {
                rb.velocity += -transform.forward * playerAcceleration;
            }
            if (Input.GetKey(KeyCode.A)) {
                rb.velocity += -transform.right * playerAcceleration;
            }
            if (Input.GetKey(KeyCode.D)) {
                rb.velocity += transform.right * playerAcceleration;
            }
            if (Input.GetKeyDown(KeyCode.Space)) {
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
