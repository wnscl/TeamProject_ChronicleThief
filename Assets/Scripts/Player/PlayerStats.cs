using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int health = 100;
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, MaxHealth); } // 최대체력 부분 MaxHealth 변수로 수정했습니다.

    [SerializeField] private int maxHealth = 100;
    public int MaxHealth { get => maxHealth;  set => maxHealth = Mathf.Max(1, value); } // 최대 체력 필드 추가

    [Range(1f, 10f)][SerializeField] private float speed = 5f; // 이동 속도
    public float Speed { get => speed; set => speed = Mathf.Clamp(value, 0, 10); }

    //[SerializeField] private float attackRange = 10f; // 사정 거리
    //public float AttackRange { get => attackRange; set => attackRange = value; }

    //플레이어 자체 공격력
    [SerializeField] private float playerAttackPower = 10f;
    public float PlayerAttackPower { get => playerAttackPower; set => playerAttackPower = Mathf.Max(1, value); }

    //플레이어 자체 방어력
    [SerializeField] private float playerDefensePower = 5;
    public float PlayerDefensePower { get => playerDefensePower; set => playerDefensePower = Mathf.Clamp(value, 0f, 100f); } // 최대치 100

}
