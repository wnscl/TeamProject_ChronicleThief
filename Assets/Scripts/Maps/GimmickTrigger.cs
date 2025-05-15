using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;


public class GimmickTrigger : MonoBehaviour
{
    private Animator animator;
    private GameObject[] gimicFloorObj;


    // private bool isTriggered = false;
    // public int nextFloorIndex = 0;

    // public GameObject button;

    // public SpriteRenderer spriteRenderer; // << 참조를 하지 못하는 상황.
    // public GameObject fadeObject;



    private void Start()
    {
        animator = GetComponent<Animator>();
        // spriteRenderer = GetComponent<SpriteRenderer>();
        // spriteRenderer = fadeObject.transform.GetComponentInChildren<SpriteRenderer>(); // 객체를 자식 오브젝트에서 가져와야 한다.
    }


    // 배틀매니저에서 실행할 메서드.
    public void StageOneFloorGimic()
    {
        animator.SetTrigger("FloorShake");
    }

    void FindGimicObjects() // 애니메이터 연동으로 메서드 실행(FloorShake)
    {
        GameObject parentObj = GameObject.Find("Stage1_Floor");
        if (parentObj != null)
        {
            gimicFloorObj = parentObj.GetComponentsInChildren<Transform>(true)
                .Select(t => t.gameObject)
                .Where(go => go.CompareTag("Stage1_Floor"))
                .ToArray();
            Debug.Log("Stage1_Floor 개수:" + gimicFloorObj.Length + "등록");
        }
        else
        {
            Debug.Log("부모 오브젝트 찾을 수 없음.");
        }

        if (gimicFloorObj != null)
        {
            StartCoroutine(ActivateChianObject());
        }
        else
        {
            Debug.Log("gimicObject: null");
        }
    }

    IEnumerator ActivateChianObject()
    {
        for (int i = 0; i < gimicFloorObj.Length; i++)
        {
            if (i > 0)
            {
                gimicFloorObj[i - 1].SetActive(false);
                Debug.Log($"{gimicFloorObj[i - 1].name} 비활성");
            }
            gimicFloorObj[i].SetActive(true);
            Debug.Log($"{gimicFloorObj[i].name} 활성");

            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);

        SpriteRenderer blackBox = GameObject.Find("Fade_In_BlackBox")?.GetComponent<SpriteRenderer>();

        if (blackBox != null)
        {
            TriggerFadeIn(blackBox);
        }
        else
        {
            Debug.LogError("blackBox 찾을 수 없음.");
        }

        yield return new WaitForSeconds(2f);

        StageManager.instance.FloorChange(StageManager.instance.player);
        TriggerFadeOut(blackBox);

        yield break;
    }

    void TriggerFadeIn(SpriteRenderer spriteRenderer)
    {
        FadeInOut fadeScript = FindObjectOfType<FadeInOut>();

        if (fadeScript != null)
        {
            Debug.Log("FadeInCoroutine 실행.");
            fadeScript.StartCoroutine(fadeScript.FadeInCoroutine(spriteRenderer, 2f));
        }
        else
        {
            Debug.LogError("FadeInOut 찾을 수 없음. 오브젝트 추가됐는지 확인 필요요.");
        }
    }

    void TriggerFadeOut(SpriteRenderer spriteRenderer)
    {
        FadeInOut fadeScript = FindObjectOfType<FadeInOut>();

        if (fadeScript != null)
        {
            Debug.Log("FadeOutCoroutine 실행.");
            fadeScript.StartCoroutine(fadeScript.FadeOutCoroutine(spriteRenderer, 2f));
        }
        else
        {
            Debug.LogError("FadeInOut 찾을 수 없음. 오브젝트 추가됐는지 확인 필요.");
        }
    }
}
