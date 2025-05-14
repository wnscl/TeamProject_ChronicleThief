using TMPro;
using UnityEngine;

public class StageTimer : MonoBehaviour
{
    [SerializeField] private TMP_Text timeText; // Inspector���� ����

    // ���� �ð�(��)�� �޾Ƽ� MM:SS �������� �����ݴϴ�.
    public void SetTime(int totalSeconds)
    {
        int m = totalSeconds / 60;
        int s = totalSeconds % 60;
        timeText.text = $"{m:00}:{s:00}";
    }
}