using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Button LoginBtn;
    public Button SignupBtn;
    public GameObject MainMenu;
    public GameObject LoginMenu;
    public GameObject SignupMenu;
    
    private void Start()
    {
        LoginBtn.onClick.AddListener(OnLoginButtonClick);
        SignupBtn.onClick.AddListener(OnSignupButtonClick);
    }

    void OnLoginButtonClick()
    {
        Debug.Log("OnLoginButtonClick");
        MainMenu.SetActive(false);
        LoginMenu.SetActive(true);
    }

    void OnSignupButtonClick()
    {
        MainMenu.SetActive(false);
        SignupMenu.SetActive(true);
    }
}
