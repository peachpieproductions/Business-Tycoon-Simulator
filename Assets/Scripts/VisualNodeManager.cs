using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;
using System.IO;

[ExecuteInEditMode]
public class VisualNodeManager : MonoBehaviour {

    public GameObject nodePrefab;
    public GameObject nodeLinePrefab;
    public Object nodeData;
    public Sprite[] emotionIcons;
    public LineRenderer connectNodeLine;
    public VisualNode connectNodeFrom;
    public VisualNode connectNodeTo;

    public GameObject CreateNewNode() {
        var inst = PrefabUtility.InstantiatePrefab(nodePrefab) as GameObject;
        inst.transform.position = SceneView.lastActiveSceneView.camera.transform.position;
        inst.transform.position = new Vector3(inst.transform.position.x, inst.transform.position.y, 0);

        var so = CreateNewData();

        inst.GetComponent<VisualNode>().data = so as ConversationNodeData;
        inst.GetComponent<VisualNode>().dataName = inst.GetComponent<VisualNode>().data.name;

        return inst;
    }

    public ScriptableObject CreateNewData() {
        DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Data/Conversations");
        var num = dirInfo.GetFiles().Length / 2;

        var so = ScriptableObject.CreateInstance("ConversationNodeData");
        AssetDatabase.CreateAsset(so, "Assets/Data/Conversations/NodeData" + num + ".asset");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        return so;
    }

    public LineRenderer CreateNewLineConnection(Transform from, Transform to) {
        var newLine = Instantiate(nodeLinePrefab);
        newLine.GetComponent<VisualNodeLine>().from = from.transform;
        newLine.GetComponent<VisualNodeLine>().to = to.transform;
        var lineParent = GameObject.Find("LINES");
        if (lineParent == null) lineParent = new GameObject("LINES");
        newLine.transform.parent = lineParent.transform;
        return newLine.GetComponent<LineRenderer>();
    }


}
