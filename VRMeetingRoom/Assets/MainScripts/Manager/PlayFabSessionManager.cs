// PlayFabSessionManager.cs
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

public class PlayFabSessionManager : MonoBehaviour
{
    public static PlayFabSessionManager Instance;

    public string EntityId { get; private set; }
    public string EntityType { get; private set; }
    public string Username { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // ±£³Ö¿ç³¡¾°
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetLoginResult(EntityTokenResponse token, string username)
    {
        EntityId = token.Entity.Id;
        EntityType = token.Entity.Type;
        Username = username;

        Debug.Log("Saved EntityId: " + EntityId + " | Username: " + username);
    }
}
