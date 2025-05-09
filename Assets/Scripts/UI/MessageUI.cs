using UnityEngine;
using TMPro;

namespace UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        /// 메시지 설정 후 2초 뒤 자동 제거
        public void SetMessage(string message)
        {
            messageText.text = message;
            Destroy(gameObject, 2f);
        }
    }
}