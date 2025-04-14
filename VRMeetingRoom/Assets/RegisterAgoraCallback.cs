using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Agora_RTC_Plugin.ScreenShareV2;
using Agora_RTC_Plugin.JoinChannelVideo;
using UnityEngine.UI;
using Agora.Rtc;
using UnityEngine.Serialization;
using io.agora.rtc.demo;
using System;
using System.Linq;
using TMPro;

public class RegisterAgoraCallback : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        ScreenShareV2 _desktopScreenShare = GetComponent<ScreenShareV2>();
        JoinChannelVideo _videoSample = GetComponent<JoinChannelVideo>();
        UserEventHandler handler = new UserEventHandler(_desktopScreenShare, _videoSample);
        IRtcEngineEx RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngineEx();
        RtcEngine.InitEventHandler(handler);
        Debug.Log("RegisterAgoraCallback Start");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly ScreenShareV2 _desktopScreenShare;
        private readonly JoinChannelVideo _videoSample;

        internal UserEventHandler(ScreenShareV2 desktopScreenShare, JoinChannelVideo videoSample)
        {
            _desktopScreenShare = desktopScreenShare;
            _videoSample = videoSample;
        }

        public override void OnError(int err, string msg)
        {
            Debug.Log(string.Format("OnError err: {0}, msg: {1}", err, msg));
        }

        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            int build = 0;

            Debug.Log(string.Format("sdk version: ${0}",
                _desktopScreenShare.RtcEngine.GetVersion(ref build)));
            Debug.Log(string.Format("OnJoinChannelSuccess channelName: {0}, uid: {1}, elapsed: {2}",
                                connection.channelId, connection.localUid, elapsed));

        }

        public override void OnRejoinChannelSuccess(RtcConnection connection, int elapsed)
        {
            Debug.Log("OnRejoinChannelSuccess");
        }

        public override void OnLeaveChannel(RtcConnection connection, RtcStats stats)
        {
            ScreenShareV2.DestroyVideoView(connection.localUid);
            Debug.Log(string.Format("OnLeaveChannel: {0}", connection.localUid));

        }

        public override void OnClientRoleChanged(RtcConnection connection, CLIENT_ROLE_TYPE oldRole, CLIENT_ROLE_TYPE newRole, ClientRoleOptions newRoleOptions)
        {
            Debug.Log("OnClientRoleChanged");

        }

        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {

            Debug.Log(string.Format("OnUserJoined connection_uid: ${0}  uid: ${1} elapsed: ${2}", connection.localUid, uid, elapsed));

            // check if joined the screen share ex channel
            if (connection.localUid == _desktopScreenShare.localUidEx)
            {
                ScreenShareV2.MakeVideoView(uid, _desktopScreenShare.GetChannelName(), VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            }
        }

        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {

            Debug.Log(string.Format("OnUserOffLine connection_uid: ${0} uid: ${1}, reason: ${2}", connection.localUid, uid,
                (int)reason));

            // check if joined the screen share ex channel
            if (connection.localUid == _desktopScreenShare.localUidEx)
            {
                ScreenShareV2.DestroyVideoView(uid);
            }
            else
            {
                // 用户关闭摄像头，移除对应的远端视图
                if (_videoSample.remoteViews.TryGetValue(uid, out VideoSurface videoSurface))
                {
                    GameObject.Destroy(videoSurface.gameObject);
                    _videoSample.remoteViews.Remove(uid);
                    Debug.Log($"Agora: Remote user {uid} turned off their video.");
                }
            }
        }

        public override void OnUserMuteVideo(RtcConnection connection, uint uid, bool mute)
        {
            if (connection.localUid == _desktopScreenShare.localUidEx)
            {
                return;
            }
            if (mute == true)
            {
                // 用户关闭摄像头，移除对应的远端视图
                if (_videoSample.remoteViews.TryGetValue(uid, out VideoSurface videoSurface))
                {
                    GameObject.Destroy(videoSurface.gameObject);
                    _videoSample.remoteViews.Remove(uid);
                    Debug.Log($"Agora: Remote user {uid} turned off their video.");
                }
            }
            else
            {
                // 用户开启摄像头，创建远端视频视图
                if (!_videoSample.remoteViews.ContainsKey(uid))
                {
                    GameObject newRemoteView = new GameObject($"RemoteView_{uid}");
                    newRemoteView.transform.SetParent(GameObject.Find("RemoteContainer").transform, false);
                    newRemoteView.transform.Rotate(0.0f, 0.0f, -180.0f);

                    // 添加 RectTransform 以控制 UI 布局
                    RectTransform rectTransform = newRemoteView.AddComponent<RectTransform>();
                    rectTransform.sizeDelta = new Vector2(100, 100);
                    rectTransform.localScale = Vector3.one;

                    // 添加 RawImage 组件，作为视频渲染表面
                    RawImage rawImage = newRemoteView.AddComponent<RawImage>();

                    // 添加 VideoSurface 组件，绑定视频流
                    VideoSurface videoSurface = newRemoteView.AddComponent<VideoSurface>();
                    videoSurface.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
                    videoSurface.SetEnable(true);

                    // 将新视图存入字典
                    _videoSample.remoteViews.Add(uid, videoSurface);

                    Debug.Log($"Agora: Remote user {uid} turned on their video.");
                }
            }
        }

        // 远端用户离开当前频道时会触发该回调
        //public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        //{
        //    // 用户关闭摄像头，移除对应的远端视图
        //    if (_videoSample.remoteViews.TryGetValue(uid, out VideoSurface videoSurface))
        //    {
        //        GameObject.Destroy(videoSurface.gameObject);
        //        _videoSample.remoteViews.Remove(uid);
        //        Debug.Log($"Remote user {uid} turned off their video.");
        //    }
        //}
    }

}
