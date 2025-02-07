using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SignUpPanel : MonoBehaviour
{
    public PlayFabSignInRequest SignUpRequest;
    public Button SignUpButton;
    public TMP_InputField username, password, doublePassword;
    
    private void Start()
    {
        username.text = "";
        password.text = "";
        doublePassword.text = "";
        SignUpButton.onClick.AddListener(OnSignUpClick);
    }
    private void OnSignUpClick()
    {
        Debug.Log(username.text + " " + password.text);
        if (username.text == "" || password.text == "")
        {
            Debug.Log("密码为空");
            return;
        }

        if (password.text != doublePassword.text)
        {
            Debug.Log("密码不一致");
            return;
        }
        SignUpRequest.GetUserInfo(username.text, password.text);
    }
}
