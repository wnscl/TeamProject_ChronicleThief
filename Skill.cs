using UnityEngine;

public enum SkillType
{
    Passive,    // 능력치 증가, 체력 회복 등
    Active      // 발동형
}

[System.Serializable]
public class Skill
{
    public string skillName;         // 스킬 이름
    public string description;       // 스킬 설명
    public Sprite icon;              // UI에서 사용할 아이콘 이미지
    public SkillType type;           // 스킬 타입 (Passive / Active)
    public GameObject effectPrefab;  // 액티브 스킬의 경우, 이펙트 프리팹
}
