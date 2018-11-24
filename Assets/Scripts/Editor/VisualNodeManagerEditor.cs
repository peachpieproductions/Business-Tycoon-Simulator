using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(VisualNodeManager))]
public class VisualNodeManagerEditor : Editor {

    public override void OnInspectorGUI() {

        VisualNodeManager nodeManager = (VisualNodeManager)target;


        if (GUILayout.Button("Create New Node")) {
            nodeManager.CreateNewNode();
        }

        base.OnInspectorGUI();


    }


}
