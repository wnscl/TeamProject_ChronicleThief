using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        public GameObject dialogPanel;  // 대화창 패널
        public TMP_Text speakerText;    // 화자 이름
        public TMP_Text contentText;    // 대사 내용
        public GameObject messageUIPrefab;  // 메시지 UI 프리팹
        public Button useButton, skipButton; //

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
                // 처음 씬도 찾아 주기
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }
            else Destroy(gameObject);
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 씬 이름 체크 제거 → 언제든찾기
            var dlg = GameObject.Find("DialogPanel");
            if (dlg != null)
            {
                dialogPanel = dlg;
                speakerText = dlg.transform.Find("SpeakerText")
                                  .GetComponent<TMP_Text>();
                contentText = dlg.transform.Find("ContentText")
                                  .GetComponent<TMP_Text>();
            }
            else
            {
                Debug.LogWarning($"DialogPanel not found in scene {scene.name}");
            }
        }

        // 대화창 표시
        public void ShowDialog(string speaker, string line)
        {
            dialogPanel.SetActive(true);
            speakerText.text = speaker;
            contentText.text = line;
        }

        // 대화창 숨김
        public void HideDialog() => dialogPanel.SetActive(false);

        // 메시지 표시
        public void ShowMessage(string message)
        {
            var messageUI = Instantiate(messageUIPrefab);
            messageUI.GetComponent<MessageUI>().SetMessage(message);
        }
        public void ShowChoice(string speaker, string line, UnityAction onUse, UnityAction onSkip)
        {
            ShowDialog(speaker, line);
            useButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            useButton.onClick.RemoveAllListeners();
            skipButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(() => { HideChoice(); onUse(); });
            skipButton.onClick.AddListener(() => { HideChoice(); onSkip(); });
        }
        public void ShowSkipOnly(UnityEngine.Events.UnityAction onSkip)
        {
            // 기존 선택용 버튼 숨기기
            HideChoice();

            // 스킵 버튼만 보이도록 활성화
            skipButton.gameObject.SetActive(true);

            // 리스너 설정
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(() => {
                HideChoice();      // 버튼들 숨기기
                onSkip?.Invoke();  // 업그레이드 패널 숨기기 등
            });
        }

        public void HideChoice()
        {
            useButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }
    }
}