using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public GameObject[] stages;
    private int currentStageIndex = 0;
    public int nextStageIndex;
    public GameObject player;

    // public GameObject fadeGimicObject;



    private void Awake()
    {
        if (instance == null)
        {
            instance = this;

            FindStages();
        }
        else
        {
            Destroy(gameObject);
        }
    }


    void FindStages()
    {
        GameObject parentObject = GameObject.Find("Stage"); // 부모 오브젝트 찾기기
        if (parentObject != null)
        {
            stages = parentObject.GetComponentsInChildren<Transform>(true) // 자식's Transform 가져온다. ture를 넣으면 비활성화된 객체까지 전부 가져온다.
                .Select(t => t.gameObject)  //Transform을 GameObject로 변환
                .Where(go => go.CompareTag("Stage")) // "Stage" 태그가 설정된 오브젝트만 필터링
                .ToArray(); // 배열로 변환환
            Debug.Log("Stage 총 개수: " + stages.Length + " 등록.");
        }
        else
        {
            Debug.Log("부모 오브젝트를 찾을 수 없습니다.");
        }
    }

    public void ChangeStage(int newIndex, GameObject player)
    {
        if (newIndex >= 0 && newIndex < stages.Length)
        {
            if (stages[currentStageIndex] != null) {
                stages[currentStageIndex].SetActive(false);
            }
            stages[newIndex].SetActive(true);
            currentStageIndex = newIndex;

            GameObject spawnPoint = stages[newIndex].transform.Find("PlayerSpawn")?.gameObject;

            if (spawnPoint != null && player != null)
            {
                player.transform.position = spawnPoint.transform.position;
                Debug.Log($"플레이어가 {newIndex}번 스테이지의 Point로 이동");
            }
            else
            {
                Debug.LogError("SpawnPoint를 찾을 수 없거나 Player가 존재하지 않습니다.");
            }
        }
        else
        {
            Debug.LogWarning("잘못된 스테이지 인덱스: " + newIndex);
        }
    }

    public void FloorChange(GameObject player)
    {
        int nextStageIndex = currentStageIndex + 1;

        if (nextStageIndex >= 0 && nextStageIndex < stages.Length)
        {
            ChangeStage(nextStageIndex, player);
        }
        else
        {
            Debug.LogWarning($"잘못된 스테이지 인덱스: {nextStageIndex}");
        }
        // StartFadeOut(fadeGimicObject.GetComponent<SpriteRenderer>());
    }

    // void StartFadeOut(SpriteRenderer sprite)
    // {
    //     GimmickTrigger gimic = FindObjectOfType<GimmickTrigger>();

    //     if (gimic != null)
    //     {
    //         StartCoroutine(gimic.FadeOutCoroutine(sprite, 2f));
    //     }
    //     else
    //     {
    //         Debug.LogError("GimmickTrigger를 찾을 수 없음.");
    //     }
    // }
}
