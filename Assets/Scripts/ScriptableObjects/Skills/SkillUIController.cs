using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    public static SkillUIController Instance;

    public GameObject skillPanel;
    public SkillSlotUI[] slots; 

    private void Awake()
    {
        Instance = this;
    }

    public void ShowSkills()
    {
        skillPanel.SetActive(true);

        var skills = SkillManager.Instance.GetRandomSkills();
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].SetSkill(skills[i]);
        }
    }

    public void HidePanel()
    {
        skillPanel.SetActive(false);
    }
}
