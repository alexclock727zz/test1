using UnityEngine;

public class DialogueFollower : MonoBehaviour
{
    public Transform targetNPC;
    public Camera mainCamera;
    public Vector3 worldOffset = new Vector3(0, 2f, 0);

    private RectTransform rectTransform;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main;
        targetNPC = null;
    }

    void Update()
    {
        if (targetNPC != null && mainCamera != null)
        {
            // Преобразуем мировую позицию NPC + смещение в экранные координаты
            Vector3 worldPoint = targetNPC.position + worldOffset;
            Vector2 screenPoint = mainCamera.WorldToScreenPoint(worldPoint);
            // Назначаем новые координаты RectTransform'у
            rectTransform.position = screenPoint;
        }
    }
    public void SetTarget(Transform npc)
    {
        targetNPC = npc;
    }
    public void ClearTarget()
    {
        targetNPC = null;
    }
}