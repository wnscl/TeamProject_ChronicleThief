using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class HealthSystem : MonoBehaviour
{
    public static HealthSystem Instance;

    [Header("체력 게이지")]
    public Image currentHealthBar;    // 현재 체력 바 이미지
    public Text healthText;           // 체력 텍스트
    public int maxHitPoint;  // 최대 체력치 (이제 값을 따로 지정하지 않고, 플레이어 스탯에서 가져옴)

    [Header("마나 게이지")]
    public Image currentManaBar;      // 현재 마나 바 이미지
    public Text manaText;             // 마나 텍스트
    public int maxManaPoint = 100; // 최대 마나치

    [Header("자동 회복 설정")]
    public bool Regenerate = true;         // 자동 회복 사용 여부
    public int regen = 1;                  // 회복량(체력/마나)
    private float timeleft = 0.0f;         // 회복 간격 타이머
    public int regenUpdateInterval = 2; // 회복 간격(초)

    [Header("무적 모드")]
    public bool GodMode; // 참이면 최대치로 자동 회복

    public PlayerStats playerStats;

    void Awake()
    {
        // 싱글톤 초기화
        Instance = this;

        var playerObj = GameObject.FindWithTag("Player");
        if (playerObj != null)
            playerStats = playerObj.GetComponent<PlayerStats>();
        else
            Debug.LogError("HealthSystem: Player 태그를 가진 오브젝트를 찾을 수 없습니다.");

        if (playerStats == null)
            Debug.LogError("HealthSystem: PlayerStats 컴포넌트를 찾을 수 없습니다.");

        maxHitPoint = playerStats.MaxHealth; // maxHitPoint에 플레이어 스탯 반영
    }

    void Start()
    {
        // 초기 그래픽 업데이트 및 타이머 초기화
        UpdateGraphics();
        timeleft = regenUpdateInterval;
    }

    void Update()
    {
        // 자동 회복 로직 실행
        if (Regenerate)
            Regen();
    }

    // 자동 회복 처리
    private void Regen()
    {
        timeleft -= Time.deltaTime;

        if (timeleft <= 0.0f)
        {
            if (GodMode)
            {
                // 무적 모드일 때는 최대치로 채움
                HealDamage(maxHitPoint);
                RestoreMana(maxManaPoint);
            }
            else
            {
                // 일반 회복
                HealDamage(regen);
                RestoreMana(regen);
            }

            UpdateGraphics();
            timeleft = regenUpdateInterval;
        }
    }

    //==============================================================
    // 체력 관련
    //==============================================================

    // 체력 바 위치 및 텍스트 갱신
    private void UpdateHealthBar()
    {
        if (playerStats == null) return;
        float current = playerStats.Health;
        float ratio = current / (float)maxHitPoint;
        float barWidth = currentHealthBar.rectTransform.rect.width;
        currentHealthBar.rectTransform.localPosition =
            new Vector3(barWidth * ratio - barWidth, 0, 0);
        healthText.text = $"{current:0}/{maxHitPoint:0}";
    }

    public void ModifyHealth(int value)
    {
        if (playerStats == null) return;

        // 실제 체력값 변경 (PlayerStats.Health는 MaxHealth 기준으로 clamp 처리됨)
        playerStats.Health += value;

        // UI 갱신
        UpdateGraphics();
    }

    // 데미지 처리
    public void TakeDamage(int damage)
    {
        if (playerStats == null) return;
        playerStats.Health -= damage;
        UpdateGraphics();
    }

    // 회복 처리
    public void HealDamage(int heal)
    {
        if (playerStats == null) return;
        playerStats.Health += heal;
        UpdateGraphics();
    }

    // 최대 체력 증가 (고정량 기반)
    public void SetMaxHealth(int amount)
    {
        maxHitPoint += amount;
        playerStats.MaxHealth = maxHitPoint; // 동시에 플레이어 스탯에도 값을 저장
        UpdateGraphics();
    }

    //==============================================================
    // 마나 관련
    //==============================================================

    // 마나 바 위치 및 텍스트 갱신
    private void UpdateManaBar()
    {
        float ratio = 0f;
        if (maxManaPoint > 0) ratio = maxManaPoint / maxManaPoint; // 단순 전체 채움 예시
        float barWidth = currentManaBar.rectTransform.rect.width;
        currentManaBar.rectTransform.localPosition =
            new Vector3(barWidth * ratio - barWidth, 0, 0);
        manaText.text = $"{maxManaPoint:0}/{maxManaPoint:0}";
    }

    // 마나 소모
    public void UseMana(int cost)
    {
        if (playerStats == null) return;
        // 별도 Mana 속성이 없을 경우, 구현 생략
        UpdateGraphics();
    }

    // 마나 회복
    public void RestoreMana(int amount)
    {
        // 마나 로직 필요 시 연결
        UpdateGraphics();
    }

    // 최대 마나 증가 (퍼센트 기반)
    public void SetMaxMana(float percent)
    {
        maxManaPoint += (int)(maxManaPoint * percent / 100);
        UpdateGraphics();
    }

    //==============================================================
    // 그래픽(체력/마나 바) 전체 갱신
    //==============================================================
    public void UpdateGraphics()
    {
        UpdateHealthBar();
        UpdateManaBar();
    }
}
