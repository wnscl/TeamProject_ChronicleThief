using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MapInteraction
{
    private float timer = 0f;
    private float interval = 1f;

    void OnTriggerStay2D(Collider2D collision)
    {
        timer += Time.deltaTime;
        // int damageZone = 1;

        if (timer >= interval && collision.CompareTag("Player"))
        {
            Debug.Log("데미지 존 Enter!!");
            // HealthSystem.Instance.TakeDamage(damageZone);
            timer = 0f;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("데미지 존 Exit");
        }
    }
}
