using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private SpriteRenderer characterRenderer;
    [SerializeField] private Transform weaponPivot;

    protected PlayerAnimationHandler playerAnimationHandler;
    protected PlayerStats playerStats;

    private Rigidbody2D _rigidbody;
    private Camera _camera;

    private Vector2 movementDirection = Vector2.zero;
    private Vector2 lookDirection = Vector2.zero;
    private bool isDead = false;

    // Blink 관련 변수
    private bool isBlink = false;
    //private float blinkDistance = 5f;
    private float blinkCooldown = 5f;
    private float lastBlinkTime = -999f;
    private float blinkDuration = 0.4f;

    public Vector2 MovementDirection => movementDirection;
    public Vector2 LookDirection => lookDirection;
    public bool IsDead => isDead;
    public float BlinkCooldownRemaining => Mathf.Max(0, blinkCooldown - (Time.time - lastBlinkTime)); // 남은 쿨타임 계산
    public bool IsBlink => isBlink;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimationHandler = GetComponent<PlayerAnimationHandler>();
        playerStats = GetComponent<PlayerStats>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (isDead) return; // 사망 시 입력 무시

        HandleInput(); // 입력 처리
        RotateCharacter(); // 무기 회전
        UpdateAnimation(); // 애니메이션 업데이트

        if (Input.GetKeyDown(KeyCode.Space) && CanBlink())
            StartCoroutine(PerformBlink());
    }

    private bool CanBlink()
    {
        return !isDead && !isBlink && Time.time - lastBlinkTime >= blinkCooldown;
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
        Vector2 velocity = movementDirection * playerStats.Speed;
        _rigidbody.velocity = velocity;
    }

    private void UpdateAnimation()
    {
        if (playerAnimationHandler == null) return;

        bool isMoving = movementDirection.magnitude > 0.1f;
        playerAnimationHandler.SetMovement(isMoving);
    }

    private IEnumerator PerformBlink()
    {
        isBlink = true;
        lastBlinkTime = Time.time;

        // 1. 정확한 마우스 방향 계산
        Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            _camera.transform.position.z));

        Vector2 blinkDirection = (mouseWorldPos - (Vector2)transform.position).normalized;

        // 2. 최소 이동 거리 보장 (Y값이 너무 작을 경우)
        if (Mathf.Abs(blinkDirection.y) < 0.1f)
        {
            blinkDirection.y = blinkDirection.x > 0 ? 0.1f : -0.1f;
            blinkDirection = blinkDirection.normalized;
        }

        // 3. 실제 이동 거리 계산
        float actualBlinkDistance = 3f;
        Vector2 blinkDestination = (Vector2)transform.position + (blinkDirection * actualBlinkDistance);

        // 4. 애니메이션 bool 먼저 활성화
        playerAnimationHandler?.SetBlink(true);

        // 5. Rigidbody가 있을 경우 물리 이동
        if (_rigidbody != null)
        {
            _rigidbody.MovePosition(blinkDestination);
        }
        else
        {
            transform.position = new Vector3(blinkDestination.x, blinkDestination.y, transform.position.z);
        }

        // 6. 이동 결과 강제 동기화
        yield return null; // 한 프레임 대기

        // 7. 애니메이션 유지 시간
        yield return new WaitForSeconds(blinkDuration - Time.deltaTime);

        // 8. 애니메이션 종료
        playerAnimationHandler?.SetBlink(false);

        isBlink = false;

        // 디버그 로그
        Debug.Log($"Blink: From {transform.position} To {blinkDestination} | Direction: {blinkDirection}");
    }

    //private Vector2 CalculateBlinkDestination(Vector2 direction)
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, blinkDistance);
    //    if (hit.collider != null && !hit.collider.isTrigger)
    //    {
    //        return hit.point - direction * 0.5f;
    //    }
    //    return (Vector2)transform.position + direction * blinkDistance;
    //}

    public void TakeDamage()
    {
        if (isDead) return;
        playerAnimationHandler?.PlayDamage();
    }

    public void Die()
    {
        isDead = true;
        _rigidbody.velocity = Vector2.zero;
        playerAnimationHandler?.PlayDeath();
    }


}
