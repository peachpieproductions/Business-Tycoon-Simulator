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
