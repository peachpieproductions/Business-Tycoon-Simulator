﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class NPCData : ScriptableObject {

    public string npcName;
    public GameObject npcModelPrefab;
    public AssetData favoriteAsset;
    public float relationshipPercent = 50f;
    public bool isFemale;
    public bool isHome;
    public Transform home;

}
