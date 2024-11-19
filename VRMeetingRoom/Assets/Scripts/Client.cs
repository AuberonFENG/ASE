using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net.Sockets;
using System.Net;
using UnityEditor.PackageManager;
using System;
using System.Text;


public class Client : MonoBehaviour
{
    private static Socket socket;
    private static byte[] buffer = new byte[1024];
    void Start()
    {
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        socket.Connect("127.0.0.1", 6666);
        StartReceive();
        Send();
    }

    void StartReceive()
    {
        socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
    }
    void ReceiveCallback(IAsyncResult ar)
    {
        int len = socket.EndReceive(ar);
        if (len == 0)
        {
            return;
        }
        string str = Encoding.UTF8.GetString(buffer, 0, len);
        StartReceive();
    }
    void Send()
    {
        socket.Send(Encoding.UTF8.GetBytes("hello"));
    }

    void Update()
    {
        
    }
}
