using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using UnityEngine;

public class MainLobbyInter : MapInteraction
{
    protected override void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
            SomeFunction();
        }
    }


    protected void Interact()
    {
        Debug.Log("MainLobby: 입력 상호작용 작동.");
    }

    void SomeFunction()
    {
        int nextStage = StageManager.instance.nextStageIndex;
        StageManager.instance.ChangeStage(nextStage);
    }
}
