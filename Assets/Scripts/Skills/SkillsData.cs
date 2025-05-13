using UnityEngine;

public enum SkillType { Passive, Active }

[CreateAssetMenu(fileName = "NewSkill", menuName = "Skills/SkillData")]
public class SkillData : ScriptableObject
{
    public string skillName;
    [TextArea] public string description;
    public SkillType type;
    public Sprite icon;
    public GameObject effectPrefab;
}
