using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class login : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject LoginMenu;

    public void OnLoginButtonClick()
    {
        MainMenu.SetActive(false);
        LoginMenu.SetActive(true);
    }
}
