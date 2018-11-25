using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CanEditMultipleObjects]
[CustomEditor(typeof(VisualNode))]
public class VisualNodeEditor : Editor {

    

    public GameObject CreateNewResponseNode(VisualNode node) {
        var inst = node.nodeManager.CreateNewNode();
        inst.transform.position = node.transform.position + Vector3.right * 5;
        inst.GetComponent<SpriteRenderer>().color = new Color(.7f, 1f, 1f);
        inst.GetComponent<VisualNode>().response = true;
        node.responses.Add(inst.GetComponent<VisualNode>());
        node.data.responses.Add(inst.GetComponent<VisualNode>().data);
        inst.GetComponent<VisualNode>().responseTo.Add(node);
        inst.GetComponent<VisualNode>().data.responseTo.Add(node.data);
        Selection.activeTransform = inst.transform;

        node.nodeManager.CreateNewLineConnection(node.transform, inst.transform);

        return inst;
    }

    public GameObject CreateNewConversationNode(VisualNode node) {
        var inst = node.nodeManager.CreateNewNode();
        inst.transform.position = node.transform.position + Vector3.right * 5;
        node.responses.Add(inst.GetComponent<VisualNode>());
        node.data.responses.Add(inst.GetComponent<VisualNode>().data);
        inst.GetComponent<VisualNode>().responseTo.Add(node);
        inst.GetComponent<VisualNode>().data.responseTo.Add(node.data);
        Selection.activeTransform = inst.transform;

        node.nodeManager.CreateNewLineConnection(node.transform, inst.transform);

        return inst;
    }

    public override void OnInspectorGUI() {

        VisualNode node = (VisualNode)target;

        GUI.backgroundColor = new Color(.7f, 1f, 1f);
        if (GUILayout.Button("Create New Response")) {
            CreateNewResponseNode(node);
        }

        GUI.backgroundColor = Color.white;
        if (GUILayout.Button("Create New Conversation Node")) {
            CreateNewConversationNode(node);
        }

        GUI.backgroundColor = new Color(1f, .8f, .8f);
        if (GUILayout.Button("Break Connections")) {
            foreach (VisualNode n in node.responses) {
                n.responseTo.Remove(node);
                n.data.responseTo.Remove(node.data);
            }
            node.responses.Clear();
            node.data.responses.Clear();
            foreach (VisualNode n in node.responseTo) {
                n.responses.Remove(node);
                n.data.responses.Remove(node.data);
            }
            node.responseTo.Clear();
            node.data.responseTo.Clear();
            foreach(VisualNodeLine l in FindObjectsOfType<VisualNodeLine>()) {
                if (l.from == node.transform || l.to == node.transform) {
                    DestroyImmediate(l.gameObject);
                }
            }
        }
        GUI.backgroundColor = Color.white;

        EditorGUILayout.BeginHorizontal();
        node.dataName = EditorGUILayout.TextArea(node.dataName);

        if (GUILayout.Button("Rename Data")) {
            if (node.dataName != "") {
                AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(node.data), node.dataName);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }
        }
        if (GUILayout.Button("Edit Data")) {
            Selection.activeObject = node.data;
        }
        EditorGUILayout.EndHorizontal();

