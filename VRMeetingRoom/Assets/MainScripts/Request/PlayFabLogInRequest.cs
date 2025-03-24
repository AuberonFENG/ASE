using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabLogInRequest : MonoBehaviour
{
    public PlayFabManager playerFabManager; 
    public GameObject MeetingChoice;
    public void GetUserInfo(string username,string password)
    {
        playerFabManager.Login(username,password);
        this.gameObject.SetActive(false);
        MeetingChoice.gameObject.SetActive(true);
    }
}
