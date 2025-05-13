using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IntroEffect : MonoBehaviour
{
    [Header("텍스트")]
    public TextMeshProUGUI titleText;
    public float softnessMin = 0.1f, softnessMax = 0.6f;
    public float dilateMin = -0.5f, dilateMax = 0.5f;
    public float animationSpeed = 1f;
    public float durationBeforeButton = 3f;

    [Header("버튼")]
    public CanvasGroup startButtonGroup;
    public CanvasGroup exitButtonGroup;
    public float fadeDuration = 1f;

    [Header("배경 이미지 깜빡임")]
    public Image blinkingImage; // UI 이미지 연결용
    public float blinkSpeed = 1.5f; // 깜빡이는 속도
    public float blinkAlphaMin = 0f;
    public float blinkAlphaMax = 1f;

    private Material mat;
    private float time;

    private void Start()
    {
        mat = titleText.fontMaterial;

        // 버튼 비활성화 초기화
        if (startButtonGroup != null)
        {
            startButtonGroup.alpha = 0;
            startButtonGroup.interactable = false;
            startButtonGroup.blocksRaycasts = false;
        }

        if (exitButtonGroup != null)
        {
            exitButtonGroup.alpha = 0;
            exitButtonGroup.interactable = false;
            exitButtonGroup.blocksRaycasts = false;
        }

        // 버튼 페이드 시작 예약
        StartCoroutine(FadeInButtonAfterDelay(durationBeforeButton));
    }

    private void Update()
    {
        time += Time.deltaTime * animationSpeed;

        float t1 = (Mathf.Sin(time) + 1f) / 2f; // 부드러운 시간이동을 위해  // 0 ~ 1
        float softness = Mathf.Lerp(softnessMin, softnessMax, t1);
        float dilate = Mathf.Lerp(dilateMin, dilateMax, t1);

        mat.SetFloat("_FaceSoftness", softness);
        mat.SetFloat("_FaceDilate", dilate);

        if (blinkingImage != null)
        {
            float blinkT = (Mathf.Sin(time * blinkSpeed) + 1f) / 2f; // 0~1 사이
            float alpha = Mathf.Lerp(blinkAlphaMin, blinkAlphaMax, blinkT);

            Color c = blinkingImage.color;
            c.a = alpha;
            blinkingImage.color = c;
        }
    }

    private IEnumerator FadeInButtonAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsed / fadeDuration);
            startButtonGroup.alpha = alpha;
            exitButtonGroup.alpha = alpha;
            yield return null;
        }

        // 버튼 클릭 가능하게 활성화
        startButtonGroup.interactable = true;
        startButtonGroup.blocksRaycasts = true;
        exitButtonGroup.interactable = true;
        exitButtonGroup.blocksRaycasts = true;
    }
}
