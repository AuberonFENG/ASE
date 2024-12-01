using Unity.Netcode;
using UnityEngine;

public class MonitorManager : NetworkBehaviour
{
    [SerializeField] private GameObject monitorPrefab; // Assign in Inspector

    private void Start()
    {
        // Subscribe to the connection callback
        NetworkManager.Singleton.OnClientConnectedCallback += OnClientConnected;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        // Unsubscribe to avoid memory leaks
        if (NetworkManager.Singleton != null)
        {
            NetworkManager.Singleton.OnClientConnectedCallback -= OnClientConnected;
        }
    }

    private void OnClientConnected(ulong clientId)
    {
        if (IsServer && clientId == 0)
        {
            SpawnMonitor(clientId);
        }
    }

    private void SpawnMonitor(ulong clientId)
    {
        GameObject monitor = Instantiate(monitorPrefab, GetSpawnPosition(clientId), Quaternion.identity);
        monitor.transform.localScale = new Vector3(0.5f,0.5f,0.5f);
        NetworkObject networkObject = monitor.GetComponent<NetworkObject>();
        networkObject.SpawnWithOwnership(clientId);
    }

    private Vector3 GetSpawnPosition(ulong clientId)
    {
        return new Vector3(clientId * 2.0f, 0, 0); // Example spawn position logic
    }
}
