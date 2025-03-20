using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class RemoveAllMarkTests
{
    private GameObject testObject;
    private RemoveAllMark removeAllMark;
    private RenderTexture testRenderTexture;

    [SetUp]
    public void Setup()
    {
        // 创建一个测试 GameObject 并添加 RemoveAllMark 组件
        testObject = new GameObject("TestObject");
        removeAllMark = testObject.AddComponent<RemoveAllMark>();

        // 创建 RenderTexture 并赋值
        testRenderTexture = new RenderTexture(256, 256, 16);
        removeAllMark.targetRenderTexture = testRenderTexture;

        // 确保 RenderTexture 处于已创建状态
        testRenderTexture.Create();
    }

    [UnityTest]
    public IEnumerator TestRenderTextureIsReleased()
    {
        // 确保 RenderTexture 处于已创建状态
        Assert.IsTrue(testRenderTexture.IsCreated(), "RenderTexture should be created before the test.");

        // 直接调用 `OnToggleMenu()` 方法，测试 Release 功能
        removeAllMark.GetType()
            .GetMethod("OnToggleMenu", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)?
            .Invoke(removeAllMark, new object[] { null });

        // 等待一帧，确保事件触发
        yield return null;

        // 检查 RenderTexture 是否被释放
        Assert.IsFalse(testRenderTexture.IsCreated(), "RenderTexture should be released.");
    }

    [TearDown]
    public void Teardown()
    {
        // 清理对象
        GameObject.DestroyImmediate(testObject);
        if (testRenderTexture != null)
        {
            testRenderTexture.Release();
            GameObject.DestroyImmediate(testRenderTexture);
        }
    }
}
