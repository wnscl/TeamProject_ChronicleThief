using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;
    public List<SkillData> allSkills;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<SkillData> GetRandomSkills(int count = 3)
    {
        List<SkillData> copy = new List<SkillData>(allSkills);
        List<SkillData> result = new List<SkillData>();

        for (int i = 0; i < count && copy.Count > 0; i++)
        {
            int index = Random.Range(0, copy.Count);
            result.Add(copy[index]);
            copy.RemoveAt(index);
        }

        return result;
    }

    public void UnlockSkill(SkillData skill)
    {
       
    }
}
