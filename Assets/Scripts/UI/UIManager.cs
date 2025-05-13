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
        public GameObject lobbySpawnerPrefab;      // 로비용
        public Transform lobbySpawnPosition;
        [HideInInspector] public NPCSpawner currentLobbySpawner;

        public GameObject wave10SpawnerPrefab;     // 10웨이브용
        public Transform wave10SpawnPosition;
        [HideInInspector] public NPCSpawner currentWave10Spawner;

        public GameObject finalSpawnerPrefab;      // 20웨이브용
        public Transform finalSpawnPosition;
        [HideInInspector] public NPCSpawner currentFinalSpawner;

        [Header("대화 UI")]
        public GameObject dialogPanel;  // 대화창 패널
        public TMP_Text speakerText;    // 화자 이름
        public TMP_Text contentText;    // 대사 내용
        public GameObject messageUIPrefab;  // 메시지 UI 프리팹
        public Button useButton, skipButton; // 수락, 뒤로가기 버튼

        [Header("스폰 타이머")]
        public GameObject spawnTimerPrefab;   // Inspector에 연결할 SpawnTimerUI 프리팹
        private SpawnTimer spawnTimer;

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

            var canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("UIManager: 씬에 Canvas가 없습니다!");
                return;
            }

            // (2) Canvas 하위에 SpawnTimerUI 인스턴스 생성
            var go = Instantiate(spawnTimerPrefab, canvas.transform, false); // worldPositionStays 값은 false로 해줘야 RectTransform 값 그대로 유지됨
            spawnTimer = go.GetComponent<SpawnTimer>();
            if (spawnTimer == null)
                Debug.LogError("UIManager: SpawnTimer 컴포넌트가 없습니다!");
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

        // 1) 로비에서 바로 스포너 생성 (스폰만)
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
            sp.disableSpawnTimer = true;  // 자동 해제 없음
            currentLobbySpawner = sp;
        }

        // 2) 웨이브용 스포너 (1~9,11~19)
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

        // 3) 10웨이브 전용 스포너
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
            sp.disableSpawnTimer = true;  // 1분 제한 없음
            currentWave10Spawner = sp;
        }

        // 4) 최종 20웨이브 전용 스포너
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
            SpawnLobbySpawner(); // 테스트용
        }
    }
}