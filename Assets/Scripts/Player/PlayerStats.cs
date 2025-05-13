using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int health = 100;
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, MaxHealth); } // �ִ�ü�� �κ� MaxHealth ������ �����߽��ϴ�.

    [SerializeField] private int maxHealth = 100;
    public int MaxHealth { get => maxHealth;  set => maxHealth = Mathf.Max(1, value); } // �ִ� ü�� �ʵ� �߰�

    [Range(1f, 10f)][SerializeField] private float speed = 5f; // �̵� �ӵ�
    public float Speed { get => speed; set => speed = Mathf.Clamp(value, 0, 10); }

    //[SerializeField] private float attackRange = 10f; // ���� �Ÿ�
    //public float AttackRange { get => attackRange; set => attackRange = value; }

    //�÷��̾� ��ü ���ݷ�
    [SerializeField] private float playerAttackPower = 10f;
    public float PlayerAttackPower { get => playerAttackPower; set => playerAttackPower = Mathf.Max(1, value); }

    //�÷��̾� ��ü ����
    [SerializeField] private float playerDefensePower = 5;
    public float PlayerDefensePower { get => playerDefensePower; set => playerDefensePower = Mathf.Clamp(value, 0f, 100f); } // �ִ�ġ 100

}
