using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHandler : MonoBehaviour
{
    [SerializeField] private float weaponSize = 1f;
    public float WeaponSize { get =>  weaponSize; set { weaponSize = value; } }

    public LayerMask target;

    private static readonly int IsAttack = Animator.StringToHash("IsAttack");

    public PlayerController playerController {  get; private set; }

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();

    }

}
