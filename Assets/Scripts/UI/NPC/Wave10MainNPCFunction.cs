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
            Debug.LogWarning("MapInteraction�� ã�� ���߽��ϴ�!");
    }

    public void Execute(GameObject interactor)
    {
        var ui = UIManager.Instance;
        var bsm = BattleSystemManager.Instance;
        string speaker = npcController.npcName;

        // ù ��ȣ�ۿ� ��
        if (!firstTriggered)
        {
            // 1) ��ȭâ [0]���� ����
            ui.ShowDialog(speaker, npcController.dialogueLines[0]);

            // 2) Skip ��ư��
            ui.ShowSkipOnly(() =>
            {
                // �� ��ũ�� Ȱ��ȭ
                if (mapScrollA != null) mapScrollA.scrollSpeed = 2f;
                if (mapScrollB != null) mapScrollB.scrollSpeed = 2f;

                // ��ȭâ�� [1]�� ������Ʈ
                ui.ShowDialog(speaker, npcController.dialogueLines[1]);

                firstTriggered = true;

                // 1�� �� ��ȭâ �ݱ�
                StartCoroutine(HideDialogAfterDelay(1f));
            });
        }
        // �� ��° ����, 19���̺� ����
        else if (bsm.waveCount < 19)
        {
            // 1) ��ȭâ [2]�� ����
            ui.ShowDialog(speaker, npcController.dialogueLines[2]);

            // 2) Skip ��ư��
            ui.ShowSkipOnly(() =>
            {
                ui.ShowDialog(speaker, npcController.dialogueLines[3]);
                StartCoroutine(HideDialogAfterDelay(1f));
            });
        }
        // 19���̺� Ŭ���� ��
        else
        {
            // 1) ��ȭâ [4]�� ����
            ui.ShowDialog(speaker, npcController.dialogueLines[4]);

            // 2) Use + Skip ���
            ui.ShowChoice(
                speaker,
                npcController.dialogueLines[4],
                // Use ��ư �ݹ�
                () =>
                {
                    if (mapInteraction != null)
                        mapInteraction.SetActive(true);

                    ui.ShowDialog(speaker, npcController.dialogueLines[5]);

                    // ��ȯ ���� �ִϸ����� Ʈ����
                    if (animator != null)
                        animator.SetTrigger("SpawnReleased");

                    StartCoroutine(HideDialogAfterDelay(1f));
                },
                // Skip ��ư �ݹ�
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