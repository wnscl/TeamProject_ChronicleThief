using UnityEngine;

public class PlayerSkillSet : MonoBehaviour
{
    public static PlayerSkillSet Instance;
    private void Awake() => Instance = this;

    public void ApplySkill(SkillData skill)
    {
        if (skill.type == SkillType.Active && skill.effectPrefab != null)
        {
            Instantiate(skill.effectPrefab, transform.position, Quaternion.identity);
        }
    }
   

}

