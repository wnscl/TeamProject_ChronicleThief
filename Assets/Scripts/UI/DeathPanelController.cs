using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathPanelController : MonoBehaviour
{
    public static DeathPanelController Instance { get; private set; }

    [Header("사망 패널")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TMP_Text titleText;

    void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // 기본 비활성
        panel.SetActive(false);

        // 버튼 이벤트 연결
        mainMenuButton.onClick.AddListener(OnMainMenu);
        quitButton.onClick.AddListener(OnQuit);
    }

    public void Show()
    {
        panel.SetActive(true);
        Time.timeScale = 0f;
    }


    public void Hide()
    {
        panel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainScene");
    }

    private void OnQuit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
        }

}
