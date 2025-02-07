using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayFabSignInRequest : MonoBehaviour
{
   public GameObject MainMenu;
   public PlayFabManager playerFabManager;
   public void GetUserInfo(string username,string password)
   {
      playerFabManager.RegisterPlayer(username,password);
      MainMenu.gameObject.SetActive(true);
      this.gameObject.SetActive(false);
   }
}
