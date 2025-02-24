using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using XRMultiplayer;

public class MuteManager : MonoBehaviour
{
    public Canvas canvas;
    public Button muteButton;
    public PlayerListUI playerListUI;
    /*
    public void buttonShow()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            canvas.enabled = true;
            muteButton.onClick.AddListener(OnMuteAllChanged);
        }
        else
        {
            canvas.enabled = false;
        }
    }
    
    private void OnMuteAllChanged()
    {
        Debug.Log("111111");
        playerListUI.MuteAllPlayer();
    }
    */
   
}
