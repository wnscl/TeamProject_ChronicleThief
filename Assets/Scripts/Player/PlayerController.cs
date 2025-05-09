using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Setting")]
    [SerializeField] private float moveSpeed = 5f;
    //[SerializeField] private float invincibleDuration = 0.5f; // 무적 시간

    [Header("Component References")]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;
    [SerializeField] private PlayerAnimationHandler playerAnimationHandler;

    private Rigidbody2D _rigidbody;
    private Camera _camera;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;

    //private bool isInvincible = false;
    private bool isDead = false;

    public Vector2 MovementDirection => movementDirection;
    public Vector2 LookDirection => lookDirection;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (isDead) return; // 사망 시 입력 무시

        HandleInput(); // 입력 처리
        RotateCharacter(); // 무기 회전
        UpdateAnimation(); // 애니메이션 업데이트
    }

    private void FixedUpdate()
    {
        if (!isDead)
            HandleMovement(); // 물리 이동 처리
    }

    private void HandleInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        movementDirection = new Vector2(horizontal, vertical).normalized;

        Vector2 mousePosition = Input.mousePosition;
        Vector2 worldPos = _camera.ScreenToWorldPoint(mousePosition);
        lookDirection = (worldPos - (Vector2)transform.position);

        if (lookDirection.magnitude < 0.9f)
            lookDirection = Vector2.zero;
        else
            lookDirection = lookDirection.normalized;
    }

    private void RotateCharacter()
    {
        if (lookDirection == Vector2.zero) return;

        float rotZ = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg;
        bool isLeft = Mathf.Abs(rotZ) > 90f;

        if (characterRenderer != null)
            characterRenderer.flipX = isLeft;

        if (weaponPivot != null)
            weaponPivot.rotation= Quaternion.Euler(0, 0, rotZ);
    }

    private void HandleMovement()
    {
        Vector2 velocity = movementDirection * moveSpeed;
        _rigidbody.velocity = velocity;
    }

    private void UpdateAnimation()
    {
        
    }
}
