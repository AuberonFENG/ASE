using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;

public class ChatManagerTests
{
    private GameObject canvas;
    private ChatManager chatManager;

    private TMP_InputField inputField;
    private Button sendButton;
    private GameObject chatContent;
    private GameObject messagePrefab;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        canvas = new GameObject("Canvas", typeof(Canvas));

        // 创建 InputField
        var inputGO = new GameObject("ChatInput", typeof(RectTransform), typeof(TMP_InputField));
        inputGO.transform.SetParent(canvas.transform);
        inputField = inputGO.GetComponent<TMP_InputField>();
        inputField.text = "";

        // 创建 SendButton
        var buttonGO = new GameObject("SendButton", typeof(RectTransform), typeof(Button));
        buttonGO.transform.SetParent(canvas.transform);
        sendButton = buttonGO.GetComponent<Button>();

        // 创建 ChatContent
        chatContent = new GameObject("ChatContent", typeof(RectTransform), typeof(VerticalLayoutGroup));
        chatContent.transform.SetParent(canvas.transform);

        // 创建 Message Prefab
        messagePrefab = new GameObject("Message", typeof(TextMeshProUGUI));

        // 设置 ChatManager
        var managerGO = new GameObject("ChatManager", typeof(ChatManager));
        chatManager = managerGO.GetComponent<ChatManager>();
        chatManager.chatInput = inputField;
        chatManager.sendButton = sendButton;
        chatManager.chatContent = chatContent;
        chatManager.messagePrefab = messagePrefab;

        chatManager.Start(); // 模拟 Start()

        yield return null;
    }

    [UnityTest]
    public IEnumerator SendMessage_CreatesMessageObjectWithCorrectText()
    {
        // 设定输入内容
        inputField.text = "Hello, VR Chat!";

        // 模拟点击按钮
        sendButton.onClick.Invoke();

        yield return null;

        // 检查是否有子元素被添加
        Assert.AreEqual(1, chatContent.transform.childCount, "应当创建一个新消息对象");

        var createdMessage = chatContent.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(createdMessage, "创建的消息应包含 TextMeshProUGUI 组件");
        Assert.AreEqual("Hello, VR Chat!", createdMessage.text, "消息文本应正确显示输入内容");

        // 检查输入框是否清空
        Assert.IsEmpty(inputField.text, "输入框应在发送后清空");

        // 检查 layoutgroup 是否重新启用（可选）
        var layout = chatContent.GetComponent<VerticalLayoutGroup>();
        Assert.IsNotNull(layout, "ChatContent 上应挂载 VerticalLayoutGroup");
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(canvas);
        Object.Destroy(chatManager.gameObject);
        yield return null;
    }
}
