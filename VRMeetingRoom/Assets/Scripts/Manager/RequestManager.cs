using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class RequestManager : BaseManager
{
    public RequestManager(VrRoomFace face) : base(face){}
    
    private Dictionary<ActionCode,BaseRequest> requestDict=new Dictionary<ActionCode,BaseRequest>();

    public void AddRequest(BaseRequest request)
    {
        requestDict.Add(request.GetActionCode, request);
    }

    public void RemoveRequest(ActionCode action)
    {
        requestDict.Remove(action);
    }

    public void HandleRequest(MainPack pack)
    {
        if (requestDict.TryGetValue(pack.Actioncode, out BaseRequest request))
        {
            request.OnResponse(pack);
        }
        else
        {
            
        }
    }
    
}
