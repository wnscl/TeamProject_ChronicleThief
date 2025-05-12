using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class HealUI : MonoBehaviour
{
    public static HealUI Instance { get; private set; }

    [Header("패널")]
    public GameObject panel;
    public TMP_Text goldText;

    [Header("회복 버튼")]
    public Button heal10Btn;
    public Button heal25Btn;
    public Button heal50Btn;
    public Button healFullBtn;

    [Header("비용 및 설명 텍스트")]
    public TMP_Text cost10Text;
    public TMP_Text cost25Text;
    public TMP_Text cost50Text;
    public TMP_Text costFullText;

    [Header("설정")]
    public float[] ratios = { 0.1f, 0.25f, 0.5f, 1f };
    public int baseCost = 100;

    [Header("참조 컴포넌트")]
    public ResourcesHandler resourcesHandler;
    public HealthSystem healthSystem;
    private PlayerStats playerStats;

    private bool[] used = new bool[4];  // 버튼 사용 이력

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        panel.SetActive(false);

        if (resourcesHandler == null) resourcesHandler = ResourcesHandler.Instance;
        if (healthSystem == null) healthSystem = HealthSystem.Instance;
        var player = GameObject.FindWithTag("Player");
        if (player != null) playerStats = player.GetComponent<PlayerStats>();

        heal10Btn.onClick.AddListener(() => TryHeal(0));
        heal25Btn.onClick.AddListener(() => TryHeal(1));
        heal50Btn.onClick.AddListener(() => TryHeal(2));
        healFullBtn.onClick.AddListener(() => TryHeal(3));

        UpdateAllTexts();
    }

    // NPC 재소환 시 한 번만 호출
    public void ResetHeals()
    {
        for (int i = 0; i < used.Length; i++) used[i] = false;
    }

    // 힐 UI 열기
    public void Show()
    {
        panel.SetActive(true);

        // 닫았다 다시 열어도 사용 이력 유지
        heal10Btn.interactable = !used[0];
        heal25Btn.interactable = !used[1];
        heal50Btn.interactable = !used[2];
        healFullBtn.interactable = !used[3];

        UpdateAllTexts();
    }

    private void TryHeal(int idx)
    {
        if (used[idx])
        {
            UIManager.Instance.ShowDialog("치유사", "이미 이용하셨습니다!");
            return;
        }

        if (playerStats.Health >= healthSystem.maxHitPoint)
        {
            UIManager.Instance.ShowDialog("치유사", "체력이 이미 가득합니다!");
            return;
        }

        int cost = Mathf.CeilToInt(baseCost * ratios[idx]);
        if (!resourcesHandler.SpendGold(cost))
        {
            UIManager.Instance.ShowDialog("치유사", "골드가 부족합니다!");
            return;
        }

        int healAmt = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[idx]);
        float missing = healthSystem.maxHitPoint - playerStats.Health;
        if (healAmt > missing) healAmt = Mathf.CeilToInt(missing);

        healthSystem.HealDamage(healAmt);
        UIManager.Instance.ShowDialog("치유사", $"{healAmt}만큼 회복되었습니다.");

        used[idx] = true;   // 사용 기록 남기기
        Show();             // 버튼 상태, 텍스트 갱신
    }

    private void UpdateAllTexts()
    {
        int c10 = Mathf.CeilToInt(baseCost * ratios[0]);
        int h10 = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[0]);
        cost10Text.text = $"회복량: {h10}HP, 비용: {c10}G";

        int c25 = Mathf.CeilToInt(baseCost * ratios[1]);
        int h25 = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[1]);
        cost25Text.text = $"회복량: {h25}HP, 비용: {c25}G";

        int c50 = Mathf.CeilToInt(baseCost * ratios[2]);
        int h50 = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[2]);
        cost50Text.text = $"회복량: {h50}HP, 비용: {c50}G";

        int cf = baseCost;
        int hf = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[3]);
        costFullText.text = $"회복량: {hf}HP, 비용: {cf}G";

        goldText.text = $"Gold: {resourcesHandler.Gold}G";
    }

    // 힐 UI 닫기
    public void Hide()
    {
        panel.SetActive(false);
    }
}
