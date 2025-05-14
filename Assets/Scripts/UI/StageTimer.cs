using TMPro;
using UnityEngine;

public class StageTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText; // Inspector에서 연결

    // 남은 시간(초)을 받아서 MM:SS 형식으로 보여줍니다.
    public void SetTime(int totalSeconds)
    {
        int m = totalSeconds / 60;
        int s = totalSeconds % 60;
        timeText.text = $"{m:00}:{s:00}";
    }
}