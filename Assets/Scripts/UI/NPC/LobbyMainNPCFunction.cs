using UnityEngine;
using System.Collections;
using NPC;
using UI;

public class LobbyMainNPCFunction : MonoBehaviour, INPCFunction
{
    [Tooltip("사라지는 애니메이션 재생 후 NPC를 파괴하기 전 대기 시간(초)")]
    public float despawnAnimationDuration = 0.3f; // 메인npc는 타이머를 진행시키지 않기 때문에 타이머 코루틴에서 이 값을 받아올 수 없음. 따라서 새로 선언.

    [Tooltip("다이얼로그 패널을 자동으로 숨기기 전 대기 시간(초)")]
    public float dialogHideDelay = 1.0f;


    private Animator animator;

    void Awake()
    {
        // Animator를 미리 캐싱해 둡니다.
        animator = GetComponent<Animator>();
    }

    // INPCFunction.Execute: '특수 기능 사용' 선택 시에만 호출됩니다.
    public void Execute(GameObject interactor)
    {
        // 1) 로비 입장 Collider 비활성화
        var col = GameObject.Find("Stage/Stage 0 - MainLobby/Collider/InteractionCollision");
        if (col != null)
            col.SetActive(false);

        // 2) NPC 애니메이션에 SpawnReleased 트리거
        if (animator != null)
            animator.SetTrigger("SpawnReleased");

        // 3) 애니메이션 재생 시간 후에 NPC 오브젝트 파괴
        Destroy(gameObject, despawnAnimationDuration);

        // 4) UIManager (영속 오브젝트) 에서 2초 뒤에 HideDialog 호출
        UIManager.Instance.Invoke(nameof(UIManager.HideDialog), dialogHideDelay);
    }
}