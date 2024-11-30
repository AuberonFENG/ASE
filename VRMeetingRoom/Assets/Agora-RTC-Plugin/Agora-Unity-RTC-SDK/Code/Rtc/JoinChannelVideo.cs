using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
 using UnityEngine.Android;
#endif

public class JoinChannelVideo : MonoBehaviour
{
    // 填入你的 app ID
    private string _appID = "5dbad5013ad24129a070b83a994f98f6";
    // 填入你的频道名
    private string _channelName = "channel";
    // 填入 Token
    private string _token = "007eJxTYLDwNkp23taop/nZMtFuljVLho2kg9zf6cHb/CetVp7WvUGBwTQlKTHF1MDQODHFyMTQyDLRwNwgycI40dLSJM3SIs1syX+v9IZARgbGikMsQBIMQXx2huSMxLy81BwGBgCnMR2w";
    internal VideoSurface LocalView;
    internal VideoSurface RemoteView;
    internal IRtcEngine RtcEngine;

#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif

    void Start()
    {
        SetupVideoSDKEngine();
        InitEventHandler();
        SetupUI();
        PreviewSelf();
    }

    void Update()
    {
        CheckPermissions();
    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            Leave();
            // 销毁 IRtcEngine
            RtcEngine.Dispose();
            RtcEngine = null;
        }
    }

    private void CheckPermissions()
    {
#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
            foreach (string permission in permissionList)
            {
                if (!Permission.HasUserAuthorizedPermission(permission))
                {
                    Permission.RequestUserPermission(permission);
                }
            }
#endif
    }

    private void PreviewSelf()
    {
        // 启用视频模块
        RtcEngine.EnableVideo();
        // 开启本地视频预览
        RtcEngine.StartPreview();
        // 设置本地视频显示
        LocalView.SetForUser(0, "");
        // 渲染视频
        LocalView.SetEnable(true);
    }

    private void SetupUI()
    {
        GameObject go = GameObject.Find("LocalView");
        LocalView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
        go = GameObject.Find("RemoteView");
        RemoteView = go.AddComponent<VideoSurface>();
        go.transform.Rotate(0.0f, 0.0f, -180.0f);
        go = GameObject.Find("Leave");
        go.GetComponent<Button>().onClick.AddListener(Leave);
        go = GameObject.Find("Join");
        go.GetComponent<Button>().onClick.AddListener(Join);
    }

    private void SetupVideoSDKEngine()
    {
        // 创建 IRtcEngine 实例
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
        context.appId = _appID;
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        // 初始化 IRtcEngine
        RtcEngine.Initialize(context);
    }

    // 创建用户回调类实例，并设置回调
    private void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

    public void Join()
    {
        // 设置频道媒体选项
        ChannelMediaOptions options = new ChannelMediaOptions();
        // 开始视频渲染
        LocalView.SetEnable(true);
        // 发布麦克风采集的音频流
        options.publishMicrophoneTrack.SetValue(true);
        // 发布摄像头采集的视频流
        options.publishCameraTrack.SetValue(true);
        // 自动订阅所有音频流
        options.autoSubscribeAudio.SetValue(true);
        // 自动订阅所有视频流
        options.autoSubscribeVideo.SetValue(true);
        // 将频道场景设为直播
        options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        // 将用户角色设为主播
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // 加入频道
        RtcEngine.JoinChannel(_token, _channelName, 0, options);
    }

    public void Leave()
    {
        Debug.Log("Leaving _channelName");
        // 关闭视频模块
        RtcEngine.StopPreview();
        // 离开频道
        RtcEngine.LeaveChannel();
        // 停止远端视频渲染
        RemoteView.SetEnable(false);
    }

    // 实现你自己的回调类，可以继承 IRtcEngineEventHandler 接口类实现
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly JoinChannelVideo _videoSample;

        internal UserEventHandler(JoinChannelVideo videoSample)
        {
            _videoSample = videoSample;
        }

        // 发生错误回调
        public override void OnError(int err, string msg)
        {
        }

        // 本地用户成功加入频道时，会触发该回调
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
        }

        // SDK 接收到第一帧远端视频并成功解码时，会触发 OnUserJoined 回调
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            // 设置远端视频显示
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // 开始视频渲染
            _videoSample.RemoteView.SetEnable(true);
            Debug.Log("Remote user joined");
        }

        // 远端用户离开当前频道时会触发该回调
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.RemoteView.SetEnable(false);
            Debug.Log("Remote user offline");
        }
    }
}
