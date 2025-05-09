using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    [Header("모든 스킬 리스트")]
    public List<SkillData> allSkills;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // 씬 전환해도 유지
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public List<SkillData> GetRandomSkills(int count = 3)
    {
        List<SkillData> available = new List<SkillData>(allSkills);
        List<SkillData> result = new List<SkillData>();

        for (int i = 0; i < count && available.Count > 0; i++)
        {
            int index = Random.Range(0, available.Count);
            result.Add(available[index]);
            available.RemoveAt(index);
        }

        return result;
    }
}
