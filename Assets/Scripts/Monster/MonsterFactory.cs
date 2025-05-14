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


    public void OnMakeMonster()
    {
        for (int i = 0; i < 10; i++)
        {
            minPos = new Vector2(-6, -6);
            maxPos = new Vector2(6, 6);
            float randomPosX = Random.Range(minPos.x, maxPos.x);
            float randomPosY = Random.Range(minPos.y, maxPos.y);

            createPos = new Vector2(randomPosX, randomPosY);

            Instantiate(skullRunner, createPos, Quaternion.identity, this.transform);
            
        }
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
