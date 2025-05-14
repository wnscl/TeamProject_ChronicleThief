using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaSkill : MonoBehaviour
{
    [Header("Skill Setting")]
    public float radius = 3f;
    public float delay = 3f;
    public int damage = 10;
    public LayerMask targetLayer;

    public GameObject warningEffect;
    public GameObject skillEffect;

    private Vector3 targetPosition;

    //public void Initialize(Vector3 center)
    //{
    //    targetPosition = center;
    //    StartCoroutine(TriggerArea());
    //}
    private void Start()
    {
        StartCoroutine(TriggerArea());
    }

    private IEnumerator TriggerArea()
    {
        if (warningEffect != null)
        {
            GameObject warning = Instantiate(warningEffect, transform.position, Quaternion.identity);
            warning.transform.localScale = Vector3.one * radius; //* 2f;
            Destroy(warning, delay);
        }

        yield return new WaitForSeconds(delay);

        if (skillEffect != null)
        {
            GameObject effect = Instantiate(skillEffect, transform.position, Quaternion.identity);
            Destroy(effect, 5f);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, targetLayer);

        foreach (var hit in hits)
        {
            if (hit.gameObject.layer == LayerMask.NameToLayer("Player"))
            { 
                BattleSystemManager.Instance.AttackPlayer(damage);
            }
        }

        Destroy(gameObject);
    }
}
