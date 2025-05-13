using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections.Generic;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager Instance { get; private set; }

        [Header("Wave NPC Spawner")]
        public GameObject waveSpawnerPrefab;
        public Transform[] waveSpawnPositions;
        [HideInInspector] public NPCSpawner currentWaveSpawner;

        [Header("Main NPC Spawners")]
        public GameObject lobbySpawnerPrefab;      // �κ��
        public Transform lobbySpawnPosition;
        [HideInInspector] public NPCSpawner currentLobbySpawner;

        public GameObject wave10SpawnerPrefab;     // 10���̺��
        public Transform wave10SpawnPosition;
        [HideInInspector] public NPCSpawner currentWave10Spawner;

        public GameObject finalSpawnerPrefab;      // 20���̺��
        public Transform finalSpawnPosition;
        [HideInInspector] public NPCSpawner currentFinalSpawner;

        [Header("��ȭ UI")]
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

        // 1) �κ񿡼� �ٷ� ������ ���� (������)
        public void SpawnLobbySpawner()
        {
            if (currentLobbySpawner != null)
                Destroy(currentLobbySpawner.gameObject);

            var go = Instantiate(
                lobbySpawnerPrefab,
                lobbySpawnPosition.position,
                Quaternion.identity,
                transform);
            var sp = go.GetComponent<NPCSpawner>();
            
            sp.Reset();

            sp.spawnOnTrigger = true;
            sp.disableSpawnTimer = true;  // �ڵ� ���� ����
            currentLobbySpawner = sp;
        }

        // 2) ���̺�� ������ (1~9,11~19)
        public void SpawnWaveSpawner(int wave)
        {
            if (currentWaveSpawner != null)
                Destroy(currentWaveSpawner.gameObject);

            if (wave < 1 || wave > waveSpawnPositions.Length) return;

            var go = Instantiate(
                waveSpawnerPrefab,
                waveSpawnPositions[wave - 1].position,
                Quaternion.identity,
                transform);
            var sp = go.GetComponent<NPCSpawner>();
            sp.Reset();
            sp.spawnOnTrigger = true;
            sp.disableSpawnTimer = false;
            currentWaveSpawner = sp;
        }

        // 3) 10���̺� ���� ������
        public void SpawnWave10Spawner()
        {
            if (currentWave10Spawner != null)
                Destroy(currentWave10Spawner.gameObject);

            var go = Instantiate(
                wave10SpawnerPrefab,
                wave10SpawnPosition.position,
                Quaternion.identity,
                transform);
            var sp = go.GetComponent<NPCSpawner>();
            sp.Reset();
            sp.spawnOnTrigger = true;
            sp.disableSpawnTimer = true;  // 1�� ���� ����
            currentWave10Spawner = sp;
        }

        // 4) ���� 20���̺� ���� ������
        public void SpawnFinalSpawner()
        {
            if (currentFinalSpawner != null)
                Destroy(currentFinalSpawner.gameObject);

            var go = Instantiate(
                finalSpawnerPrefab,
                finalSpawnPosition.position,
                Quaternion.identity,
                transform);
            var sp = go.GetComponent<NPCSpawner>();
            sp.Reset();
            sp.spawnOnTrigger = true;
            sp.disableSpawnTimer = true;
            currentFinalSpawner = sp;
        }

        private void Start()
        {
            SpawnLobbySpawner(); // �׽�Ʈ��
        }
    }
}