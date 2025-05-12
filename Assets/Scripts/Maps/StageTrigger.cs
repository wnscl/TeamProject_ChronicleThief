using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StageTrigger : MonoBehaviour
{
    public int nextStageIndex;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (StageManager.instance != null)
            {
                StageManager.instance.ChangeStage(nextStageIndex);
                Debug.Log("스테이지 " + nextStageIndex + "로 이동!");
            }
        }
    }
}
