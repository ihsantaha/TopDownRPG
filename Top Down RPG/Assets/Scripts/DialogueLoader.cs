using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    public GameObject mainCamera;
    public GameObject player;
    public GameObject dialogueBox;
    public GameObject dialogue;
    public Fader sceneFader;
    public Fader dialogueOverlayFader;
    public Fader dialogueBoxFader;
    public Fader dialogueFader;
    public bool activate;
    public bool currentStateIsOpen;
    public bool decisionPoint;
    public bool inNarratorMode;
    int moveCameraStep;
    string dialogueType = "Dialogue";
    int currentDialogueFragmentIndex;
    int communicatorID;
    int dialogueFragmentChoiceID = 0;

    void Start()
    {
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        communicatorID = GetComponent<ObjectID>().ID;
        player = GameObject.FindGameObjectWithTag("Player");
        dialogueBox = GameObject.FindGameObjectWithTag("DialogueBox");
        dialogueBox.GetComponent<Transform>().position = new Vector3(0, -3.5f, 0);
        UpdateFrame(0, "DialogueBox");
        sceneFader = GameObject.FindGameObjectWithTag("FrameOverlay").GetComponent<Fader>();
    }

    void Update()
    {
        PlayerIsCloseEnoughToCommunicate();
        InitiateCommunication();
        ManageDialogue();
        ManageCamera();
    }

    public bool PlayerIsCloseEnoughToCommunicate()
    {
        RaycastHit2D playerHitFromLeft = Physics2D.Raycast(transform.position, Vector2.left, 0.55f, 1 << 8);
        RaycastHit2D playerHitFromTop = Physics2D.Raycast(transform.position, Vector2.up, 0.55f, 1 << 8);
        RaycastHit2D playerHitFromRight = Physics2D.Raycast(transform.position, Vector2.right, 0.55f, 1 << 8);
        RaycastHit2D playerHitFromBottom = Physics2D.Raycast(transform.position, Vector2.left, 0.55f, 1 << 8);

        return playerHitFromLeft.collider != null || playerHitFromTop.collider != null || playerHitFromRight.collider != null || playerHitFromBottom.collider != null;
    }

    public void InitiateCommunication()
    {
        var dialogueTextSet = dialogue.GetComponent<Dialogue>().dialogueTextSetList[getDialogue(communicatorID, dialogueFragmentChoiceID)];
        
        if (!currentStateIsOpen && ((Input.GetButton("X Button") && PlayerIsCloseEnoughToCommunicate()) || inNarratorMode))
        {
            if (inNarratorMode)
            {
                StartCoroutine(InitiateNarratorDialogue(dialogueTextSet, 3.0f));
            }
            else
            {
                currentStateIsOpen = true;
                currentDialogueFragmentIndex = getDialogue(communicatorID, dialogueFragmentChoiceID);

                if (player != null)
                {
                    player.GetComponent<PlayerMovement>().moveSpeed = 0;
                }

                Instantiate(dialogue, Vector3.zero, transform.rotation);
                dialogue.GetComponent<Dialogue>().textComponent.text = dialogueTextSet.dialogueText;
            }
        }
    }

    public void ManageDialogue()
    {
        var dialogueTextSetList = dialogue.GetComponent<Dialogue>().dialogueTextSetList;

        if (currentStateIsOpen)
        {
            if (dialogueTextSetList[currentDialogueFragmentIndex].isEndPoint && Input.GetButtonDown("A Button"))
            {
                StartCoroutine(FadeOutOnly(1, dialogueTextSetList, true));
            }
            else if (dialogueTextSetList[currentDialogueFragmentIndex].isDecisionPoint && Input.GetButtonDown("X Button"))
            {
                dialogue.GetComponent<Dialogue>().textComponent.text = dialogueTextSetList[getDialogue(communicatorID, currentDialogueFragmentIndex)].dialogueText;
                currentDialogueFragmentIndex = getDialogue(communicatorID, currentDialogueFragmentIndex);
            }
            else if (Input.GetButtonDown("A Button"))
            {
                currentDialogueFragmentIndex++;
                StartCoroutine(FadeOutOnly(1, dialogueTextSetList));
            }
        }
    }

    public void CloseDialogue()
    {
        currentStateIsOpen = false;
        UpdateFrame(0, "DialogueBox");
        if (player != null)
        {
            player.GetComponent<PlayerMovement>().moveSpeed = 5f;
        }
    }

    public int getDialogue(int commID, int choiceID)
    {
        if (commID == 0)
            return 0;
        if (commID == 1)
        {
            if (choiceID == 0)
                return 0;
            if (choiceID == 2)
                return 5;
        }

        return dialogue.GetComponent<Dialogue>().dialogueTextSetList.Length - 1;
    }

    IEnumerator InitiateNarratorDialogue(DialogueTextSet dialogTextSet, float seconds)
    {
        inNarratorMode = false;
        yield return new WaitForSeconds(seconds);
        currentStateIsOpen = true;
        currentDialogueFragmentIndex = getDialogue(communicatorID, dialogueFragmentChoiceID);

        if (player != null)
        {
            player.GetComponent<PlayerMovement>().moveSpeed = 0;
        }

        dialogueBox.GetComponent<Transform>().position = new Vector3(0, -3.5f, 0);

        Instantiate(dialogue, Vector3.zero, transform.rotation);
        dialogue.GetComponent<Dialogue>().textComponent.text = dialogTextSet.dialogueText;

        dialogueBox.GetComponent<Fader>().StartCoroutine(dialogueBox.GetComponent<Fader>().FadeInOnly(1));
        StartCoroutine(FadeInOnly(1, dialogue.GetComponent<Dialogue>().dialogueTextSetList));

    }

    public IEnumerator FadeOutOnly(float fadeOutDuration)
    {
        yield return new WaitForSeconds(0);

        for (float alpha = fadeOutDuration; alpha >= -0.05; alpha -= 0.05f)
        {
            UpdateFrame(alpha, dialogueType);
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator FadeOutOnly(float fadeOutDuration, DialogueTextSet[] dialogueTextSetList)
    {
        yield return new WaitForSeconds(0);

        for (float alpha = fadeOutDuration; alpha >= -0.05; alpha -= 0.05f)
        {
            UpdateFrame(alpha, dialogueType);
            yield return new WaitForSeconds(0.05f);
        }

        StartCoroutine(FadeInOnly(1, dialogueTextSetList));
    }

    public IEnumerator FadeOutOnly(float fadeOutDuration, DialogueTextSet[] dialogueTextSetList, bool close)
    {
        yield return new WaitForSeconds(0);

        for (float alpha = fadeOutDuration; alpha >= -0.05; alpha -= 0.05f)
        {
            UpdateFrame(alpha, dialogueType);
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(1);
        dialogueBox.GetComponent<Fader>().StartCoroutine(dialogueBox.GetComponent<Fader>().FadeOutOnly(1));
        yield return new WaitForSeconds(2);
        CloseDialogue();

        if (communicatorID == 0) {
            sceneFader.StartCoroutine(sceneFader.FadeOutOnly(1));
            StartCoroutine(MoveCamera());
        }
    }

    public IEnumerator FadeInOnly(float fadeInDuration)
    {
        UpdateFrame(0, dialogueType);
        yield return new WaitForSeconds(1);

        for (float alpha = 0.05f; alpha <= fadeInDuration; alpha += 0.05f)
        {
            UpdateFrame(alpha, dialogueType);
            yield return new WaitForSeconds(0.05f);
        }

    }

    public IEnumerator FadeInOnly(float fadeInDuration, DialogueTextSet[] dialogTextSetList)
    {
        UpdateFrame(0, dialogueType);

        yield return new WaitForSeconds(1);
        dialogue.GetComponent<Dialogue>().textComponent.text = dialogTextSetList[currentDialogueFragmentIndex].dialogueText;

        for (float alpha = 0.05f; alpha <= fadeInDuration; alpha += 0.05f)
        {
            UpdateFrame(alpha, dialogueType);
            yield return new WaitForSeconds(0.05f);
        }

    }


    public void UpdateFrame(float f, string type)
    {
        if (type == "Dialogue")
        {
            Color c = GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>().textComponent.color;
            c.a = f;
            GameObject.FindGameObjectWithTag("Dialogue").GetComponent<Dialogue>().textComponent.material.color = c;
        } else if (type == "DialogueBox")
        {
            Color c = dialogueBox.GetComponent<Fader>().render.material.color;
            c.a = f;
            dialogueBox.GetComponent<Fader>().render.material.color = c;
        }
    }

    public IEnumerator MoveCamera()
    {
       
        yield return new WaitForSeconds(5);
        moveCameraStep = 1;
    }

    public void ManageCamera()
    {
        if (moveCameraStep == 1)
        {
            if (mainCamera.transform.position.y > -9f)
            {
                mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);

                Vector3 yPosition = mainCamera.transform.position;
                yPosition.y -= 0.025f;
                mainCamera.transform.position = yPosition;
            } else
            {
                moveCameraStep = 0;
            }

        }
    }
}
