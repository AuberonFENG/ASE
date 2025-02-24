using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using XRMultiplayer;

public class MuteManager : MonoBehaviour
{
    public Button muteButton;
    public PlayerListUI playerListUI;
    private void Start()
    {
        muteButton.onClick.AddListener(OnMuteAllChanged);
    }
    private void OnMuteAllChanged()
    {
        playerListUI.MuteAllPlayer();
    }
    
   
}
