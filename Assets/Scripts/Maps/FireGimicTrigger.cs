using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class FireGimicTrigger : MonoBehaviour
{
    public GameObject[] firePoints;
    private bool isActivated = false;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isActivated)
        {
            isActivated = true;
            OneStepGimic();
            StartCoroutine(ActivateFirePointsOneStep(1.2f));
        }
    }


    void OneStepGimic()
    {
        GameObject parentObject = GameObject.Find("OneStepGimic");
        if (parentObject != null)
        {
            Transform[] fireTransforms = parentObject.GetComponentsInChildren<Transform>(true);
            
            if (fireTransforms != null && fireTransforms.Length > 0)
            {
                firePoints = fireTransforms
                .Select(t => t.gameObject)
                .Where(t => t.CompareTag("FirePoint"))
                .OrderBy(go => {
                    string numberPart = go.name.Replace("FirePoint", "");
                    return int. TryParse(numberPart, out int result) ? result : int.MaxValue;
                })
                .ToArray();
            }
            
            Debug.Log("fireObjects: " + firePoints.Length + " 등록");
        }
        else
        {
            Debug.Log("FirePoint 오브젝트를 찾을 수 없음.");
        }
    }

    IEnumerator ActivateFirePointsOneStep(float delay)
    {
        for (int i = 0; i < firePoints.Length; i +=2)
        {
            if (firePoints[i] != null && !firePoints[i].activeSelf)
            {
                yield return new WaitForSeconds(delay);
                firePoints[i].SetActive(true);
                Debug.Log($"활성: {firePoints[i].name}");
            }

            if (i + 1 < firePoints.Length && firePoints[i + 1] != null && !firePoints[i + 1].activeSelf)
            {
                firePoints[i + 1].SetActive(true);
                Debug.Log($"활성2: {firePoints[i + 1].name}");
            }
        }
        SecondStepGimic();
    }

    void SecondStepGimic()
    {
        GameObject parentObject = GameObject.Find("SecondStepGimic");
        if (parentObject != null)
        {
            Transform[] fireTransforms = parentObject.GetComponentsInChildren<Transform>(true);
            
            if (fireTransforms != null && fireTransforms.Length > 0)
            {
                firePoints = fireTransforms
                .Select(t => t.gameObject)
                .Where(t => t.CompareTag("FirePoint"))
                .OrderBy(go => {
                    string numberPart = go.name.Replace("FirePoint", "");
                    return int. TryParse(numberPart, out int result) ? result : int.MaxValue;
                })
                .ToArray();
            }
            
            Debug.Log("fireObjects: " + firePoints.Length + " 등록");
        }
        else
        {
            Debug.Log("FirePoint 오브젝트를 찾을 수 없음.");
        }

        StartCoroutine(ActivateFirePointCircle(1f, 0.2f));
    }

    IEnumerator ActivateFirePointCircle(float initialDelay, float minDelay)
    {
        float currentDelay = initialDelay;
        for (int i = 0; i < firePoints.Length; i++)
        {
            GameObject firePoint = firePoints[i];
            if (firePoint != null && !firePoint.activeSelf)
            {
                yield return new WaitForSeconds(currentDelay);
                firePoint.SetActive(true);
                Debug.Log($"활성3: {firePoint.name}, 다음 딜레이: {currentDelay}");

                currentDelay = Mathf.Lerp(currentDelay, minDelay, 0.3f);
            }
        }
        yield return new WaitForSeconds(0.5f);
        

        SpriteRenderer blackBox = GameObject.Find("Fade_In_BlackBox")?.GetComponent<SpriteRenderer>();

        if (blackBox != null)
        {
            TriggerFadeIn(blackBox);
        }
        else
        {
            Debug.LogError("검은 화면을 찾을 수 없음.");
        }
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
            Debug.LogError("FadeInOut 찾을 수 없음. 오브젝트 추가됬는지 확인.");
        }
    }
    
    
}
