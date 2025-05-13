using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionTrigger : MonoBehaviour
{
    [SerializeField] private GameObject EnterObject;
    [SerializeField] private GameObject ExitObject;
    [SerializeField] bool isEnterObject = false;


    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ExitObject.SetActive(true);
            EnterObject.SetActive(false);
            if (!isEnterObject) //일단 한번만
            {
                IsEnterObject();
            }
        }
    }

    public void IsEnterObject()
    {
        isEnterObject = true;
        BattleSystemManager.Instance.OnStageStart();
    }
}

