using System;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;
using XRMultiplayer;
using System.Linq;

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

        // 让 Host 发送 ClientRpc 给所有客户端，更新 UI
        XRINetworkPlayer hostPlayer = FindObjectsOfType<XRINetworkPlayer>().FirstOrDefault(p => p.IsHost);
        if (hostPlayer != null)
        {
            Debug.Log("1111111111");
            hostPlayer.UpdateMuteStatusClientRpc(!muted);
        }

        // 本地 UI 也更新
        playerListUI.MuteAllPlayer(!muted);
        muted = !muted;
    }


    
}
