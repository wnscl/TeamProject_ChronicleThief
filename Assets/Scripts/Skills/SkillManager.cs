using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public void UnlockSkill(SkillData skill)
    {
        PlayerSkillSet.Instance.ApplySkill(skill);
        SkillHUDController.Instance.ShowSkill(skill);
    }
}
