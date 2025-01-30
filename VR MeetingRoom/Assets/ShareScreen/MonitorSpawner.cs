using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System
using Unity.Netcode;

public class MonitorSpawner : NetworkBehaviour
{
    [SerializeField]
    private GameObject prefabToSpawn;

    [SerializeField]
    private RectTransform rectTransform; // RectTransform to take position, rotation, and scale from

    private void Update()
    {
        if (IsClient && Keyboard.current.tKey.wasPressedThisFrame) // New Input System API
        {
            RequestSpawnServerRpc();
        }
    }

    [ServerRpc]
    private void RequestSpawnServerRpc(ServerRpcParams rpcParams = default)
    {
        if (rectTransform == null)
        {
            Debug.LogError("RectTransform is not assigned!");
            return;
        }

        // Get position, rotation, and scale from RectTransform
        Vector3 spawnPosition = rectTransform.position;
        Quaternion spawnRotation = rectTransform.rotation;
        Vector3 spawnScale = rectTransform.localScale;

        Debug.Log($"Spawning prefab at Position: {spawnPosition}, Rotation: {spawnRotation.eulerAngles}, Scale: {spawnScale}");

        // Instantiate the prefab
        GameObject instance = Instantiate(prefabToSpawn, spawnPosition, spawnRotation);

        // Apply scale to the instantiated object
        instance.transform.localScale = spawnScale;

        // NetworkObject handling
        NetworkObject networkObject = instance.GetComponent<NetworkObject>();
        if (networkObject != null)
        {
            networkObject.Spawn();
        }
        else
        {
            Debug.LogError("Prefab does not have a NetworkObject component!");
        }
    }
}
