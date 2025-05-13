using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    public float duration = 1.0f; // 애니메이션 재생 시간

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
