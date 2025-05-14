using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeartAttack : MonoBehaviour
{
    public GameObject line;
    public Rigidbody2D rigid;
    public BoxCollider2D hitBox;
    public GameObject aron;
    public GameObject weapon;
    Aron boss;
    private PlayerController player;
    bool activeNow = false;
    bool isActive = false;
    float timer = 0f;
    float angle;

    public Vector2 playerPos = Vector2.zero;
    public Vector2 direction = Vector2.zero;
    public Vector2 nextPos = Vector2.zero;
    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        playerPos = player.transform.position;  
        direction = (player.transform.position - transform.position).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (timer > 0.5f && !activeNow)
        {
            activeNow = true;
            ActiveSkill();
        }
        

        if (timer > 1f)
        {
            Destroy(this.gameObject);
        }

    }
    private void FixedUpdate()
    {
        nextPos = direction * 10f * Time.fixedDeltaTime;

        if (isActive)
        {
            rigid.MovePosition(rigid.position + nextPos);
            rigid.velocity = Vector2.zero;
        }
    }

    private void ActiveSkill()
    {
        if (activeNow && !isActive)
        {
            line.SetActive(false);
            hitBox.enabled = true;
            aron.SetActive(true);
            weapon.SetActive(true);
            isActive = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            //BattleSystemManager.Instance.AttackPlayer();
        }
    }

}
