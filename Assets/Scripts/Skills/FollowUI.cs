using UnityEngine;

public class FollowUI : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0, 2f, 0); 
    private RectTransform uiRect;

    void Start()
    {
        uiRect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        if (player == null) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(player.position + offset);
        uiRect.position = screenPos;
    }
}
