using UnityEngine;
using Unity.Netcode;
using Unity.Collections;
using System.Collections;

public class TextureStreamer : NetworkBehaviour
{
    private Renderer monitorRenderer;
    public float frameRate = 10f; // Frames per second for texture updates

    private void Start()
    {
        monitorRenderer = GetComponent<Renderer>();
        NetworkManager.Singleton.CustomMessagingManager.OnUnnamedMessage += OnTextureReceived;

        if (IsOwner)
        {
            StartCoroutine(StreamTextures());
        }
    }

    private IEnumerator StreamTextures()
    {
        float frameInterval = 1f / frameRate;

        while (true)
        {
            PrepareAndSendTexture();
            yield return new WaitForSeconds(frameInterval);
        }
    }

    private void PrepareAndSendTexture()
    {
        var texture = monitorRenderer.material.mainTexture as Texture2D;
        if (texture == null || !texture.isReadable)
        {
            Debug.LogError("Material texture is not a readable Texture2D.");
            return;
        }

        byte[] textureData = ImageConversion.EncodeToJPG(texture, 75); // Adjust quality as needed
        SendTextureToAllClients(textureData);
    }

    private void SendTextureToAllClients(byte[] data)
    {
        if (data == null || data.Length == 0)
        {
            Debug.LogError("No texture data to send.");
            return;
        }

        var customMessagingManager = NetworkManager.Singleton.CustomMessagingManager;
        foreach (var clientId in NetworkManager.ConnectedClientsIds)
        {
            if (clientId == NetworkManager.LocalClientId) continue;
            SendTextureData(clientId, data);
        }
    }

    private void SendTextureData(ulong clientId, byte[] data)
    {
        using (var writer = new FastBufferWriter(data.Length, Allocator.Temp))
        {
            writer.WriteBytesSafe(data);
            NetworkManager.Singleton.CustomMessagingManager.SendUnnamedMessage(clientId, writer, NetworkDelivery.Unreliable);
        }
    }

    private void OnTextureReceived(ulong clientId, FastBufferReader reader)
    {
        int dataLength = reader.Length;
        byte[] textureData = new byte[dataLength];
        reader.ReadBytesSafe(ref textureData, dataLength);

        Texture2D receivedTexture = new Texture2D(2, 2);
        if (receivedTexture.LoadImage(textureData))
        {
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
        }
        else
        {
            Debug.LogError("No renderer found to apply the texture.");
        }
    }
}
