using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MapInteraction : MonoBehaviour
{
    protected bool inZone = false;
    

    protected virtual void Update()
    {
        if (inZone && Input.GetKeyDown(KeyCode.E))
        {
            KeyInteract();
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = true;
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inZone = false;
        }
    }

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inZone = true;
        }
        Debug.Log("부모 클래스에서 Stay2D 점검 중 inZone :" + inZone);
    }

    protected virtual void KeyInteract()
    {
        Debug.Log("MapInteraction: 부모 클래스 작동.");
    }


    void SomeFunction()
    {
        int nextStage = StageManager.instance.nextStageIndex;
        StageManager.instance.ChangeStage(nextStage);
    }
}
