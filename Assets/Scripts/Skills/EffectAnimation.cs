using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    public float duration = 1.0f; // �ִϸ��̼� ��� �ð�

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
