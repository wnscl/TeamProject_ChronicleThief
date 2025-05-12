using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    public int nextStageIndex;
    public Vector3 newPosition;
    

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (StageManager.instance != null
            && nextStageIndex >= 0
            && nextStageIndex < StageManager.instance.stages.Length)
            {
                StageManager.instance.ChangeStage(nextStageIndex, other.gameObject);
                Debug.Log("스테이지 " + nextStageIndex + "로 이동!");
            }
            else
            {
                Debug.LogError("잘못된 nextStageIndex 값: " + nextStageIndex);
            }
        }
    }  
}
