using System.Collections;
using System.Collections.Generic;
using SocketGameProtocol;
using UnityEngine;

public class SigninRequest : BaseRequest
{
    public override void Awake()
    {
        requestCode = RequestCode.User;
        actionCode = ActionCode.Signin;
        base.Awake();
    }

    public override void OnResponse(MainPack pack)
    {
        switch (pack.Returncode)
        {
            case ReturnCode.Succeed:
                Debug.Log("register successfully");
                break;
            case ReturnCode.Failed:
                Debug.Log("register failed");
                break;
        }
    }

    public void SendRequest(string username, string password)
    {
        MainPack pack = new MainPack();
        pack.Requestcode = requestCode;
        pack.Actioncode = actionCode;
        SigninPack signinPack = new SigninPack();
        signinPack.Username = username;
        signinPack.Password = password;
        pack.Signinpack = signinPack;
        base.SendRequest(pack);
    }
}
