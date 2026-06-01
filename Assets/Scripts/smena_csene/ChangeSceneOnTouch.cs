using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTouch : MonoBehaviour
{
    [Header("Target Scene")]
    [Tooltip("Выберите сцену для перехода")]
    public SceneName targetScene;

    [Header("Settings")]
    public bool requireTag = true;
    public string requiredTag = "Player";

    [Header("Transition Type")]
    public TransitionType transitionType = TransitionType.OneWay;

    public enum SceneName
    {
        schena_1,
        schena_2,
        schena_3,
        schena_4,
        schena_5
    }

    public enum TransitionType
    {
        OneWay,      // Только вперед, нельзя вернуться
        TwoWay,      // Можно вернуться обратно
        OneWayFrom   // Только если пришел с определенной сцены
    }

    [Header("OneWayFrom Settings (если выбран этот тип)")]
    public SceneName allowedFromScene;  // С какой сцены можно перейти

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!requireTag || other.CompareTag(requiredTag))
        {
            // Проверяем, можно ли перейти
            if (CanTransition())
            {
                string sceneToLoad = GetSceneName(targetScene);
                Debug.Log($"Переход на сцену: {sceneToLoad}");
                SceneManager.LoadScene(sceneToLoad);
            }
            else
            {
                Debug.Log("Переход запрещен!");
            }
        }
    }

    private bool CanTransition()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        switch (transitionType)
        {
            case TransitionType.OneWay:
                // Всегда можно перейти
                return true;

            case TransitionType.TwoWay:
                // Всегда можно перейти
                return true;

            case TransitionType.OneWayFrom:
                // Можно перейти только с разрешенной сцены
                return currentScene == GetSceneName(allowedFromScene);

            default:
                return true;
        }
    }

    private string GetSceneName(SceneName scene)
    {
        switch (scene)
        {
            case SceneName.schena_1: return "schena 1";
            case SceneName.schena_2: return "schena 2";
            case SceneName.schena_3: return "schena 3";
            case SceneName.schena_4: return "schena 4";
            case SceneName.schena_5: return "schena 5";
            default: return "schena 1";
        }
    }
}