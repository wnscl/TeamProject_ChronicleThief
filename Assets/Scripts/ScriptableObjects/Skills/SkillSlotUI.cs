using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    public Image iconImage;
    public Text nameText;
    private SkillData skill;

    public void SetSkill(SkillData newSkill)
    {
        skill = newSkill;
        iconImage.sprite = skill.icon;
        nameText.text = skill.skillName;
    }

    public void OnClick()
    {
        SkillManager.Instance.UnlockSkill(skill);
        SkillUIController.Instance.HidePanel();
    }
}
