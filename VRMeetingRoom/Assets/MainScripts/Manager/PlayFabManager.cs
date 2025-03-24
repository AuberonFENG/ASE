using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;

public class PlayFabManager : MonoBehaviour
{
    private string username;
    public GameObject LoginPanel;
    public void RegisterPlayer(string username, string password)
    {
        var RegisterRequest = new RegisterPlayFabUserRequest
        {
            Username = username,
            Password = password,
            RequireBothUsernameAndEmail = false,
        };
        PlayFabClientAPI.RegisterPlayFabUser(RegisterRequest,OnRegisterSuccess,OnRegisterFailure);
    }

    public void Login(string username, string password)
    {
        var LoginRequest = new LoginWithPlayFabRequest
        {
            Username = username,
            Password = password,
        };
        PlayFabClientAPI.LoginWithPlayFab(LoginRequest,OnLoginSuccess,OnLoginFailure);
    }

    public void CreateMeeting(string password)
    {
        string meetingID = System.Guid.NewGuid().ToString().Substring(0, 8);

        var request = new ExecuteCloudScriptRequest
        {
            FunctionName = "createMeetingForUser",
            FunctionParameter = new Dictionary<string, object>
            {
                { "PlayFabId", "2DB02A9D139018A" },
                { "MeetingID", meetingID },
                { "MeetingPassword", password }
            }
        };

        PlayFabClientAPI.ExecuteCloudScript(request,
            result => Debug.Log($"会议创建成功！ID: {meetingID}"),
            error => Debug.LogError("会议创建失败: " + error.ErrorMessage));
    }
    

    private void OnRegisterSuccess(RegisterPlayFabUserResult result)
    {
        Debug.Log("RegisterSuccess");
    }
    private void OnRegisterFailure(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.UsernameNotAvailable)
        {
            Debug.Log("Username not available");
        }
        else if(error.Error==PlayFabErrorCode.UserAlreadyAdded)
        {
            Debug.Log("Username already added");
        }
    }

    private void OnLoginSuccess(LoginResult result)
    {
        Debug.Log("LoginSuccess");
        
        LoginPanel.SetActive(false);
        
        SceneManager.LoadScene("Main"); 
        SceneManager.UnloadSceneAsync("UI1");
        
        // next step function
    }

    private void OnLoginFailure(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.InvalidUsernameOrPassword)
        {
            Debug.LogError("Invalid username or password");
        }
        else
        {
            Debug.LogError(error.GenerateErrorReport());
        }
    }
    private void GetUsername(string playFabId)
    {
        var request = new GetAccountInfoRequest
        {
            PlayFabId = playFabId
        };

        PlayFabClientAPI.GetAccountInfo(request, OnGetAccountInfoSuccess, OnGetAccountInfoFailure);
    }

    private void OnGetAccountInfoSuccess(GetAccountInfoResult result)
    {
        username = result.AccountInfo.Username;
        Debug.Log("Username: " + username);
    }

    private void OnGetAccountInfoFailure(PlayFabError error)
    {
        Debug.LogError("Failed to get account info: ");
    }
    
    public void CreateMeeting(string meetingID, string meetingPassword)
    {
        string memberID = username;
        var request = new CreateSharedGroupRequest
        {
            SharedGroupId = meetingID
        };

        PlayFabClientAPI.CreateSharedGroup(request, result =>
        {
            Debug.Log($"Meeting {meetingID} created with ID: {meetingID}");
            StoreMeetingData(meetingID, meetingPassword, memberID);
        }, error =>
        {
            Debug.LogError("Failed to create meeting: " + error.GenerateErrorReport());
        });
    }

    private void StoreMeetingData(string meetingID, string meetingPassword, string memberID)
    {
        var request = new UpdateSharedGroupDataRequest
        {
            SharedGroupId = meetingID,
            Data = new Dictionary<string, string>
            {
                { "Password", meetingPassword},
                { "HostMemberID", memberID},
                { memberID,  "[]"} // Empty members list
            }
        };

        PlayFabClientAPI.UpdateSharedGroupData(request, result =>
        {
            Debug.Log("Meeting info stored successfully!");
        }, error =>
        {
            Debug.LogError("Failed to store meeting info: " + error.GenerateErrorReport());
        });
    }
}
