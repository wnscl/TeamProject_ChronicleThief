using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public static StageManager instance;
    public GameObject[] stages;
    private int currentStageIndex = 0;
    public int nextStageIndex;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
    }

    public void ChangeStage(int newIndex)
    {
        if (newIndex >= 0 && newIndex < stages.Length)
        {
            stages[currentStageIndex].SetActive(false);
            stages[newIndex].SetActive(true);
            currentStageIndex = newIndex;
        }
    }
}
