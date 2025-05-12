using System.Collections;
using TMPro;
using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    public static SpawnTimer Instance { get; private set; }
    public TMP_Text timerText;      // Inspector에 연결할 TextMeshProUGUI

    private Coroutine countdownRoutine;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        gameObject.SetActive(false);
    }

    // 카운트다운 시작 (초 단위)
    public void StartCountdown(int seconds)
    {
        gameObject.SetActive(true);

        // 기존 코루틴 있으면 정지
        if (countdownRoutine != null)
            StopCoroutine(countdownRoutine);
        countdownRoutine = StartCoroutine(Countdown(seconds));
    }

    private IEnumerator Countdown(int time)
    {
        gameObject.SetActive(true);
        int remaining = time;
        while (remaining >= 0)
        {
            // MM:SS 포맷
            int m = remaining / 60;
            int s = remaining % 60;
            timerText.text = $"{m:00}:{s:00}";
            yield return new WaitForSeconds(1f);
            remaining--;
        }
        gameObject.SetActive(false);
    }
}
