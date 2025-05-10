using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;

public class MonsterFactory : MonoBehaviour
{
    private GameObject skullRunner;

    [SerializeField] Vector2 createPos;

    public void Awake()
    {
        skullRunner = FindObjectOfType<SkullRunner>().gameObject;
        float createPosX = 10f;
        float createPosY = 10f;
        createPos = new Vector2(createPosX, createPosY);
    }

    public void Start()
    {
        MakeMonster();
        //ClearAllMonsterByName("SkullRunner");
        ClearAllMonsterByName("SkullRunner(Clone)");
    }

    public void MakeMonster()
    {
        for (int i = 0; i < 10; i++)
        {
            Instantiate(skullRunner, createPos, Quaternion.identity, this.transform);
        }

        /*Instantiate(original);                           // ���� �⺻
        Instantiate(original, position, rotation);       // ��ġ�� ȸ�� ����
        Instantiate(original, parent);                   // �θ� ���� (Transform ����)
        Instantiate(original, position, rotation, parent); // ��ġ + ȸ�� + �θ� ���� ����*/
    }

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
