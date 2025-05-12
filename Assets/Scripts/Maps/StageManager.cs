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
    public Vector3[] stagePos;
    

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

    // void Start()
    // {
    //     if (StageManager.instance == null)
    //     {
    //         Debug.Log("StageManager 인스턴스가가 null 입니다.");
    //     }
    // }

    void FindStages()
    {
        GameObject parentObject = GameObject.Find("Stage"); // 부모 오브젝트 찾기기
        if (parentObject != null)
        {
            stages = parentObject.GetComponentsInChildren<Transform>(true) // 자식 Transform 검색
                .Select(t => t.gameObject)  //Transform을 GameObject로 변환
                .Where(go => go.CompareTag("Stage")) // "Stage" 태그가 설정된 오브젝트만 필터링
                .ToArray(); // 배열로 변환환
            Debug.Log("Stage 총 개수: " + stages.Length + " 등록.");

            // stagePos 배열 크기 맞추기.
            stagePos = new Vector3[stages.Length];
            for (int i = 0; i < stages.Length; i++)
            {
                stagePos[i] = stages[i].transform.position; // 각 스테이지위 위치를 저장.
            }
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

            if (player != null)
            {
                player.transform.position = stagePos[newIndex];
            }
            else
            {
                Debug.Log("player 객체가 존재하지 않습니다!");
            }          
        }
        else
        {
            Debug.LogWarning("잘못된 스테이지 인덱스: " + newIndex);
        }
    }
}
