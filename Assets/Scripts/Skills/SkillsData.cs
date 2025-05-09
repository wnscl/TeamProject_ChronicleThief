using UnityEngine;

public enum SkillType
{
    Passive,
    Active
}

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/Create New Skill")]
public class SkillData : ScriptableObject
{
    public string skillName;
    [TextArea]
    public string description;
    public Sprite icon;
    public SkillType type;
    public GameObject effectPrefab;
}
