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
    private float blinkCooldown = 2f;
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
        playerAnimationHandler?.SetBlink(true);

        // 1. ��ǥ ��ġ ���
        Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 blinkDirection = (mouseWorldPos - (Vector2)transform.position).normalized;
        float blinkDistance = 3f;

        // 2. �浹 �˻� (Raycast + OverlapCircle)
        RaycastHit2D hit = Physics2D.Raycast( // ������ �߻��� �浹ü�� �����ϴ� �ٽ� ���� �Լ�(�� �˰� �־�� �ϴ� ���� �ϳ��ε�)
            transform.position, // origin ���� ���� ��ġ (�÷��̾� �� ��ġ)
            blinkDirection, // direction ���� ���� (���콺 ����)
            blinkDistance, // distance �˻� �Ÿ� ������ 3f�� ����
            LayerMask.GetMask("Obstacle")); // �˻��� Layer

        if (hit.collider != null)
        {
            blinkDistance = hit.distance * 0.95f; // 5% ���� ���� Ȯ��
        }

        Vector2 targetPos = (Vector2)transform.position + (blinkDirection * blinkDistance);

        // 3. ���� �̵� �� ���� ����ȭ
        Physics2D.SyncTransforms(); // ���� ��ġ�� ���������� ���� �ݿ�
        // Unity�� �⺻������ ������ ���� �� Transform ��������� ���������� �ݿ��ϱ⶧����
        // �������� ����ȭ�ϸ� �����̵� �� ��� �浹 �˻簡 ���������ٰ� �Ѵ�.
        // Raycast : �̵� �� ��ֹ� ���� ���� Ȯ���ϰ�
        // OverlapCircle : �̵� �� �̼��� ���� ���� ���� ����.

        // 4. Rigidbody�� �̵� (���� ���� ����)
        if (_rigidbody != null)
        {
            _rigidbody.MovePosition(targetPos); // transform.position���� ���� �������� ȣȯ���� ���ٰ� �Ѵ�.
                                                // ������ SyncTransforms()�� ������ ���� �߻� ������.
        }
        else
        {
            transform.position = targetPos;
        }

        // 5. �̵� �� �ٽ� ���� ����ȭ
        Physics2D.SyncTransforms();

        yield return new WaitForSeconds(blinkDuration);

        // 6. �̵� �� �߰� �浹 �˻� (���� ����)
        Collider2D[] overlaps = Physics2D.OverlapCircleAll(
            transform.position,
            0.4f,
            LayerMask.GetMask("Obstacle"));

        if (overlaps.Length > 0)
        {
            // �浹 �߻� �� ���� ����� ������ ��ġ�� ����
            Vector2 escapeDir = (transform.position - (Vector3)overlaps[0].ClosestPoint(transform.position)).normalized;
            transform.position += (Vector3)(escapeDir * 0.1f);
            Physics2D.SyncTransforms();
        }

        playerAnimationHandler?.SetBlink(false);
        isBlink = false;
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
