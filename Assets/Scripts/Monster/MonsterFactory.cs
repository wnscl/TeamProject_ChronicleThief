using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MonsterFactory : MonoBehaviour
{
    [SerializeField] private GameObject skullRunner;
    [SerializeField] Vector2 createPos;
    [SerializeField] Vector2 minPos;
    [SerializeField] Vector2 maxPos;

    public void Awake()
    {
        //skullRunner = FindObjectOfType<SkullRunner>().gameObject;
        //skullRunner = FindAnyObjectByType<SkullRunner>().gameObject;
        //skullRunner = 

    }

    public void Start()
    {
        OnMakeMonster();
        //ClearAllMonsterByName("SkullRunner");
        //ClearAllMonsterByName("SkullRunner(Clone)");
    }

    public void OnMakeMonster()
    {
        for (int i = 0; i < 10; i++)
        {
            minPos = new Vector2(-6, -6);
            maxPos = new Vector2(6, 6);
            float randomPosX = UnityEngine.Random.Range(minPos.x, maxPos.x);
            float randomPosY = UnityEngine.Random.Range(minPos.y, maxPos.y);

            createPos = new Vector2(randomPosX, randomPosY);
            Instantiate(skullRunner, createPos, Quaternion.identity, this.transform);

            //yield return new WaitForSeconds(0.5f);
        }
        //StartCoroutine(MakeMonster());
    }

/*    public IEnumerator MakeMonster()
    {
        for (int i = 0; i < 10; i++)
        {
            minPos = new Vector2(-6, -6);
            maxPos = new Vector2(6, 6);
            float randomPosX = UnityEngine.Random.Range(minPos.x, maxPos.x);
            float randomPosY = UnityEngine.Random.Range(minPos.y, maxPos.y);

            createPos = new Vector2(randomPosX, randomPosY);
            Instantiate(skullRunner, createPos, Quaternion.identity, this.transform);

            yield return new WaitForSeconds(0.5f);
        }

        *//*Instantiate(original);                           // ���� �⺻
        Instantiate(original, position, rotation);       // ��ġ�� ȸ�� ����
        Instantiate(original, parent);                   // �θ� ���� (Transform ����)
        Instantiate(original, position, rotation, parent); // ��ġ + ȸ�� + �θ� ���� ����*//*
    }
*/
    public void DestroyTestMonster()
    {
/*        while (transform.FindChild("SkullRunner") != null)
            //���÷��ʶ�� �̸��� ������Ʋ ã�� ���� �� ����
        {
            Destroy(transform.Find("SkullRunner").gameObject, 1f);
        }*/

    }

    void ClearAllMonsterByName(string name)
    {
        var monster = new List<Transform>();

        foreach (Transform mob in transform)
        {
            if (mob.name == name)
            {
                monster.Add(mob);
                if (mob != null)
                {
                    Destroy(mob.gameObject, 10f);
                }
                else
                {
                    Debug.Log("���� ã������");
                }

            }
                
        }

/*        foreach (Transform mob in monster)
        {
            Destroy(mob.gameObject, 1f);
        }*/
    }
}
