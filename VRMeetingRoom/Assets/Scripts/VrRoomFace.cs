using SocketGameProtocol;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VrRoomFace : MonoBehaviour
{
    private ClientManager clientManager;
    private RequestManager requestManager;
    private static VrRoomFace face;

    public static VrRoomFace Face
    {
        get
        {
            if (face == null)
            {
                face = GameObject.Find("VrRoomFace").GetComponent<VrRoomFace>();
            }
            return face;
        }
        
    }
    void Awake()
    {
        clientManager = new ClientManager(this);
        requestManager = new RequestManager(this);
        requestManager.OnInit();
        clientManager.OnInit();
    }
    private void OnDestroy()
    {
        clientManager.OnDestroy();
        requestManager.OnDestroy();
    }
    public void HandleRespone(MainPack pack)
    {
        requestManager.HandleRequest(pack);
    }
    public void Send(MainPack pack)
    {
        clientManager.Send(pack);
    }

    public void AddRequest(BaseRequest request)
    {
        requestManager.AddRequest(request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestManager.RemoveRequest(action);
    }
    
}
