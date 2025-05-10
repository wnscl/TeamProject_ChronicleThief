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

        /*Instantiate(original);                           // 가장 기본
        Instantiate(original, position, rotation);       // 위치와 회전 지정
        Instantiate(original, parent);                   // 부모만 지정 (Transform 기준)
        Instantiate(original, position, rotation, parent); // 위치 + 회전 + 부모 전부 지정*/
    }

    public void DestroyTestMonster()
    {
/*        while (transform.FindChild("SkullRunner") != null)
            //스컬러너라는 이름의 오브젝틀 찾지 못할 때 까지
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
                    Debug.Log("몹을 찾지못함");
                }

            }
                
        }

/*        foreach (Transform mob in monster)
        {
            Destroy(mob.gameObject, 1f);
        }*/
    }
}
