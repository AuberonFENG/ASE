using UnityEngine;
using Unity.Netcode;
using System.IO;
using UnityEngine.InputSystem;


public class MonitorTextureReplicator : NetworkBehaviour
{
    private Renderer monitorRenderer;

    private void Start()
    {
        monitorRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (IsOwner && Keyboard.current.mKey.wasPressedThisFrame)
        {
            Debug.LogError("Begin Sync Texture");
            ReplicateMaterial();
        }
    }

    private void ReplicateMaterial()
    {
        // Ensure the material's texture is readable
        var texture = monitorRenderer.material.mainTexture as Texture2D;
        if (texture == null)
        {
            Debug.LogError("Material texture is not a Texture2D.");
            return;
        }

        // Create a readable copy of the texture
        Texture2D readableTexture = new Texture2D(texture.width, texture.height, texture.format, false);
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture renderTexture = new RenderTexture(texture.width, texture.height, 24);
        Graphics.Blit(texture, renderTexture);
        RenderTexture.active = renderTexture;
        readableTexture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        readableTexture.Apply();
        RenderTexture.active = currentRT;

        // Convert texture to a byte array
        byte[] textureData = readableTexture.EncodeToPNG();

        // Send texture data to the server
        SendTextureToServerRpc(textureData);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendTextureToServerRpc(byte[] textureData, ServerRpcParams rpcParams = default)
    {
        // Broadcast texture data to all clients
        DistributeTextureToClientsClientRpc(textureData);
    }

    [ClientRpc]
    private void DistributeTextureToClientsClientRpc(byte[] textureData)
    {
        ApplyTexture(textureData);
    }

    private void ApplyTexture(byte[] textureData)
    {
        Texture2D texture = new Texture2D(2, 2);
        if (texture.LoadImage(textureData))
        {
            monitorRenderer.material.mainTexture = texture;
        }
        else
        {
            Debug.LogError("Failed to load texture from data.");
        }
    }
}
