using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrRoomFace : MonoBehaviour
{
    private ClientManager clientManager;
    void Start()
    {
        clientManager = new ClientManager(this);
        clientManager.OnInit();
    }
    private void OnDestroy()
    {
        clientManager.OnDestroy();
    }
    public void HandleRespone(MainPack pack)
    {

    }
    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }
}
