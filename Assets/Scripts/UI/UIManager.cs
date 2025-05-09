using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialog UI")]
    public GameObject dialogPanel;  // 대화창 패널
    public TMP_Text speakerText;    // 화자 이름
    public TMP_Text contentText;    // 대사 내용

    [Header("Reward UI")]
    public GameObject rewardPopup;  // 보상 팝업
    public TMP_Text rewardText;     // “골드 +50” 등

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
            OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        }
        else Destroy(gameObject);
    }

    // 씬 전환 시 필요한 UI 컴포넌트 찾아 연결
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var dlg = GameObject.Find("DialogPanel");
        if (dlg == null)
        {
            Debug.LogWarning("[UIManager] DialogPanel을 찾을 수 없습니다.");
            return;
        }

        if (scene.name == "GameScene")
        {
            // 대화창 연결
            dialogPanel = dlg;
            speakerText = dlg.transform.Find("SpeakerText").GetComponent<TMP_Text>();
            contentText = dlg.transform.Find("ContentText").GetComponent<TMP_Text>();
        }
    }

    // NPCController에서 호출
    public void ShowDialog(string speaker, string line)
    {
        dialogPanel.SetActive(true);
        speakerText.text = speaker;
        contentText.text = line;
    }

    public void HideDialog() => dialogPanel.SetActive(false);
}
