using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ConversationNodeData : ScriptableObject {

    public enum Emotion { Neutral, Nice, Mean }

    public List<TextVariant> text = new List<TextVariant> ();
    public Emotion emotion;
    public bool response;
    public List<ConversationNodeData> responses = new List<ConversationNodeData>();
    public List<ConversationNodeData> responseTo = new List<ConversationNodeData>();

}
