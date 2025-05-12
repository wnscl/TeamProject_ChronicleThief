using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    Rigidbody2D r;

    private void Awake()
    {
        r = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        r.velocity = Vector3.zero;  
    }
}
