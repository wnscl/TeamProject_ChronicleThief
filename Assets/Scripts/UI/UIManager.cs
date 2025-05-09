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

        public GameObject dialogPanel;  // ��ȭâ �г�
        public TMP_Text speakerText;    // ȭ�� �̸�
        public TMP_Text contentText;    // ��� ����
        public GameObject messageUIPrefab;  // �޽��� UI ������
        public Button useButton, skipButton; // qjxms

        void Awake()
        {
            // �̱��� �ʱ�ȭ
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }
            else Destroy(gameObject);

            useButton = dialogPanel.transform.Find("UseButton").GetComponent<Button>();
            skipButton = dialogPanel.transform.Find("SkipButton").GetComponent<Button>();
            HideChoice();
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            var dlg = GameObject.Find("DialogPanel");
            if (dlg != null && scene.name == "GameScene")
            {
                dialogPanel = dlg;
                speakerText = dlg.transform.Find("SpeakerText").GetComponent<TMP_Text>();
                contentText = dlg.transform.Find("ContentText").GetComponent<TMP_Text>();
            }
        }

        // ��ȭâ ǥ��
        public void ShowDialog(string speaker, string line)
        {
            dialogPanel.SetActive(true);
            speakerText.text = speaker;
            contentText.text = line;
        }

        // ��ȭâ ����
        public void HideDialog() => dialogPanel.SetActive(false);

        // �޽��� ǥ��
        public void ShowMessage(string message)
        {
            var messageUI = Instantiate(messageUIPrefab);
            messageUI.GetComponent<MessageUI>().SetMessage(message);
        }
        void HideChoice()
        {
            useButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }

        public void ShowChoice(string speaker, string line,
                       UnityAction onUse, UnityAction onSkip)
        {
            ShowDialog(speaker, line);
            useButton.gameObject.SetActive(true);
            skipButton.gameObject.SetActive(true);
            useButton.onClick.RemoveAllListeners();
            skipButton.onClick.RemoveAllListeners();
            useButton.onClick.AddListener(() => { HideChoice(); onUse(); });
            skipButton.onClick.AddListener(() => { HideChoice(); onSkip(); });
        }
    }
}