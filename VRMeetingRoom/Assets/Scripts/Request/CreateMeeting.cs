using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CreateMeeting : MonoBehaviour
{
    public PlayFabManager playerFabManager;
    
    public void GetUserInfo(string password)
    {
        playerFabManager.CreateMeeting(password);
    }
}
