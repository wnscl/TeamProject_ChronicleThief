using UnityEngine;

public class NPCController : MonoBehaviour
{
    public string npcName;             // Inspector에서 설정할 NPC 이름
    public string[] dialogueLines;     // 대사 리스트


    // InteractionTrigger.DoInteract()에서 호출
    public void OnInteract()
    {
        // 1) 대화 띄우기
        Debug.Log("안녕하세요. 상호작용 테스트입니다.");
        UIManager.Instance.ShowDialog(npcName, dialogueLines[0]);
    }
}