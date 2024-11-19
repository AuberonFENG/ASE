using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SocketGameProtocol;

public class BaseRequest : MonoBehaviour
{
    protected RequestCode requestCode;
    protected ActionCode actionCode;
    protected VrRoomFace face;
    public ActionCode GetActionCode
    {
        get
        {
            return actionCode;
        }
    }
    
    public virtual void Awake()
    {
        face = VrRoomFace.Face;
    }

    public virtual void Start()
    {
        face.AddRequest(this);
    }

    public void OnDestroy()
    {
        face.RemoveRequest(actionCode);
    }

    public virtual void OnResponse(MainPack pack)
    {
        
    }

    public virtual void SendRequest(MainPack pack)
    {
        face.Send(pack);
    }
}
