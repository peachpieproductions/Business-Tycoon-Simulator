using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu]
public class AssetData : ScriptableObject {

    public string useTag;
    public int assetLevel;
    public float baseValue;
    public float currentValue;
    public float currentPop;
    public bool physicsAsset;
    public GameObject modelPrefab;
    public float miniModelScaleMultiplier = 1;
    public float invModelScaleMultiplier = 1;

}
