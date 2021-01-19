using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue Text Set")]
public class DialogueTextSet : ScriptableObject
{
    [TextArea(10, 15)] [SerializeField] public string dialogueText;
    [SerializeField] public bool isDecisionPoint, isEndPoint;
    [SerializeField] public DialogueTextSet[] dialogueTextSetList;


    public void SetDialogueInfo(string str)
    {
        dialogueText = str;
    }

    public void SetDialogueInfo(string str, bool dec)
    {
        dialogueText = str;
        isDecisionPoint = dec;
    }

    public void SetDialogueInfo(string str, bool dec, bool end)
    {
        dialogueText = str;
        isDecisionPoint = dec;
        isEndPoint = end;
    }

    public string GetDialogueTxt()
    {
        return dialogueText;
    }

    public DialogueTextSet[] GetNextDialogueTextSetList()
    {
        return dialogueTextSetList;
    }
}
