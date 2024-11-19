using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseManager
{
    protected VrRoomFace face;

    public BaseManager(VrRoomFace face)
    {
        this.face = face;
    }   

    public virtual void OnInit()
    {

    }
    public virtual void OnDestroy()
    {

    }
}
