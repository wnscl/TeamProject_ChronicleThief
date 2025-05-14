using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HeartAttackRe : MonoBehaviour
{
    public GameObject skill;
    PlayerController player;
    Aron aron;

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        aron = FindObjectOfType<Aron>();
    }

    private void Start()
    {
        StartCoroutine(UseAronSkill());
    }

    private IEnumerator UseAronSkill()
    {
        float timer = 0;
        while (timer < 5f)
        {
            timer += 0.5f;
            float angle = Random.Range(0f, 360f);
            float rad = angle * Mathf.Deg2Rad;
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
            Vector2 spawnPos = (Vector2)player.transform.position + direction * 4f;
            Instantiate(skill, spawnPos, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }
}
//arrowPos = (Vector2)transform.position + (arrowDirection * 1.5f);