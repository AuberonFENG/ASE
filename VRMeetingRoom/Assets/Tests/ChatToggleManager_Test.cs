using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ChatToggleManagerTests
{
    private GameObject chatPanel;
    private Button toggleButton;
    private ChatToggleManager manager;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // 创建 ChatPanel（UI 物体）
        chatPanel = new GameObject("ChatPanel");
        chatPanel.SetActive(true); // 初始我们先设为 true，测试 Start() 里是否会关闭它

        // 创建 ToggleButton
        var toggleButtonGO = new GameObject("ToggleButton", typeof(Button));
        toggleButton = toggleButtonGO.GetComponent<Button>();

        // 创建 ChatToggleManager
        var managerGO = new GameObject("ChatToggleManager", typeof(ChatToggleManager));
        manager = managerGO.GetComponent<ChatToggleManager>();
        manager.chatPanel = chatPanel;
        manager.toggleButton = toggleButton;

        // 模拟 Unity 的 Start 生命周期函数
        manager.Start();

        yield return null;
    }

    [UnityTest]
    public IEnumerator ToggleChatPanel_TogglesVisibilityCorrectly()
    {
        // 启动后，ChatPanel 应该是隐藏的
        Assert.IsFalse(chatPanel.activeSelf, "ChatPanel 应该在 Start() 时被隐藏");

        // 直接调用方法而非点击按钮
        manager.ToggleChatPanel();
        yield return null;
        Assert.IsTrue(chatPanel.activeSelf, "第一次调用后，ChatPanel 应该被显示");

        manager.ToggleChatPanel();
        yield return null;
        Assert.IsFalse(chatPanel.activeSelf, "第二次调用后，ChatPanel 应该被隐藏");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(chatPanel);
        Object.Destroy(toggleButton.gameObject);
        Object.Destroy(manager.gameObject);
        yield return null;
    }
}
