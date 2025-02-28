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
    private bool muted = false;

    private void Start()
    {
        muteButton.onClick.AddListener(OnMuteAllChanged);
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            canvas.enabled = true;
        }
        else
        {
            canvas.enabled = false;
        }
    }
    
    private void OnMuteAllChanged()
    {
        foreach (var player in FindObjectsOfType<XRINetworkPlayer>())
        {
            player.RequestMuteChangeServerRpc(!muted); // 让 `ServerRpc` 触发
        }

        // 本地 UI 也更新
        playerListUI.MuteAllPlayer(!muted);
        muted = !muted;
    }

    
}
