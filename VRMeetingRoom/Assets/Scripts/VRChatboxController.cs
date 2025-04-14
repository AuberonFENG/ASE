using UnityEngine;
using UnityEngine.UI;

public class VRChatboxController : MonoBehaviour
{
    public GameObject chatboxUI;   // 拖入 Chatbox 面板
    public Button toggleButton;    // 拖入 UI 按钮
    public Transform playerCamera; // 拖入玩家摄像机 (VR 主摄像机)

    public float distanceFromCamera = 2f; // UI 显示的默认距离
    public Vector3 offset = new Vector3(0, -0.5f, 0); // 偏移量

    public void Start()
    {
        chatboxUI.SetActive(true); // 确保 UI 始终激活
        toggleButton.onClick.AddListener(MoveChatboxToFront);
    }

    public void MoveChatboxToFront()
    {
        // 计算 UI 在玩家前方的位置
        Vector3 newPosition = playerCamera.position + playerCamera.forward * distanceFromCamera + offset;
        chatboxUI.transform.position = newPosition;

        // 让 Chatbox 始终面向玩家
        chatboxUI.transform.LookAt(playerCamera);
        chatboxUI.transform.Rotate(0, 180, 0);
    }
}