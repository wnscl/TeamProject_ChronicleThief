using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeInOut : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public GameObject fadeObject;

     void Start()
    {
        spriteRenderer = fadeObject.transform.GetComponentInChildren<SpriteRenderer>();
        // if (spriteRenderer == null)
        // {
        //     Debug.Log($"{fadeObject.name}에서 SpriteRenderer를 찾을 수 없음.");
        // }
        // else
        // {
        //     Debug.Log($"{fadeObject.name}에서 SpriteRenderer 가져오기 성공.");
        // }
    }

    public IEnumerator FadeInCoroutine(SpriteRenderer sprite, float duration)
    {
        float time = 0;

        while (time < duration)
        {
            float alpha = Mathf.Lerp(1, 0, time / duration);
            sprite.color = new Color(0, 0, 0, alpha);

            Debug.Log($"페이드 인 : alpha = {alpha}");

            time += Time.deltaTime;
            yield return null;
        }
        sprite.color = new Color(0, 0, 0, 0f);
        Debug.Log("페이드 인 완료.");
    }
}
