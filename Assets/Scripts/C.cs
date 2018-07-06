using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class C : MonoBehaviour {

    public float time;
    public string timeString;
    public Data data;
    public static C c;
    public List<Player> player = new List<Player> ();
    public Light sunLight;
    public Transform deliveryZone;
    public Shop currentShop;
    public Economy econ;
    public Transform npcHomesParent;
    

    public VirtualInputModule virtualInputModule;

    private void Awake() {
        c = this;
        time = 10 * 60;

    }

    private void Start() {
        data.npcsOut.Clear();
        data.npcsAtHome.Clear();

        //generate NPCs
        foreach(NPCData n in data.npcData) {
            n.isHome = true;
            n.home = npcHomesParent.GetChild(Random.Range(0, npcHomesParent.childCount));
            var npc = Instantiate(data.npcPrefab);
            data.npcsAtHome.Add(npc);
            npc.Set(n);
            npc.transform.position = npc.data.home.position;
            npc.gameObject.SetActive(false);
        }
    }

    private void Update() {

        if (data.npcsOut.Count < 5) {
            if (Random.value < .001f) {
                var npc = data.npcsAtHome[Random.Range(0, data.npcsAtHome.Count)];
                data.npcsAtHome.Remove(npc);
                data.npcsOut.Add(npc);
                npc.gameObject.SetActive(true);
            }
        }

        //DEBUG STUFF
        if (Input.GetKeyDown(KeyCode.UpArrow)) Time.timeScale += 1f;
        if (Input.GetKeyDown(KeyCode.DownArrow)) Time.timeScale = Mathf.Max(1,Time.timeScale - 1f);
        if (Input.GetKeyDown(KeyCode.F1)) Cursor.lockState = CursorLockMode.Locked;

        //time
        sunLight.transform.eulerAngles = new Vector3(RemapFloat(time,0,60 * 24,230, 230 + 360), 33, 0);
        var sunlight = true;
        if (time < 8 * 60 || time > 20 * 60) sunlight = false;
        if (sunlight) {
            if (RenderSettings.ambientIntensity < 1) RenderSettings.ambientIntensity += Time.deltaTime * .01f;
        }
        else {
            if (RenderSettings.ambientIntensity > 0) RenderSettings.ambientIntensity -= Time.deltaTime * .01f;
        }
        sunLight.intensity = RenderSettings.ambientIntensity * 1.3f;
        time += Time.deltaTime;
        if (time > 24 * 60) time = 0;
        var suffix = " am";
        if (time > 12 * 60) suffix = " pm";
        var hour = ((time > 12 * 60) ? Mathf.Floor(((time) - 12 * 60) / 60f).ToString("F0") : Mathf.Floor((time) / 60f).ToString("F0"));
        if (hour == "0") hour = "12";
        var min = ((time % 60 < 10) ? ("0" + Mathf.Floor(time % 60)) : (Mathf.Floor(time % 60)).ToString());
        timeString = hour + ":" + min + suffix;

    }

    public AssetData GetRandomAsset(int lvl) {
        List<AssetData> newList = new List<AssetData>();
        foreach(AssetData d in data.assetData) {
            if (d.assetLevel == lvl) newList.Add(d);
        }
        return newList[Random.Range(0, newList.Count)];
    }

    public void Delivery() {
        var inst = Instantiate(data.assetPrefab);
        inst.Set(null, "Wooden Crate");
        inst.transform.position = deliveryZone.position;
        foreach(AssetInventorySlot a in player[0].upcomingDelivery) {
            inst.GetComponentInChildren<Storage>().AddAssetToInventory(a.asset, a.amount);
            a.amount = 0;
            a.asset = null;
        }
    }

    public static float RemapFloat(float value, float from1, float to1, float from2, float to2) {
        return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
    }


}
