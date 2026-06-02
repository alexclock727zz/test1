using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject currentNPC;

    private DialogueManager dialogueManager;

    void Start()
    {
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    void Update()
    {
        if (currentNPC != null && Input.GetKeyDown(KeyCode.E))
        {
            DialogueTrigger trigger = currentNPC.GetComponent<DialogueTrigger>();
            if (trigger != null)
                trigger.TriggerDialogue();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("NPC"))
            currentNPC = other.gameObject;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("NPC") && currentNPC == other.gameObject)
            currentNPC = null;
    }
}
