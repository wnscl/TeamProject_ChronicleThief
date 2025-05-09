using UnityEngine;

public class NPCController : MonoBehaviour
{
    public string npcName;             // Inspector���� ������ NPC �̸�
    public string[] dialogueLines;     // ��� ����Ʈ


    // InteractionTrigger.DoInteract()���� ȣ��
    public void OnInteract()
    {
        // 1) ��ȭ ����
        Debug.Log("�ȳ��ϼ���. ��ȣ�ۿ� �׽�Ʈ�Դϴ�.");
        UIManager.Instance.ShowDialog(npcName, dialogueLines[0]);
    }
}