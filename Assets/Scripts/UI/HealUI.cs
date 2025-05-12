using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class HealUI : MonoBehaviour
{
    public static HealUI Instance { get; private set; }

    [Header("�г�")]
    public GameObject panel;
    public TMP_Text goldText;

    [Header("ȸ�� ��ư")]
    public Button heal10Btn;
    public Button heal25Btn;
    public Button heal50Btn;
    public Button healFullBtn;

    [Header("��� �� ���� �ؽ�Ʈ")]
    public TMP_Text cost10Text;
    public TMP_Text cost25Text;
    public TMP_Text cost50Text;
    public TMP_Text costFullText;

    [Header("����")]
    public float[] ratios = { 0.1f, 0.25f, 0.5f, 1f };
    public int baseCost = 100;

    [Header("���� ������Ʈ")]
    public ResourcesHandler resourcesHandler;
    public HealthSystem healthSystem;
    private PlayerStats playerStats;

    private bool[] used = new bool[4];  // ��ư ��� �̷�

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

    // NPC ���ȯ �� �� ���� ȣ��
    public void ResetHeals()
    {
        for (int i = 0; i < used.Length; i++) used[i] = false;
    }

    // �� UI ����
    public void Show()
    {
        panel.SetActive(true);

        // �ݾҴ� �ٽ� ��� ��� �̷� ����
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
            UIManager.Instance.ShowDialog("ġ����", "�̹� �̿��ϼ̽��ϴ�!");
            return;
        }

        if (playerStats.Health >= healthSystem.maxHitPoint)
        {
            UIManager.Instance.ShowDialog("ġ����", "ü���� �̹� �����մϴ�!");
            return;
        }

        int cost = Mathf.CeilToInt(baseCost * ratios[idx]);
        if (!resourcesHandler.SpendGold(cost))
        {
            UIManager.Instance.ShowDialog("ġ����", "��尡 �����մϴ�!");
            return;
        }

        int healAmt = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[idx]);
        float missing = healthSystem.maxHitPoint - playerStats.Health;
        if (healAmt > missing) healAmt = Mathf.CeilToInt(missing);

        healthSystem.HealDamage(healAmt);
        UIManager.Instance.ShowDialog("ġ����", $"{healAmt}��ŭ ȸ���Ǿ����ϴ�.");

        used[idx] = true;   // ��� ��� �����
        Show();             // ��ư ����, �ؽ�Ʈ ����
    }

    private void UpdateAllTexts()
    {
        int c10 = Mathf.CeilToInt(baseCost * ratios[0]);
        int h10 = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[0]);
        cost10Text.text = $"ȸ����: {h10}HP, ���: {c10}G";

        int c25 = Mathf.CeilToInt(baseCost * ratios[1]);
        int h25 = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[1]);
        cost25Text.text = $"ȸ����: {h25}HP, ���: {c25}G";

        int c50 = Mathf.CeilToInt(baseCost * ratios[2]);
        int h50 = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[2]);
        cost50Text.text = $"ȸ����: {h50}HP, ���: {c50}G";

        int cf = baseCost;
        int hf = Mathf.CeilToInt(healthSystem.maxHitPoint * ratios[3]);
        costFullText.text = $"ȸ����: {hf}HP, ���: {cf}G";

        goldText.text = $"Gold: {resourcesHandler.Gold}G";
    }

    // �� UI �ݱ�
    public void Hide()
    {
        panel.SetActive(false);
    }
}
