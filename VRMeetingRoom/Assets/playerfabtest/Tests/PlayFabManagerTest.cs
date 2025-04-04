using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManagerTest
{
    private PlayFabManager playFabManager;
    

    [SetUp]
    public void Setup()
    {
        // 初始化 PlayFabManager
        playFabManager = new GameObject("PlayFabManager").AddComponent<PlayFabManager>();
    }

    [TearDown]
    public void Teardown()
    {
        if (playFabManager != null)
        {
            Object.DestroyImmediate(playFabManager.gameObject);
        }
    }
    
    [UnityTest]
    public IEnumerator TestRegisterPlayer()
    {
        string testUsername = "TestUser" + System.Guid.NewGuid().ToString("N").Substring(0, 8);
        string testPassword = "TestPass123";

        var isSuccess = false;
        var isComplete = false;

        PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest
        {
            Username = testUsername,
            Password = testPassword,
            RequireBothUsernameAndEmail = false
        }, result =>
        {
            isSuccess = true;
            isComplete = true;
            Debug.Log("Register Success!");
        }, error =>
        {
            isSuccess = false;
            isComplete = true;
            Debug.LogError("Error Code: " + error.Error);
            Debug.LogError("Error Message: " + error.ErrorMessage);
            Debug.LogError("Full Error Report: " + error.GenerateErrorReport());
        });

        // 等待回调完成
        yield return new WaitUntil(() => isComplete);

        // 断言注册是否成功
        Assert.IsTrue(isSuccess, "Registration should be successful.");
    }

    [UnityTest]
    public IEnumerator TestLoginPlayer()
    {
        string testUsername = "jerry0917";
        string testPassword = "123456";

        bool isSuccess = false;
        bool isComplete = false;

        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest
        {
            Username = testUsername,
            Password = testPassword,
        }, result =>
        {
            isSuccess = true;
            isComplete = true;
            Debug.Log("Login Success!");
        }, error =>
        {
            isSuccess = false;
            isComplete = true;
            Debug.LogError("Login Failed: " + error.GenerateErrorReport());
        });

        yield return new WaitUntil(() => isComplete);

        Assert.IsTrue(isSuccess, "Login should be successful.");
    }

    /*
    [UnityTest]
    public IEnumerator TestCreateMeeting()
    {
        string testMeetingPassword = "Meeting123";
        string meetingID = System.Guid.NewGuid().ToString().Substring(0, 8);

        bool isSuccess = false;

        playFabManager.CreateMeeting(meetingID, testMeetingPassword);

        yield return new WaitForSeconds(2); // 等待执行完成
        isSuccess = true; // 如果没有抛出异常，认为成功

        Assert.IsTrue(isSuccess, "Meeting creation should be successful.");
    }*/
}
