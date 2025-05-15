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
            if (ExitObject != null)
            {
                ExitObject.SetActive(true);
            }
            else
            {
                Debug.LogError("ExitObject가 설정되지 않음!");
            }

            if (EnterObject != null)
            {
                EnterObject.SetActive(false);
            }
            else
            {
                Debug.LogError("EnterObject가 설정되지 않음!");
            }
            
            if (!isEnterObject) //�ϴ� �ѹ���
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

