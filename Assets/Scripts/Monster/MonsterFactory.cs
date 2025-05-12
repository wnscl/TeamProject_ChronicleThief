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

        *//*Instantiate(original);                           // 가장 기본
        Instantiate(original, position, rotation);       // 위치와 회전 지정
        Instantiate(original, parent);                   // 부모만 지정 (Transform 기준)
        Instantiate(original, position, rotation, parent); // 위치 + 회전 + 부모 전부 지정*//*
    }
*/
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
