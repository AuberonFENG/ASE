using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class VRChatboxControllerTests
{
    private GameObject playerCameraObject;
    private GameObject chatboxUIObject;
    private GameObject toggleButtonObject;
    private VRChatboxController controller;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // 创建模拟摄像机对象
        playerCameraObject = new GameObject("PlayerCamera", typeof(Camera));
        playerCameraObject.transform.position = Vector3.zero;
        playerCameraObject.transform.forward = Vector3.forward;

        // 创建 Chatbox UI
        chatboxUIObject = new GameObject("ChatboxUI");
        chatboxUIObject.AddComponent<Canvas>();

        // 创建 Toggle Button
        toggleButtonObject = new GameObject("ToggleButton");
        var button = toggleButtonObject.AddComponent<UnityEngine.UI.Button>();

        // 创建控制器
        var controllerObject = new GameObject("Controller");
        controller = controllerObject.AddComponent<VRChatboxController>();
        controller.playerCamera = playerCameraObject.transform;
        controller.chatboxUI = chatboxUIObject;
        controller.toggleButton = button;
        controller.distanceFromCamera = 2f;
        controller.offset = new Vector3(0, -0.5f, 0);

        controller.Start(); // 手动调用 Start()

        yield return null;
    }

    [UnityTest]
    public IEnumerator ChatboxMovesToCorrectPositionAndFacesCamera()
    {
        // 记录初始位置
        Vector3 expectedPosition = playerCameraObject.transform.position + playerCameraObject.transform.forward * 2f + new Vector3(0, -0.5f, 0);

        // 模拟点击按钮
        controller.MoveChatboxToFront();

        // 等待一帧更新
        yield return null;

        // 检查位置是否正确
        Assert.That(chatboxUIObject.transform.position, Is.EqualTo(expectedPosition).Using(Vector3ComparerWithTolerance(0.01f)));

        // 检查是否面向摄像机（反向朝向）
        Vector3 actualForward = chatboxUIObject.transform.forward;
        Vector3 expectedBackward = (playerCameraObject.transform.position - expectedPosition).normalized;
        Assert.That(Vector3.Dot(actualForward, -expectedBackward), Is.GreaterThan(0.99f));

    }

    private static IEqualityComparer<Vector3> Vector3ComparerWithTolerance(float tolerance)
    {
        return new Vector3EqualityComparer(tolerance);
    }

    private class Vector3EqualityComparer : IEqualityComparer<Vector3>
    {
        private float tolerance;
        public Vector3EqualityComparer(float tolerance)
        {
            this.tolerance = tolerance;
        }

        public bool Equals(Vector3 a, Vector3 b)
        {
            return Vector3.Distance(a, b) < tolerance;
        }

        public int GetHashCode(Vector3 obj)
        {
            return obj.GetHashCode();
        }
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(playerCameraObject);
        Object.Destroy(chatboxUIObject);
        Object.Destroy(toggleButtonObject);
        Object.Destroy(controller.gameObject);
        yield return null;
    }
}
