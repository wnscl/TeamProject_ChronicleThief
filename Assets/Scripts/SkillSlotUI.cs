using UnityEngine;
using UnityEngine.UI;

public class SkillSlotUI : MonoBehaviour
{
    public Image iconImage;       
    public Text skillNameText;    
    public SkillData skillData;     

    public void SetData(SkillData data)
    {
        skillData = data;
        iconImage.sprite = data.icon;
        skillNameText.text = data.skillName;
    }

    public void OnClick()
    {
        Debug.Log($"[���õ� ��ų] {skillData.skillName}");
        // SkillManager.Instance.UnlockSkill(skillData);
    }
  

}
