using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ScreenTextEffect : MonoBehaviour
{
    public static ScreenTextEffect Instance;

    [Header("UI Elements (TMP)")]
    public CanvasGroup canvasGroup;
    public TMP_Text backgroundText;
    public TMP_Text centerText;
    public Image darkenImage;               // затемняющий фон

    [Header("Settings")]
    public float globalFadeInTime = 2f;
    public float typingSpeed = 0.03f;
    public float finalMessageDelay = 0.5f;
    public float finalMessageDuration = 3f;
    public float globalFadeOutTime = 1.5f;

    [Header("Затемнение фона")]
    [Range(0f, 1f)] public float maxDarkenAlpha = 0.7f;   // максимальная затемнённость

    [TextArea(10, 30)]
    public string fullScreenText = @"...";

    [Header("Финальная фраза")]
    public string finalMessage = "Да ну нафиг не буду я это читать";
    public Color finalMessageColor = Color.yellow;
    public float finalMessageScale = 1.5f;
    public float finalMessagePulseSpeed = 2f;

    private Coroutine activeCoroutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0f;
        if (darkenImage != null)
        {
            Color c = darkenImage.color;
            c.a = 0f;
            darkenImage.color = c;
        }
        gameObject.SetActive(false);
    }

    public void ShowEffect()
    {
        if (activeCoroutine != null)
            StopCoroutine(activeCoroutine);
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        activeCoroutine = StartCoroutine(EffectCoroutine());
    }

    private IEnumerator EffectCoroutine()
    {
        canvasGroup.alpha = 0f;
        if (darkenImage != null)
        {
            Color c = darkenImage.color;
            c.a = 0f;
            darkenImage.color = c;
        }

        // Подготовка текста
        backgroundText.text = fullScreenText;
        backgroundText.maxVisibleCharacters = 0;
        backgroundText.lineSpacing = -15f;

        centerText.gameObject.SetActive(false);
        centerText.text = finalMessage;
        centerText.color = finalMessageColor;
        centerText.transform.localScale = Vector3.one;

        // --- 1. Плавное появление затемнения и текстового блока ---
        float timer = 0f;
        while (timer < globalFadeInTime)
        {
            timer += Time.deltaTime;
            float alpha = Mathf.Clamp01(timer / globalFadeInTime);
            canvasGroup.alpha = alpha;
            if (darkenImage != null)
            {
                Color c = darkenImage.color;
                c.a = alpha * maxDarkenAlpha;
                darkenImage.color = c;
            }
            yield return null;
        }
        canvasGroup.alpha = 1f;
        if (darkenImage != null)
        {
            Color c = darkenImage.color;
            c.a = maxDarkenAlpha;
            darkenImage.color = c;
        }

        // --- 2. Побуквенная печать ---
        int totalChars = backgroundText.text.Length;
        for (int i = 0; i <= totalChars; i++)
        {
            backgroundText.maxVisibleCharacters = i;
            yield return new WaitForSeconds(typingSpeed);
        }

        yield return new WaitForSeconds(finalMessageDelay);

        // --- 3. Финальная фраза с пульсацией ---
        centerText.gameObject.SetActive(true);
        StartCoroutine(PulseFinalMessage());

        yield return new WaitForSeconds(finalMessageDuration);

        // --- 4. Плавное исчезновение всего (включая затемнение) ---
        timer = 0f;
        while (timer < globalFadeOutTime)
        {
            timer += Time.deltaTime;
            float alpha = 1f - Mathf.Clamp01(timer / globalFadeOutTime);
            canvasGroup.alpha = alpha;
            if (darkenImage != null)
            {
                Color c = darkenImage.color;
                c.a = alpha * maxDarkenAlpha;
                darkenImage.color = c;
            }
            yield return null;
        }

        // --- 5. Сброс и выключение ---
        canvasGroup.alpha = 0f;
        if (darkenImage != null)
        {
            Color c = darkenImage.color;
            c.a = 0f;
            darkenImage.color = c;
        }
        backgroundText.maxVisibleCharacters = 0;
        centerText.gameObject.SetActive(false);
        gameObject.SetActive(false);
        activeCoroutine = null;
    }

    private IEnumerator PulseFinalMessage()
    {
        float time = 0f;
        Vector3 startScale = Vector3.one * finalMessageScale;
        while (centerText.gameObject.activeSelf && gameObject.activeSelf)
        {
            time += Time.deltaTime * finalMessagePulseSpeed;
            float scale = 1f + Mathf.Sin(time) * 0.1f;
            centerText.transform.localScale = startScale * scale;
            yield return null;
        }
        centerText.transform.localScale = startScale;
    }
}