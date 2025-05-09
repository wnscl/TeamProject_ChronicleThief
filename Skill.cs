using UnityEngine;

public enum SkillType
{
    Passive,    // �ɷ�ġ ����, ü�� ȸ�� ��
    Active      // �ߵ���
}

[System.Serializable]
public class Skill
{
    public string skillName;         // ��ų �̸�
    public string description;       // ��ų ����
    public Sprite icon;              // UI���� ����� ������ �̹���
    public SkillType type;           // ��ų Ÿ�� (Passive / Active)
    public GameObject effectPrefab;  // ��Ƽ�� ��ų�� ���, ����Ʈ ������
}
