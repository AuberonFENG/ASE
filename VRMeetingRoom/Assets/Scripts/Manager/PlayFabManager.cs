using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayFabManager : MonoBehaviour
{
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
}
