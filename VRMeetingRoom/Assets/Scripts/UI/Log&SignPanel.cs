using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SigninPanel : MonoBehaviour
{
    public PlayFabSignInRequest signinRequest;
    public PlayFabLogInRequest logInRequest;
    public InputField username, password;
    public Button signinButton;
    public Button logInButton;
    
    private void Start()
    {
        username.text = "";
        password.text = "";
        signinButton.onClick.AddListener(OnSigninClick);
        logInButton.onClick.AddListener(OnLoginClick);
    }

    private void OnSigninClick()
    {
        Debug.Log(username.text + " " + password.text);
        if (username.text == "" || password.text == "")
        {
            Debug.Log("");
            return;
        }
        signinRequest.GetUserInfo(username.text, password.text);
    }

    private void OnLoginClick()
    {
        if (username.text == "" || password.text == "")
        {
            Debug.Log("");
            return;
        }
        logInRequest.GetUserInfo(username.text, password.text);
    }
    
}
