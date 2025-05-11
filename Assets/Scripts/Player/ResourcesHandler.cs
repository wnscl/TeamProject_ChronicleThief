using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesHandler : MonoBehaviour
{
    [SerializeField] private float healthChangeDelay = 0.5f;

    private PlayerController playerController;
    private PlayerStats playerStats;
    private PlayerAnimationHandler playerAnimationHandler;

    private float timeSinceLastChange = float.MaxValue;

    public float CurrentHealth { get; private set; }
    public float MaxHealth => playerStats.Health;

    [Header("Gold")]
    [SerializeField] private int gold = 0;
    public int Gold => gold; // 읽기전용

    // 골드 변경 시 UI 등에 알릴 이벤트
    //public event Action<int> OnGoldChanged;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        playerStats = GetComponent<PlayerStats>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
    }

    private void Start()
    {
        CurrentHealth = playerStats.Health;
    }

    private void Update()
    {
        if (timeSinceLastChange < healthChangeDelay)
        {
            timeSinceLastChange += Time.deltaTime;
            if (timeSinceLastChange >= healthChangeDelay)
            { 
                playerAnimationHandler.InvincibilityEnd(); 
            }
        }
    }

    public bool ChangeHealth(float change)
    {
        if (change == 0 | timeSinceLastChange < healthChangeDelay)
            return false;

        timeSinceLastChange = 0;

        CurrentHealth += change;
        CurrentHealth = CurrentHealth > MaxHealth ? MaxHealth : CurrentHealth;
        CurrentHealth = CurrentHealth < 0 ? 0 : CurrentHealth;

        if (change < 0)
        {
            playerAnimationHandler.PlayDamage();
        }

        if (CurrentHealth <= 0f)
            Death();


        return true;
    }

    public void AddGold(int amount) // 몬스터 처치 함수에 추가해주면 됨.
    {
        if (amount <= 0) return;

        gold += amount;
        //OnGoldChanged?.Invoke(gold); // 이벤트로 UI 알림
        Debug.Log($"Gold Added : {amount} | Total : {gold}");
    }

    public void SpendGold()//public bool SpendGold(int amount) 상점 부분에 넣어주면 됨.
    {
        //if (amount <= 0 || gold < amount) return false; // 잔고 부족 시 실패

        //gold -= amount; //음수방지
        //OnGoldChanged?.Invoke(gold); // 이벤트로 UI 알림
        //Debug.Log($"Gold Spent: {amount} | Remaining: {gold}");
        //return true;
    }

    private void Death()
    {

    }
}
