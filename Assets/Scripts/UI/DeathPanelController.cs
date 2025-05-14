using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DeathPanelController : MonoBehaviour
{
    public static DeathPanelController Instance { get; private set; }

    [Header("��� �г�")]
    [SerializeField] private GameObject panel;
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private TMP_Text titleText;

    void Awake()
    {
        // �̱��� �ʱ�ȭ
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // �⺻ ��Ȱ��
        panel.SetActive(false);

        // ��ư �̺�Ʈ ����
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
