using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Collections;


public class ContinuousTextureStreamer : NetworkBehaviour
{
    private byte[] textureData; // Byte array to store texture data
    private Renderer monitorRenderer;
    public float frameRate = 30f; // Frames per second for texture updates (adjustable)

    private void Start()
    {
        monitorRenderer = GetComponent<Renderer>();
        NetworkManager.Singleton.CustomMessagingManager.OnUnnamedMessage += OnTextureReceived; // Register callback for custom messages

        if (IsOwner)
        {
            StartCoroutine(StreamTextures());
        }
    }

    private IEnumerator StreamTextures()
    {
        float frameInterval = 1f / frameRate; // Time interval between frames

        while (true)
        {
            PrepareAndSendTexture();
            yield return new WaitForSeconds(frameInterval); // Control the frame rate
        }
    }

    private void PrepareAndSendTexture()
    {
        if(!monitorRenderer.enabled)
            return;
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

        // Convert texture to byte array
        textureData = readableTexture.EncodeToPNG();
        // Debug.Log($"Streaming texture: {textureData.Length} bytes");

        // Send texture data to all connected clients
        SendTextureToAllClients();
    }

    private void SendTextureToAllClients()
    {
        if (textureData == null || textureData.Length == 0)
        {
            Debug.LogError("No texture data to send.");
            return;
        }

        var customMessagingManager = NetworkManager.Singleton.CustomMessagingManager;
        foreach (var clientId in NetworkManager.ConnectedClientsIds)
        {
            if (clientId == NetworkManager.LocalClientId) continue;

            SendTextureData(clientId, textureData);
        }
    }

    private void SendTextureData(ulong clientId, byte[] data)
    {
        using (var writer = new FastBufferWriter(data.Length, Allocator.Temp))
        {
            writer.WriteBytesSafe(data); // Write the texture data
            NetworkManager.Singleton.CustomMessagingManager.SendUnnamedMessage(clientId, writer, NetworkDelivery.ReliableFragmentedSequenced);
        }

        // Debug.Log($"Sent texture data of size {data.Length} bytes to client {clientId}");
    }

    private void OnTextureReceived(ulong clientId, FastBufferReader reader)
    {
        // Debug.Log($"Received texture data from client {clientId}");

        int dataLength = reader.Length;
        textureData = new byte[dataLength];
        reader.ReadBytesSafe(ref textureData, dataLength); // Read the texture data

        // Debug.Log($"Recreating texture from {dataLength} bytes");

        // Recreate the texture from the received byte array
        Texture2D receivedTexture = new Texture2D(2, 2);
        if (receivedTexture.LoadImage(textureData))
        {
            // Debug.Log($"Texture recreated successfully: {receivedTexture.width}x{receivedTexture.height}");
            ApplyTexture(receivedTexture);
        }
        else
        {
            Debug.LogError("Failed to load texture from received data.");
        }
    }

    private void ApplyTexture(Texture2D texture)
    {
        if (monitorRenderer != null)
        {
            monitorRenderer.material.mainTexture = texture;
            // Debug.Log("Applied received texture to material.");
        }
        else
        {
            Debug.LogError("No renderer found to apply the texture.");
        }
    }
}
