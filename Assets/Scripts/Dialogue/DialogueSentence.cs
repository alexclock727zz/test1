using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class DialogueSentence
{
    [TextArea(1, 5)]
    public string rawText;                     // исходный текст с плейсхолдерами
    public Color textColor = Color.white;      // цвет текста
    public int fontSize = 40;                  // размер шрифта
    public bool randomizeNumber = false;       // флаг, что нужно вставить случайное число
    public Vector2 randomRange = new Vector2(0, 100); // диапазон случайного числа
    public string numberVariable = "";         // имя переменной, куда сохранить сгенерированное число (опционально)
    public UnityEvent onSentenceTrigger;       // действие при запуске этой реплики (смена bool, увеличение счётчика)
}