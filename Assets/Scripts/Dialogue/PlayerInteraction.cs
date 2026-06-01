using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public GameObject currentNPC;

    void Update()
    {
        if (currentNPC != null && Input.GetKeyDown(KeyCode.E))
        {
            currentNPC.GetComponent<DialogueTrigger>().TriggerDialogue();
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
