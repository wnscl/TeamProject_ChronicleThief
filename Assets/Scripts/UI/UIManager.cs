using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Dialog UI")]
    public GameObject dialogPanel;  // ��ȭâ �г�
    public TMP_Text speakerText;    // ȭ�� �̸�
    public TMP_Text contentText;    // ��� ����

    [Header("Reward UI")]
    public GameObject rewardPopup;  // ���� �˾�
    public TMP_Text rewardText;     // ����� +50�� ��

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
    }

    // �� ��ȯ �� �ʿ��� UI ������Ʈ ã�� ����
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        var dlg = GameObject.Find("DialogPanel");
        if (dlg == null)
        {
            Debug.LogWarning("[UIManager] DialogPanel�� ã�� �� �����ϴ�.");
            return;
        }

        if (scene.name == "GameScene")
        {
            // ��ȭâ ����
            dialogPanel = dlg;
            speakerText = dlg.transform.Find("SpeakerText").GetComponent<TMP_Text>();
            contentText = dlg.transform.Find("ContentText").GetComponent<TMP_Text>();
        }
    }

    // NPCController���� ȣ��
    public void ShowDialog(string speaker, string line)
    {
        dialogPanel.SetActive(true);
        speakerText.text = speaker;
        contentText.text = line;
    }

    public void HideDialog() => dialogPanel.SetActive(false);
}