        EditorStyles.textField.wordWrap = true;
        if (node.text.Count > 0) node.text[0].text = EditorGUILayout.TextArea(node.text[0].text, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 5), GUILayout.MaxWidth(Screen.width - 40));

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Required Relation", GUILayout.Width(96));
        node.relPercentCompare = (Comparison) EditorGUILayout.EnumPopup(node.relPercentCompare);
        node.relationshipPercReq = EditorGUILayout.FloatField(node.relationshipPercReq);
        EditorGUILayout.EndHorizontal();

        base.OnInspectorGUI();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("New Data")) {
            node.data = node.nodeManager.CreateNewData() as ConversationNodeData;
            node.dataName = node.data.name;
        }

        GUI.backgroundColor = new Color(1f, .6f, .6f);
        if (GUILayout.Button("DESTROY w/ DATA")) {
            var path = AssetDatabase.GetAssetPath(node.data);
            AssetDatabase.DeleteAsset(path);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            DestroyImmediate(node.gameObject);
        }
        EditorGUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;


    }

    bool cKeyHeld;

    private void OnSceneGUI() {

        VisualNode node = (VisualNode)target;

        if (Event.current.type == EventType.KeyDown) {

            var ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
            ray.z = -1;
            RaycastHit hit;
            VisualNode hoveringNode = null;
            if (Physics.Raycast(ray, Vector3.forward, out hit, 10)) {
                hoveringNode = hit.transform.GetComponent<VisualNode>();
            }
            if (Event.current.keyCode == KeyCode.C) {
                if (!cKeyHeld) {
                    cKeyHeld = true;
                    node.nodeManager.connectNodeLine.gameObject.SetActive(true);
                    node.nodeManager.connectNodeLine.SetPosition(0, ray);
                }
                node.nodeManager.connectNodeLine.SetPosition(1, ray);
                
                if (node.nodeManager.connectNodeFrom == null) {
                    if (hoveringNode) node.nodeManager.connectNodeFrom = hoveringNode;
                }
                else if (node.nodeManager.connectNodeTo == null) {
                    if (hoveringNode && hoveringNode != node.nodeManager.connectNodeFrom) {
                        node.nodeManager.connectNodeTo = hoveringNode;
                    }
                }
            }
        }
        if (Event.current.type == EventType.KeyUp) {
            cKeyHeld = false;
            node.nodeManager.connectNodeLine.gameObject.SetActive(false);

            if (node.nodeManager.connectNodeFrom && node.nodeManager.connectNodeTo) {
                node.nodeManager.connectNodeFrom.responses.Add(node.nodeManager.connectNodeTo);
                node.nodeManager.connectNodeFrom.data.responses.Add(node.nodeManager.connectNodeTo.data);
                node.nodeManager.connectNodeTo.responseTo.Add(node.nodeManager.connectNodeFrom);
                node.nodeManager.connectNodeTo.data.responseTo.Add(node.nodeManager.connectNodeFrom.data);
                node.nodeManager.CreateNewLineConnection(node.nodeManager.connectNodeFrom.transform, node.nodeManager.connectNodeTo.transform);
            }

            node.nodeManager.connectNodeFrom = null;
            node.nodeManager.connectNodeTo = null;
        }

        

        //Draw Scene view buttons
        Handles.BeginGUI();

        Rect rect = new Rect(0, 0, 100, 25);
        rect.center = new Vector2(Screen.width / 2, Screen.height * .7f + 140);
        GUIStyle style = new GUIStyle(GUI.skin.button);
        if (node.showEditingHud) GUI.color = new Color(.7f, .7f, .7f);
        style.alignment = TextAnchor.MiddleCenter;
        if (GUI.Button(rect, "Show / Hide", style)) {
            node.showEditingHud = !node.showEditingHud;
        }

        if (node.showEditingHud) {

            rect = new Rect(0, 0, 150, 50);
            rect.center = new Vector2(Screen.width / 2 - 100, Screen.height * .7f + 80);
            GUI.color = new Color(.7f, 1f, 1f);
            style.fontSize = 15;
            style.alignment = TextAnchor.MiddleCenter;
            if (GUI.Button(rect, "New Response", style)) {
                CreateNewResponseNode(node);
            }

            rect = new Rect(0, 0, 150, 50);
            rect.center = new Vector2(Screen.width / 2 + 100, Screen.height * .7f + 80);
            GUI.color = new Color(1f, 1f, 1f);
            if (GUI.Button(rect, "New Conv. Node", style)) {
                CreateNewConversationNode(node);
            }

            rect = new Rect(0, 0, 300, 70);
            rect.center = new Vector2(Screen.width / 2, Screen.height * .7f - 16);
            GUI.color = new Color(1f, 1f, 1f);
            style = new GUIStyle(GUI.skin.textField);
            style.fontSize = 14;
            node.text[0].text = GUI.TextField(rect, node.text[0].text, style);

            rect = new Rect(0, 0, 300, 15);
            rect.center = new Vector2(Screen.width / 2, Screen.height * .7f + 32);
            GUI.Box(rect, "", style);
            rect = new Rect(rect.x, rect.y, 300, 25);
            node.emotion = (ConversationNodeData.Emotion)EditorGUI.EnumPopup(rect, "Emotion", node.emotion);

        }

        Handles.EndGUI();
        SceneView.RepaintAll();

        

    }


}
