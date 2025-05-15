using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public int damage = 80;
    public float destroyDelay = 2f;

    private void Start()
    {
        Destroy(gameObject, destroyDelay);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != null && (collision.gameObject.layer == LayerMask.NameToLayer("Player")))
        {
            BattleSystemManager.Instance.AttackPlayer(damage);
            Destroy(this.gameObject);
        }

    }
}
