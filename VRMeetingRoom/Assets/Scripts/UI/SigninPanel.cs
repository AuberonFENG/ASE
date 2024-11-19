using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SigninPanel : MonoBehaviour
{
    public SigninRequest signinRequest;
    public InputField username, password;
    public Button signinButton;

    private void Start()
    {
        signinButton.onClick.AddListener(OnSigninClick);
    }

    private void OnSigninClick()
    {
        if (username.text == "" || password.text == "")
        {
            Debug.Log("");
            return;
        }
        signinRequest.SendRequest(username.text, password.text);
    }
}
