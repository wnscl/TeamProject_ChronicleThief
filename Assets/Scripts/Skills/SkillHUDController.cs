using UnityEngine;
using UnityEngine.UI;

public class SkillHUDController : MonoBehaviour
{
    public static SkillHUDController Instance;

    public Image skillIconImage;
    public Text skillNameText;

    private void Awake() => Instance = this;

    public void ShowSkill(SkillData skill)
    {
        skillIconImage.sprite = skill.icon;
        skillNameText.text = skill.skillName;
        gameObject.SetActive(true);
    }
}
