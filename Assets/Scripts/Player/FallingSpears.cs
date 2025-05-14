using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpears : MonoBehaviour
{
    [Header("Skill Setting")]
    public GameObject spearPrefab;
    public GameObject warningEffectPrefab;
    public float skillDuration = 4f;
    public float spawnInterval = 0.2f;
    public float spawnRadius = 1f;
    public float height = 10f;
    public int spearsPerSpawn = 4;
    public float warningDuration = 1f;

    private Vector3 targetPosition;
    
    public void Initialize(Vector3 center)
    {
        targetPosition = center;
        StartCoroutine(ExecuteSkill());
    }


    private IEnumerator ExecuteSkill()
    {
        GameObject warning = Instantiate(warningEffectPrefab, targetPosition, Quaternion.identity); // 경고 이펙트 생성
        warning.transform.localScale = Vector3.one * spawnRadius * 2f; // 원크기 조절

        yield return new WaitForSeconds(warningDuration);

        Destroy(warning);

        float timer = 0f;
        while (timer < skillDuration)
        {
            for (int i = 0; i < spearsPerSpawn; i++)
            {
                Vector2 offset = Random.insideUnitCircle * spawnRadius;
                Vector3 spawnPos = targetPosition + new Vector3(offset.x, height, offset.y);

                float angle = -225;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);

                Instantiate(spearPrefab, spawnPos, rotation);
            }

            yield return new WaitForSeconds(spawnInterval);
            timer += spawnInterval;
        }

        Destroy(gameObject);
    }

    // 보스가 사용할시 보스에게 추가해줄 코드
    //public GameObject fallingSpearSkillPrefab;
    //void CastFallingSpearSkill()
    //{
    //    Vector3 playerPos = player.transform.position;
    //    GameObject skill = Instantiate(fallingSpearSkillPrefab);
    //    skill.GetComponent<FallingSpearSkill>().Initialize(playerPos);
    //} // 플레이어를 타겟팅

}
