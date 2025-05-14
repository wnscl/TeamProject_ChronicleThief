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
    private SpriteRenderer spriteRenderer;
    public GameObject fadeObject;
    // public GameObject button;


    private void Start()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() // 테스트용
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("FadeIn");
            StartCoroutine(FadeInCoroutine(spriteRenderer, 2f));
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("FadeOut");
            StartCoroutine(FadeOutCoroutine(spriteRenderer, 2f));
        }
    }

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

    IEnumerator FadeInCoroutine(SpriteRenderer sprite, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            sprite.color = new Color(0, 0, 0, Mathf.Lerp(1, 0, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        sprite.color = new Color(0, 0, 0, 0f);
        sprite.gameObject.SetActive(false);
    }

    IEnumerator FadeOutCoroutine(SpriteRenderer sprite, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            sprite.color = new Color(0, 0, 0, Mathf.Lerp(0, 1, time / duration));
            time += Time.deltaTime;
            yield return null;
        }
        sprite.color = new Color(0, 0, 0, 1f);

        yield return new WaitForSeconds(2f);
    }
}
