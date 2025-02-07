using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LogInPanel : MonoBehaviour
{
    public PlayFabLogInRequest logInRequest;
    public TMP_InputField username, password;
    public Button logInButton;
    
    private void Start()
    {
        username.text = "";
        password.text = "";
        logInButton.onClick.AddListener(OnLoginClick);
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
