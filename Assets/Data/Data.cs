using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Data : ScriptableObject {

    public Material outlineMaterial;
    public Asset assetPrefab;
    public NPC npcPrefab;
    public List<AssetData> assetData = new List<AssetData>();
    public List<NPCData> npcData = new List<NPCData>();
    public List<GameObject> prefabs = new List<GameObject>();
    public List<GameObject> moneyPrefabs = new List<GameObject>();
    public Color[] colors;
    public Material[] placingMats;
    public List<NPC> npcsAtHome = new List<NPC>();
    public List<NPC> npcsOut = new List<NPC>();

}
