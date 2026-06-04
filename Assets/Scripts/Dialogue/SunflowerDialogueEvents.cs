using UnityEngine;
using System.Collections;

public class SunflowerDialogueEvents : MonoBehaviour
{
    public GameObject noteObject;        // Объект заметки, который надо показать (zametki untitled)
    public float displayDuration = 1.75f;   // Время отображения заметки
    public float delayBeforeShow = 0.75f; // Задержка перед появлением заметки

    private bool alreadyTriggered = false;

    // Этот метод вызывается из onSentenceTrigger второй реплики
    public void OnSecondSentence()
    {
        if (alreadyTriggered) return;     // Чтобы заметка не появлялась повторно

        alreadyTriggered = true;

        // Устанавливаем значения в GameState
        GameState.score = 2;
        GameState.hasKey = true;          // Этот флаг будет означать, что «первый раз вылетел объект»

        // Показываем заметку
        if (noteObject != null)
        {
            StartCoroutine(ShowNoteWithDelay());
        }
    }

    private IEnumerator ShowNoteWithDelay()
    {
        yield return new WaitForSeconds(delayBeforeShow);
        noteObject.SetActive(true);
        StartCoroutine(HideNoteAfterDelay());
    }

    private IEnumerator HideNoteAfterDelay()
    {
        yield return new WaitForSeconds(displayDuration);
        if (noteObject != null) noteObject.SetActive(false);
    }
}