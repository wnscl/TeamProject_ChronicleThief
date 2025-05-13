using UnityEngine;
using System.Collections;
using NPC;
using UI;

public class LobbyMainNPCFunction : MonoBehaviour, INPCFunction
{
    [Tooltip("������� �ִϸ��̼� ��� �� NPC�� �ı��ϱ� �� ��� �ð�(��)")]
    public float despawnAnimationDuration = 0.3f; // ����npc�� Ÿ�̸Ӹ� �����Ű�� �ʱ� ������ Ÿ�̸� �ڷ�ƾ���� �� ���� �޾ƿ� �� ����. ���� ���� ����.

    [Tooltip("���̾�α� �г��� �ڵ����� ����� �� ��� �ð�(��)")]
    public float dialogHideDelay = 1.0f;


    private Animator animator;

    void Awake()
    {
        // Animator�� �̸� ĳ���� �Ӵϴ�.
        animator = GetComponent<Animator>();
    }

    // INPCFunction.Execute: 'Ư�� ��� ���' ���� �ÿ��� ȣ��˴ϴ�.
    public void Execute(GameObject interactor)
    {
        // 1) �κ� ���� Collider ��Ȱ��ȭ
        var col = GameObject.Find("Stage/Stage 0 - MainLobby/Collider/InteractionCollision");
        if (col != null)
            col.SetActive(false);

        // 2) NPC �ִϸ��̼ǿ� SpawnReleased Ʈ����
        if (animator != null)
            animator.SetTrigger("SpawnReleased");

        // 3) �ִϸ��̼� ��� �ð� �Ŀ� NPC ������Ʈ �ı�
        Destroy(gameObject, despawnAnimationDuration);

        // 4) UIManager (���� ������Ʈ) ���� 2�� �ڿ� HideDialog ȣ��
        UIManager.Instance.Invoke(nameof(UIManager.HideDialog), dialogHideDelay);
    }
}