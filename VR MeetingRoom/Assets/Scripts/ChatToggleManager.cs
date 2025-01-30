using UnityEngine;
using UnityEngine.UI;

public class ChatToggleManager : MonoBehaviour
{
    public GameObject chatPanel; // 需要控制的 ChatPanel
    public Button toggleButton; // 控制 ChatPanel 的按钮

    void Start()
    {
        // 确保 ChatPanel 一开始是隐藏的
        chatPanel.SetActive(false);

        // 按钮监听点击事件
        toggleButton.onClick.AddListener(ToggleChatPanel);
    }

    void ToggleChatPanel()
    {
        // 切换 ChatPanel 的可见性
        chatPanel.SetActive(!chatPanel.activeSelf);
    }
}
