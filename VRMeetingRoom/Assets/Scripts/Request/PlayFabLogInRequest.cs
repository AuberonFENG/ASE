using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayFabLogInRequest : MonoBehaviour
{
    public PlayFabManager playerFabManager; 
    public GameObject MeetingChoice;
    public void GetUserInfo(string username,string password)
    {
        playerFabManager.Login(username,password);
        SceneManager.LoadScene("SampleScene");

    }
}
