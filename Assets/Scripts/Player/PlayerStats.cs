using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Range(1, 200)][SerializeField] private int health = 100;
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, 200); }

    [Range(1f, 10f)][SerializeField] private float speed = 5f;
    public float Speed { get => speed; set => speed = Mathf.Clamp(value, 0, 10); }

    //플레이어 자체 공격력
    //플레이어 자체 방어력

}
