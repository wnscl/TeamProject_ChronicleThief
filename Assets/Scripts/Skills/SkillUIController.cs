using UnityEngine;

public class SkillUIController : MonoBehaviour
{
    public SkillSlotUI slot1;
    public SkillSlotUI slot2;
    public SkillSlotUI slot3;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z) && slot1 != null && !slot1.IsCooldown())
            slot1.TriggerSkill();

        if (Input.GetKeyDown(KeyCode.X) && slot2 != null && !slot2.IsCooldown())
            slot2.TriggerSkill();

        if (Input.GetKeyDown(KeyCode.C) && slot3 != null && !slot3.IsCooldown())
            slot3.TriggerSkill();
    }
}
