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
        public Button useButton, skipButton; // ����, �ڷΰ��� ��ư

        [Header("���� Ÿ�̸�")]
        public GameObject spawnTimerPrefab;   // Inspector�� ������ SpawnTimerUI ������
        private SpawnTimer spawnTimer;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
                // ó�� ���� ã�� �ֱ�
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
            }
            else Destroy(gameObject);

            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("UIManager: ���� Canvas�� �����ϴ�!");
                return;
            }

            // (2) Canvas ������ SpawnTimerUI �ν��Ͻ� ����
            var go = Instantiate(spawnTimerPrefab, canvas.transform, false); // worldPositionStays ���� false�� ����� RectTransform �� �״�� ������
            spawnTimer = go.GetComponent<SpawnTimer>();
            if (spawnTimer == null)
                Debug.LogError("UIManager: SpawnTimer ������Ʈ�� �����ϴ�!");
        }

        void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
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
            // ���� ���ÿ� ��ư �����
            HideChoice();

            // ��ŵ ��ư�� ���̵��� Ȱ��ȭ
            skipButton.gameObject.SetActive(true);

            // ������ ����
            skipButton.onClick.RemoveAllListeners();
            skipButton.onClick.AddListener(() => {
                HideChoice();      // ��ư�� �����
                onSkip?.Invoke();  // ���׷��̵� �г� ����� ��
            });
        }

        public void HideChoice()
        {
            useButton.gameObject.SetActive(false);
            skipButton.gameObject.SetActive(false);
        }
    }
}