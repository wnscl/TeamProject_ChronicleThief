using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TheStone : MonoBehaviour
{
    public static TheStone instance;

    public int maxHp = 100;
    public int currentHp;

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        currentHp = maxHp;
    }

    public void TakeDamage(int dmg)
    {
        currentHp -= dmg;

        if (currentHp <= 0)
        {
            DestroyStone();
        }
    }

    private void DestroyStone()
    {
        Destroy(gameObject);
        DeathPanelController.Instance.Show();
    }
}
