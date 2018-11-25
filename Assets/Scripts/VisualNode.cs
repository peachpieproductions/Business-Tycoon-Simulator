using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using TMPro;

[System.Serializable]
public class TextVariant {
    [TextArea(1, 10)]
    public string text;
    public NPCData npc;
    //public ConversationNodeData.Emotion emotion;
}

public enum Comparison {
    GreaterThan, LessThan
}


[ExecuteInEditMode]
public class VisualNode : MonoBehaviour {

    public bool rootNode;
    public string rootNodeName;
    public List<TextVariant> text = new List<TextVariant>();
    public ConversationNodeData.Emotion emotion;
    public bool response;
    public List<VisualNode> responses = new List<VisualNode>();
    public List<VisualNode> responseTo = new List<VisualNode>();
    public Color nodeColorOverride = Color.white;
    public string NodeVisualTitle;
    public string nodeNote;
    public Sprite nodeIconSprite;
    [HideInInspector] public string dataName;
    [HideInInspector] public bool showEditingHud = true;
    [HideInInspector] public Comparison relPercentCompare;
    [HideInInspector] public float relationshipPercReq;

    [Header("References")]
    public ConversationNodeData data;
    public TextMeshPro nodeText;
    public TextMeshPro titleText;
    public TextMeshPro noteText;
    public TextMeshPro relationshipReqText;
    public SpriteRenderer nodeIcon;
    
    [HideInInspector] public VisualNodeManager nodeManager;

    private void Awake() {
        nodeManager = FindObjectOfType<VisualNodeManager>();
    }

    private void Update() {

        if (nodeColorOverride != Color.white) {
            GetComponent<SpriteRenderer>().color = nodeColorOverride;
        } else {
            if (response) GetComponent<SpriteRenderer>().color = new Color(.7f, 1f, 1f);
            else if (rootNode) GetComponent<SpriteRenderer>().color = new Color(.7f, 1f, .7f);
            else GetComponent<SpriteRenderer>().color = Color.white;
        }

        if (nodeIconSprite) { nodeIcon.sprite = nodeIconSprite; nodeIcon.color = Color.white; }
        else {
            if (emotion == ConversationNodeData.Emotion.Nice) { nodeIcon.sprite = nodeManager.emotionIcons[0]; nodeIcon.color = new Color(.7f, 1f, .7f); }
            else if (emotion == ConversationNodeData.Emotion.Mean) { nodeIcon.sprite = nodeManager.emotionIcons[1]; nodeIcon.color = new Color(1f, .8f, .8f); }
            else nodeIcon.sprite = null;
        }

        if (relationshipPercReq > 0) {
            if (relPercentCompare == Comparison.GreaterThan) relationshipReqText.text = "> ";
            else relationshipReqText.text = "< ";
            relationshipReqText.text += relationshipPercReq;
        } else relationshipReqText.text = "";

        if (text.Count > 0) {
            if (text[0].text != "") nodeText.text = text[0].text;
            else nodeText.text = "(New Node)";
            if (data) {
                data.text = text;
            }
            titleText.text = NodeVisualTitle;
            if (nodeNote != "") noteText.text = "Note: " + nodeNote;
            else noteText.text = "";
        }
        if (data) {
            data.responses.Clear();
            data.responseTo.Clear();
            foreach (VisualNode vn in responses) { if (vn) data.responses.Add(vn.data); else responses.Remove(vn); }
            foreach (VisualNode vn in responseTo) { if (vn) data.responseTo.Add(vn.data); else responseTo.Remove(vn); }
        }
    }

    

    public void OnDestroy() {
        foreach(VisualNode n in responseTo) {
            n.responses.Remove(this);
            n.data.responses.Remove(data);
        }
        foreach (VisualNode n in responses) {
            n.responseTo.Remove(this);
            n.data.responseTo.Remove(data);
        }
    }


}
