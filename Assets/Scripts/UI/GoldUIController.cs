using UnityEngine;
using TMPro;

public class GoldUIController : MonoBehaviour
{
    [SerializeField] private TMP_Text goldText;

    void Awake()
    {
        if (goldText == null)
            goldText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // ResourcesHandler �̱��濡�� ���� ��� �о�ͼ� ������Ʈ
        int gold = ResourcesHandler.Instance != null
                   ? ResourcesHandler.Instance.Gold
                   : 0;
        goldText.text = $"{gold}";
    }
}