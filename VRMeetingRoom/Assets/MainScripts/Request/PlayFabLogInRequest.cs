using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogInRequest : MonoBehaviour
{
    public PlayFabManager playerFabManager; 
    public void GetUserInfo(string username,string password)
    {
        playerFabManager.Login(username,password);
        this.gameObject.SetActive(false);
        SceneManager.LoadScene("Main");
        SceneManager.UnloadSceneAsync("UI1");
    }
}
