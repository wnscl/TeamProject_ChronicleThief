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

    // Blink ���� ����
    private bool isBlink = false;
    //private float blinkDistance = 5f;
    private float blinkCooldown = 5f;
    private float lastBlinkTime = -999f;
    private float blinkDuration = 0.4f;

    public Vector2 MovementDirection => movementDirection;
    public Vector2 LookDirection => lookDirection;
    public bool IsDead => isDead;
    public float BlinkCooldownRemaining => Mathf.Max(0, blinkCooldown - (Time.time - lastBlinkTime)); // ���� ��Ÿ�� ���
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
        if (isDead) return; // ��� �� �Է� ����

        HandleInput(); // �Է� ó��
        RotateCharacter(); // ���� ȸ��
        UpdateAnimation(); // �ִϸ��̼� ������Ʈ

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
            HandleMovement(); // ���� �̵� ó��
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

        // 1. ��Ȯ�� ���콺 ���� ���
        Vector2 mouseWorldPos = _camera.ScreenToWorldPoint(new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            _camera.transform.position.z));

        Vector2 blinkDirection = (mouseWorldPos - (Vector2)transform.position).normalized;

        // 2. �ּ� �̵� �Ÿ� ���� (Y���� �ʹ� ���� ���)
        if (Mathf.Abs(blinkDirection.y) < 0.1f)
        {
            blinkDirection.y = blinkDirection.x > 0 ? 0.1f : -0.1f;
            blinkDirection = blinkDirection.normalized;
        }

        // 3. ���� �̵� �Ÿ� ���
        float actualBlinkDistance = 3f;
        Vector2 blinkDestination = (Vector2)transform.position + (blinkDirection * actualBlinkDistance);

        // 4. �ִϸ��̼� bool ���� Ȱ��ȭ
        playerAnimationHandler?.SetBlink(true);

        // 5. Rigidbody�� ���� ��� ���� �̵�
        if (_rigidbody != null)
        {
            _rigidbody.MovePosition(blinkDestination);
        }
        else
        {
            transform.position = new Vector3(blinkDestination.x, blinkDestination.y, transform.position.z);
        }

        // 6. �̵� ��� ���� ����ȭ
        yield return null; // �� ������ ���

        // 7. �ִϸ��̼� ���� �ð�
        yield return new WaitForSeconds(blinkDuration - Time.deltaTime);

        // 8. �ִϸ��̼� ����
        playerAnimationHandler?.SetBlink(false);

        isBlink = false;

        // ����� �α�
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
