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
        // ResourcesHandler 싱글톤에서 현재 골드 읽어와서 업데이트
        int gold = ResourcesHandler.Instance != null
                   ? ResourcesHandler.Instance.Gold
                   : 0;
        goldText.text = $"{gold}";
    }
}