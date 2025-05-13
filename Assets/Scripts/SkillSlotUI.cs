using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SkillSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public SkillData skillData;
    public Text cooldownText;
    public Button button;
    private Image buttonImage;

    public Image cooldownOverlay; 

    private bool isOnCooldown = false;
    public float cooldownTime = 5f;

    void Awake()
    {
        if (button == null)
            button = GetComponent<Button>();

        if (button != null)
            buttonImage = button.GetComponent<Image>();
    }

    public void OnClick()
    {
        if (skillData != null && !isOnCooldown)
        {
            TriggerSkill();
        }
    }

    public void TriggerSkill()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("플레이어를 찾을 수 없습니다.");
            return;
        }

        
        GameObject effect = Instantiate(skillData.effectPrefab, player.transform.position, Quaternion.identity);

       
        var follow = effect.GetComponent<FollowPlayerEffect>();
        if (follow != null)
            follow.target = player.transform;

        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        isOnCooldown = true;

        if (buttonImage != null) buttonImage.color = Color.gray;
        if (cooldownOverlay != null) cooldownOverlay.gameObject.SetActive(true);

        float timer = cooldownTime;

        while (timer > 0)
        {
            timer -= Time.deltaTime;
            cooldownText.text = Mathf.Ceil(timer).ToString("F0");
            yield return null;
        }

        cooldownText.text = "";
        isOnCooldown = false;

        if (buttonImage != null) buttonImage.color = Color.white;
        if (cooldownOverlay != null) cooldownOverlay.gameObject.SetActive(false);
    }

    public bool IsCooldown()
    {
        return isOnCooldown;
    }

  
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (skillData != null && TooltipUI.Instance != null)
        {
            TooltipUI.Instance.Show(skillData.description, Input.mousePosition);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TooltipUI.Instance?.Hide();
    }
}
