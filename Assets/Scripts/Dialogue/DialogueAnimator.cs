using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueAnimator : MonoBehaviour
{
    public Animator startAnim;
    public DialogueManager dm;
    public GameObject dialogueWindow;

    private DialogueFollower follower;

    private void Start()
    {
        // Находим компонент слежения на окне (если он ещё не привязан)
        if (dialogueWindow != null)
            follower = dialogueWindow.GetComponent<DialogueFollower>();
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if (follower != null)
            follower.SetTarget(transform);
        if (dm != null)
        {
            dm.dialogueText.text = "";
            dm.nameText.text = "";
        }
        dialogueWindow.SetActive(true);
        startAnim.SetBool("startOpen", true);
    }
    public void OnTriggerExit2D(Collider2D other)
    {
        startAnim.SetBool("startOpen", false);
        dm.EndDialogue();
        if (follower != null)
            follower.ClearTarget();
    }
}
