using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using XRMultiplayer;

public class NetworkMaterial : MonoBehaviour
{
    [SerializeField] private NetworkedMaterial networkedMaterial; // Reference to the NetworkedMaterial script
    [SerializeField] private int materialIndex; // The material index to synchronize
    [SerializeField] private float syncInterval = 0.1f; // Time interval for synchronization

    private float lastSyncTime = 0f;

    private void Update()
    {
        // Check if it's time to sync
        if (Time.time - lastSyncTime >= syncInterval)
        {
            lastSyncTime = Time.time;

            if (networkedMaterial != null)
            {
                // Call to request synchronization
                networkedMaterial.RequestMaterialChange(materialIndex);
            }
        }
    }
}
