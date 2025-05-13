using UnityEngine;

public class FollowPlayerEffect : MonoBehaviour
{
    public Transform target;     
    public float duration = 1f;  
    public Vector3 offset = Vector3.zero;

    void Update()
    {
        if (target != null)
            transform.position = target.position + offset;
    }

    void Start()
    {
        Destroy(gameObject, duration);
    }
}
