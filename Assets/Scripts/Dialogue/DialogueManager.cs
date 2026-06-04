using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dialogueText;
    public Text nameText;
    public Animator boxAnim;
    public Animator startAnim;

    private Queue<DialogueSentence> sentences;
    private bool isDialogueActive = false;

    // Параметры рандомизации
    public bool randomOrder = false;        // перемешивать ли предложения
    public int maxSentencesToShow = 0;      // 0 = все, иначе ограничить количество

    void Start()
    {
        sentences = new Queue<DialogueSentence>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        if (isDialogueActive) EndDialogue();
        isDialogueActive = true;

        boxAnim.SetBool("boxOpen", true);
        startAnim.SetBool("startOpen", false);

        nameText.text = dialogue.name;
        sentences.Clear();

        List<DialogueSentence> list = new List<DialogueSentence>(dialogue.sentences);

        // ---------- НОВАЯ ЛОГИКА ----------
        // Если диалог называется "Подсолнухи" и флаг hasKey уже true,
        // то показываем только первые две реплики (убираем третью)
        if (dialogue.name == "Подсолнухи" && GameState.hasKey)
        {
            // Оставляем только первые два предложения, если их не меньше двух
            if (list.Count >= 2)
                list.RemoveRange(2, list.Count - 2);
        }
        // ---------------------------------

        // Далее идёт существующий код рандомизации и ограничения
        if (randomOrder)
        {
            for (int i = 0; i < list.Count; i++)
            {
                DialogueSentence temp = list[i];
                int rand = Random.Range(i, list.Count);
                list[i] = list[rand];
                list[rand] = temp;
            }
        }

        int count = (maxSentencesToShow > 0 && maxSentencesToShow < list.Count) ? maxSentencesToShow : list.Count;
        for (int i = 0; i < count; i++)
        {
            sentences.Enqueue(list[i]);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        DialogueSentence sentence = sentences.Dequeue();

        // Выполняем действие, связанное с предложением
        sentence.onSentenceTrigger?.Invoke();

        // Обрабатываем случайное число
        string processedText = sentence.rawText;
        if (sentence.randomizeNumber)
        {
            int randomValue = Random.Range((int)sentence.randomRange.x, (int)sentence.randomRange.y + 1);
            processedText = processedText.Replace("{rand}", randomValue.ToString());
            // Сохраняем в глобальную переменную, если указано имя
            if (!string.IsNullOrEmpty(sentence.numberVariable))
            {
                // Можно использовать рефлексию или словарь, здесь упрощённо:
                if (sentence.numberVariable == "score")
                    GameState.score = randomValue;
                // Добавьте другие переменные по необходимости
            }
        }

        // Подстановка значений из GameState (пример: {score})
        processedText = processedText.Replace("{score}", GameState.score.ToString());
        processedText = processedText.Replace("{hasKey}", GameState.hasKey.ToString());

        // Применяем цвет и размер
        dialogueText.color = sentence.textColor;
        dialogueText.fontSize = sentence.fontSize;

        StopAllCoroutines();
        StartCoroutine(TypeSentence(processedText));
    }

    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public void EndDialogue()
    {
        isDialogueActive = false;
        boxAnim.SetBool("boxOpen", false);
        dialogueText.text = "";
        nameText.text = "";
        dialogueText.color = Color.white; // сброс цвета
        dialogueText.fontSize = 40;       // сброс размера
    }
}