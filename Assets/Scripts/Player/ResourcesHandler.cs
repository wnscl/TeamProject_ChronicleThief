using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcesHandler : MonoBehaviour
{
    public static ResourcesHandler Instance { get; private set; }

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
        if (Instance == null)
            Instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

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
        {
            return false;
        }

        timeSinceLastChange = 0;

        CurrentHealth += change;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, playerStats.Health);

        HealthSystem.Instance.ModifyHealth((int)change);

        if (change < 0)
        {
            Debug.Log($"피해 받음: {-change} 데미지");
            playerAnimationHandler.PlayDamage();
        }

        if (CurrentHealth <= 0f)
        {
            Debug.Log("사망! HP가 0 이하입니다.");
            Death();
        }


        return true;
    }

    public void AddGold(int amount)
    {
        if (amount <= 0) return;

        gold += amount;
        // OnGoldChanged?.Invoke(gold);
        Debug.Log($"Gold Added : {amount} | Total : {gold}");
    }

    public bool SpendGold(int amount)
    {
        if (amount <= 0 || gold < amount)
            return false;

        gold -= amount;
        // OnGoldChanged?.Invoke(gold);
        Debug.Log($"Gold Spent: {amount} | Remaining: {gold}");
        return true;
    }

    public void Death()
    {
        playerController.Die();
    }
}
