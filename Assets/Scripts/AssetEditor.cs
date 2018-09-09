﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using cakeslice;

[CustomEditor(typeof(Asset))]
public class AssetEditor : Editor {

    public override void OnInspectorGUI() {

        Asset asset = (Asset)target;

        EditorGUILayout.LabelField("Asset Tools", EditorStyles.boldLabel);

        
        if (GUILayout.Button("Load Asset")) {
            if (asset.model) { DestroyImmediate(asset.model); }
            asset.model = Instantiate(asset.data.modelPrefab, asset.transform);
            asset.model.transform.localPosition = Vector3.zero;
            asset.outline = asset.model.GetComponent<Outline>();
            asset.coll = asset.model.GetComponent<Collider>();
            asset.camOverride = asset.model.transform.Find("CamOverride");
            asset.goToActivateWhenOn = asset.model.transform.Find("ActivateWhenOn");
            asset.assetName = asset.data.name;
            asset.useTag = asset.data.useTag;
            asset.physicsAsset = asset.data.physicsAsset;
            asset.transform.name = asset.assetName + " Asset";
            Debug.Log(asset.assetName + " Asset Loaded.");
        }


        EditorGUILayout.LabelField("Asset Properties", EditorStyles.boldLabel);

        base.OnInspectorGUI();


    }

}
