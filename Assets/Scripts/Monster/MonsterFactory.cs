using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class MonsterFactory : MonoBehaviour
{
    [SerializeField] private GameObject skullRunner;
    [SerializeField] private GameObject skullGiant;
    [SerializeField] private GameObject bowMan;
    [SerializeField] private GameObject bossAron;
    [SerializeField] private GameObject lastBossAron;
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
    public void OnMakeStage2Monster()
    {
        for (int i = 0; i < 10; i++)
        {
            int chance = Random.Range(0, 10);

            minPos = new Vector2(-16, 12.7f);
            maxPos = new Vector2(16,14.9f);
            Vector2 minPos1 = new Vector2(-16, -13f);
            Vector2 maxPos1 = new Vector2(16, -10.8f);
            float randomPosX = Random.Range(minPos.x, maxPos.x);
            float randomPosY = Random.Range(minPos.y, maxPos.y);
            float randomPosX1 = Random.Range(minPos1.x, maxPos1.x);
            float randomPosY1 = Random.Range(minPos1.y, maxPos1.y);

            createPos = new Vector2(randomPosX, randomPosY);
            Vector2 createPos1 = new Vector2(randomPosX1, randomPosY1);

            if (chance < 5)
            {
                Instantiate(bowMan, createPos, Quaternion.identity, this.transform);
            }
            else
            {
                Instantiate(bowMan, createPos1, Quaternion.identity, this.transform);
            }

        }
    }

    public void OnMakeBossMonster(int bossCount)
    {
        Vector2 spawnPos = new Vector2(0, 7);
        Vector2 aronPos = new Vector2(0, 7);
        Vector2 lastAronPos = new Vector2(0, 7);

        switch (bossCount)
        {
            case 1:
                Instantiate(skullGiant, spawnPos, Quaternion.identity, this.transform);
                break;

            case 2:
                Instantiate(bossAron, aronPos, Quaternion.identity, this.transform);
                break;

            case 3:
                Instantiate(lastBossAron, lastAronPos, Quaternion.identity, this.transform);
                break;
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
