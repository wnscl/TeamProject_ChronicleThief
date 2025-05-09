using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Setting")]
    //[SerializeField] private float moveSpeed = 5f;

    [Header("Component References")]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform WeaponPivot;

    private Rigidbody2D _rigidbody;
    private Camera _camera;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;

    public Vector2 MovementDirection => movementDirection;
    public Vector2 LookDirection => lookDirection;

    //private Animator animator;
    //private bool isAttack = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        //animator = GetComponent<Animator>();
    }

    private void Update()
    {
        HandleInput();
        RotateCharacter();
    }

    private void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleInput()
    {
        
    }

    private void RotateCharacter()
    {

    }

    private void HandleMovement()
    {

    }
}
