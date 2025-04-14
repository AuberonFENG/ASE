using UnityEngine;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;

public class UIFollowCameraTests
{
    private GameObject cameraGO;
    private Camera camera;
    private GameObject uiObject;
    private UIFollowCamera uiFollowCamera;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // 创建并设置主摄像机
        cameraGO = new GameObject("MainCamera");
        camera = cameraGO.AddComponent<Camera>();
        camera.tag = "MainCamera"; // 这样 Camera.main 才能识别它
        cameraGO.transform.position = Vector3.zero;
        cameraGO.transform.forward = Vector3.forward;

        // 创建 UI Object 并挂载脚本
        uiObject = new GameObject("UIObject");
        uiFollowCamera = uiObject.AddComponent<UIFollowCamera>();
        uiFollowCamera.distanceFromCamera = 2f; // 自定义距离
        // 不设置 mainCamera，让它自动使用 Camera.main

        yield return null;
    }

    [UnityTest]
    public IEnumerator UIFollowsCameraPositionAndRotation()
    {
        // 手动调用 Start() 模拟 Unity 生命周期
        uiFollowCamera.Start();

        // 运行一帧 Update
        uiFollowCamera.Update();

        // 1. 检查是否自动获取到了 Camera.main
        Assert.IsNotNull(uiFollowCamera.mainCamera, "未手动指定相机时应自动获取 Camera.main");

        // 2. 检查位置是否正确在相机正前方 distanceFromCamera 位置
        Vector3 expectedPosition = cameraGO.transform.position + cameraGO.transform.forward * uiFollowCamera.distanceFromCamera;
        Assert.That(uiObject.transform.position, Is.EqualTo(expectedPosition).Using(Vector3ComparerWithTolerance(0.01f)), "UI 应该在摄像机正前方");

        // 3. 检查 UI 是否朝向相机
        Vector3 toCamera = (cameraGO.transform.position - uiObject.transform.position).normalized;
        Vector3 uiForward = uiObject.transform.forward;
        //float dot = Vector3.Dot(uiForward, toCamera);
        float dot = Vector3.Dot(-uiObject.transform.forward, toCamera);
        Assert.That(dot, Is.GreaterThan(0.99f), "UI 应该面朝摄像机");

        yield return null;
    }

    [UnityTearDown]
    public IEnumerator TearDown()
    {
        Object.Destroy(cameraGO);
        Object.Destroy(uiObject);
        yield return null;
    }

    // 向量比较帮助方法
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
}
