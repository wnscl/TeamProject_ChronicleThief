using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Range(1, 1000)][SerializeField] private int health = 100;
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, 1000); }

    [Range(1f, 10f)][SerializeField] private float speed = 5f;
    public float Speed { get => speed; set => speed = Mathf.Clamp(value, 0, 10); }

    //�÷��̾� ��ü ���ݷ�
    [SerializeField] private float playerAttackPower = 10f;
    public float PlayerAttackPower { get => playerAttackPower; set => playerAttackPower = Mathf.Max(1, value); }

    //�÷��̾� ��ü ����
    [SerializeField] private float playerDefensePower = 5;
    public float PlayerDefensePower { get => playerDefensePower; set => playerDefensePower = Mathf.Max(0, value); }

    // ���� ���� ���� ���� �÷��̾� ��ü �������� ��ü

}
