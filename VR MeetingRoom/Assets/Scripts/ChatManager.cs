using UnityEngine;
using UnityEngine.UI;
using TMPro; // 引入 TMP 命名空间

public class ChatManager : MonoBehaviour
{
    public TMP_InputField chatInput; // 这里改为 TMP_InputField
    public Button sendButton;
    public GameObject chatContent;
    public GameObject messagePrefab; // 聊天消息预制体

    void Start()
    {
        sendButton.onClick.AddListener(SendMessage);
    }

    void SendMessage()
    {
        if (!string.IsNullOrEmpty(chatInput.text))
        {
            // 创建新消息
            GameObject newMessage = Instantiate(messagePrefab, chatContent.transform);
            newMessage.GetComponent<TextMeshProUGUI>().text = chatInput.text; // 改为 TextMeshProUGUI

            // 清空输入框
            chatInput.text = "";

            // 让滚动视图滚动到底部
            Canvas.ForceUpdateCanvases();
            chatContent.GetComponent<VerticalLayoutGroup>().enabled = false;
            chatContent.GetComponent<VerticalLayoutGroup>().enabled = true;
        }
    }
}
