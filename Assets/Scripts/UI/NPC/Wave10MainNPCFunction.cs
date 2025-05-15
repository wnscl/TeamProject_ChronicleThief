using UnityEngine;
using NPC;
using UI;
using System.Collections;

public class Wave10MainNPCFunction : MonoBehaviour, INPCFunction
{
    private NPCController npcController;
    private Animator animator;
    private bool firstTriggered = false;

    private MapScroll mapScrollA;
    private MapScroll mapScrollB;
    private GameObject mapInteraction;

    void Awake()
    {
        npcController = GetComponent<NPCController>();
        animator = GetComponent<Animator>();

        var goA = GameObject.Find("InfitMap");
        if (goA != null) mapScrollA = goA.GetComponent<MapScroll>();

        var goB = GameObject.Find("InfitMap (1)");
        if (goB != null) mapScrollB = goB.GetComponent<MapScroll>();

        mapInteraction = GameObject.Find("Stage/Stage 2 - DungeonFloor2/InfitMap (1)/MapInteraction");
        if (mapInteraction == null)
            Debug.LogWarning("MapInteraction을 찾지 못했습니다!");
    }

    public void Execute(GameObject interactor)
    {
        var ui = UIManager.Instance;
        var bsm = BattleSystemManager.Instance;
        string speaker = npcController.npcName;

        // 첫 상호작용 전
        if (!firstTriggered)
        {
            // 1) 대화창 [0]으로 열기
            ui.ShowDialog(speaker, npcController.dialogueLines[0]);

            // 2) Skip 버튼만
            ui.ShowSkipOnly(() =>
            {
                // 맵 스크롤 활성화
                if (mapScrollA != null) mapScrollA.scrollSpeed = 2f;
                if (mapScrollB != null) mapScrollB.scrollSpeed = 2f;

                // 대화창에 [1]로 업데이트
                ui.ShowDialog(speaker, npcController.dialogueLines[1]);

                firstTriggered = true;

                // 1초 뒤 대화창 닫기
                StartCoroutine(HideDialogAfterDelay(1f));
            });
        }
        // 두 번째 이후, 19웨이브 이전
        else if (bsm.waveCount < 19)
        {
            // 1) 대화창 [2]로 열기
            ui.ShowDialog(speaker, npcController.dialogueLines[2]);

            // 2) Skip 버튼만
            ui.ShowSkipOnly(() =>
            {
                ui.ShowDialog(speaker, npcController.dialogueLines[3]);
                StartCoroutine(HideDialogAfterDelay(1f));
            });
        }
        // 19웨이브 클리어 시
        else
        {
            // 1) 대화창 [4]로 열기
            ui.ShowDialog(speaker, npcController.dialogueLines[4]);

            // 2) Use + Skip 모두
            ui.ShowChoice(
                speaker,
                npcController.dialogueLines[4],
                // Use 버튼 콜백
                () =>
                {
                    if (mapInteraction != null)
                        mapInteraction.SetActive(true);

                    ui.ShowDialog(speaker, npcController.dialogueLines[5]);

                    // 소환 해제 애니메이터 트리거
                    if (animator != null)
                        animator.SetTrigger("SpawnReleased");

                    StartCoroutine(HideDialogAfterDelay(1f));
                },
                // Skip 버튼 콜백
                () =>
                {
                    ui.ShowDialog(speaker, npcController.dialogueLines[6]);
                    StartCoroutine(HideDialogAfterDelay(1f));
                }
            );
        }
    }

    private IEnumerator HideDialogAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        UIManager.Instance.HideDialog();
    }
}