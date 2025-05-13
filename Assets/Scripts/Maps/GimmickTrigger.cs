using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GimmickTrigger : MonoBehaviour
{
    private Animator animator;
    public GameObject[] gimicFloorObj;
    // private bool isTriggered = false;
    // public int nextFloorIndex = 0;


    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    // void Update() // 테스트용
    // {
    //     if (Input.GetKeyDown(KeyCode.F))
    //     {
    //         StageOneFloorGimic();
    //     }
    // }

    //테스트용
    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     if (collision.CompareTag("Player") && !isTriggered)
    //     {
    //         isTriggered = true;
    //         animator.SetTrigger("FloorShake");
    //     }
    // }

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

            yield return new WaitForSeconds(1f);
        }
        // StartCoroutine(FadeOutSprite(mySpriteRenderer, 2f));

        yield break; // 코루틴 중첩 방지용.
    }

    // IEnumerator FadeOutSprite(SpriteRenderer sprite, float duration)
    // {
    //     float time = 0;
    //     Color startColor = sprite.color;

    //     while (time < duration)
    //     {
    //         sprite.color = new Color(startColor.r, startColor.g, startColor.b, Mathf.Lerp(0f, 1f, time / duration));
    //         time += Time.deltaTime;
    //         yield return null;
    //     }
    //     sprite.color = new Color(startColor.r, startColor.g, startColor.b, 1f);

    //     yield break;
    // }

    // Trigger 메서드를 통해 이동할 인덱스를 입력하면 다음 스테이지로 이동할 수 있는 로직.
    // 기믹 연출로는 별로 좋은 방법은 아닌 듯...
    // public void ActiveGimic(int newIndex)
    // {
    //     if (newIndex >= 0 && newIndex < gimicFloorObj.Length)
    //     {
    //         if (gimicFloorObj[currentFloorIndex] != null){
    //             gimicFloorObj[currentFloorIndex].SetActive(false);
    //         }
    //         gimicFloorObj[newIndex].SetActive(true);
    //         currentFloorIndex = newIndex;
    //     }
    //     else
    //     {
    //         Debug.LogWarning("잘못된 Floor 인덱스:" + newIndex);
    //     }
    // }
}
