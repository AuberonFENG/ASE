using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabSignInRequest : MonoBehaviour
{
   public PlayFabManager playerFabManager;
   public void GetUserInfo(string username,string password)
   {
      playerFabManager.RegisterPlayer(username,password);
   }
}
