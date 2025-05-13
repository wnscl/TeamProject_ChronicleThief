using UnityEngine;

public class EffectAnimation : MonoBehaviour
{
    public float duration = 1.0f; 

    private void Start()
    {
        Destroy(gameObject, duration);
    }
}
