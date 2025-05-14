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
            Vector2 spawnPos = new Vector2
                (player.transform.position.x + 2.5f,
                player.transform.position.y);
            Instantiate(skill, spawnPos, Quaternion.identity,this.transform);
            yield return new WaitForSeconds(0.5f);
        }
        yield break;
    }
}
//arrowPos = (Vector2)transform.position + (arrowDirection * 1.5f);