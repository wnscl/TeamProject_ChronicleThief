using UnityEngine;
using TMPro;

namespace UI
{
    public class MessageUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text messageText;

        /// �޽��� ���� �� 2�� �� �ڵ� ����
        public void SetMessage(string message)
        {
            messageText.text = message;
            Destroy(gameObject, 2f);
        }
    }
}