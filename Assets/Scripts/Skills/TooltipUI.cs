using UnityEngine;
using UnityEngine.UI;

public class TooltipUI : MonoBehaviour
{
    public static TooltipUI Instance;

    public GameObject panel;
    public Text tooltipText;

    void Awake()
    {
        Instance = this;
        Hide();
    }

    public void Show(string content, Vector3 position)
    {
        panel.SetActive(true);
        tooltipText.text = content;
        panel.transform.position = position;
    }

    public void Hide()
    {
        panel.SetActive(false);
    }
}
