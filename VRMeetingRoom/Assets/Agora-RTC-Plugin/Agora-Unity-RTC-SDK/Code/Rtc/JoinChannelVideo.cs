using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;
using System;


//#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
// using UnityEngine.Android;
//#endif

public class JoinChannelVideo : MonoBehaviour
{
    [SerializeField] GameObject showLocalView, leaveChannel, joinChannel, changeCamera, changeCameraHost;
    // public TMP_Text CameraButtonText; // 相机按钮上的文本（TextMeshPro）
    private bool isActive = false; // 初始状态

    // 填入你的 app ID
    private string _appID = "5dbad5013ad24129a070b83a994f98f6";
    // 填入你的频道名
    private string _channelName = "test0221";
    // 填入 Token
    private string _token = "007eJxTYPh+pLaz2stw7qWosu0vw4LX7uE4qnOYwc9iwp2f6geTV/YpMJimJCWmmBoYGiemGJkYGlkmGpgbJFkYJ1pamqRZWqSZ2dTtSG8IZGT4y2XHyMgAgSA+B0NJanGJgZGRIQMDACxCIPU=";
    internal VideoSurface LocalView;

    //internal VideoSurface RemoteView;

    internal IRtcEngine RtcEngine;
    internal Dictionary<uint, VideoSurface> remoteViews = new Dictionary<uint, VideoSurface>();


#if (UNITY_2018_3_OR_NEWER && UNITY_ANDROID)
    private ArrayList permissionList = new ArrayList() { Permission.Camera, Permission.Microphone };
#endif

    void Start()
    {
        SetupVideoSDKEngine(); // 初始化 IRtcEngine
        InitEventHandler(); // 用户回调类实例，并设置回调
        SetupUI(); // 设置UI点击的函数
        // PreviewSelf(); // 启用视频模块 开启本地视频预览 渲染视频
    }

    void Update()
    {
        CheckPermissions();
    }

    void OnApplicationQuit()
    {
        if (RtcEngine != null)
        {
            LeaveChannel();
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

    //private void PreviewSelf()
    //{
    //    // 启用视频模块
    //    RtcEngine.EnableVideo();
    //    // 开启本地视频预览
    //    RtcEngine.StartPreview();
    //    // 设置本地视频显示
    //    LocalView.SetForUser(0, "");
    //    // 渲染视频
    //    LocalView.SetEnable(true);
    //}

    private void SetupUI()
    {
        //GameObject go = GameObject.Find("LocalView");
        //LocalView = go.AddComponent<VideoSurface>();
        LocalView = showLocalView.AddComponent<VideoSurface>();
        showLocalView.transform.Rotate(0.0f, 0.0f, -180.0f);

        //go = GameObject.Find("RemoteView");
        //RemoteView = go.AddComponent<VideoSurface>();
        //go.transform.Rotate(0.0f, 0.0f, -180.0f);leaveChannel, joinChannel, openCamera, closeCamera;

        //go = GameObject.Find("LeaveButton");
        leaveChannel.GetComponent<Button>().onClick.AddListener(LeaveChannel);
        //go = GameObject.Find("JoinButton");
        joinChannel.GetComponent<Button>().onClick.AddListener(JoinChannel);

        ////go = GameObject.Find("OpenCamera");
        //openCamera.GetComponent<Button>().onClick.AddListener(PublishVideo);
        ////go = GameObject.Find("CloseCamera");
        //closeCamera.GetComponent<Button>().onClick.AddListener(UnpublishVideo);

        changeCamera.GetComponent<Button>().onClick.AddListener(changeCameraState);

        changeCameraHost.GetComponent<Button>().onClick.AddListener(changeCameraStateHost);
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

    public void JoinChannel()
    {
        // 设置频道媒体选项
        ChannelMediaOptions options = new ChannelMediaOptions();
        // 不发布麦克风采集的音频流
        options.publishMicrophoneTrack.SetValue(false);
        // 不发布摄像头采集的视频流
        options.publishCameraTrack.SetValue(false);
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
        Debug.Log("Joined channel, but not publishing video.");

        // 启用视频模块
        RtcEngine.EnableVideo();
    }

    // 开启本地视频流
    public void PublishVideo()
    {
        //// 设置频道媒体选项
        //ChannelMediaOptions options = new ChannelMediaOptions();
        // 开始视频渲染
        // LocalView.SetEnable(true);
        //// 发布麦克风采集的音频流
        //options.publishMicrophoneTrack.SetValue(true);
        //// 发布摄像头采集的视频流
        //options.publishCameraTrack.SetValue(true);
        //// 自动订阅所有音频流
        //options.autoSubscribeAudio.SetValue(true);
        //// 自动订阅所有视频流
        //options.autoSubscribeVideo.SetValue(true);
        //// 将频道场景设为直播
        //options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        //// 将用户角色设为主播
        //options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        //// 加入频道
        //RtcEngine.JoinChannel(_token, _channelName, 0, options);

        // 创建新的 ChannelMediaOptions 实例
        ChannelMediaOptions newOptions = new ChannelMediaOptions();
        // 开始发布麦克风音频
        //newOptions.publishMicrophoneTrack.SetValue(true);
        // 开始发布摄像头视频
        newOptions.publishCameraTrack.SetValue(true);
        // 更新频道媒体选项
        RtcEngine.UpdateChannelMediaOptions(newOptions);
        Debug.Log("Updated channel media options: Now publishing audio and video.");


        Debug.Log("Publishing video stream.");

        // 发布本地采集的视频流
        RtcEngine.MuteLocalVideoStream(false);
        // 开启本地视频预览
        RtcEngine.StartPreview();
        // 设置本地视频显示
        LocalView.SetForUser(0, "");
        // 渲染视频
        LocalView.SetEnable(true);


    }

    // 关闭本地视频流
    public void UnpublishVideo()
    {
        Debug.Log("Unpublishing video stream.");
        // 关闭推送本地视频流
        RtcEngine.MuteLocalVideoStream(true);
        // 停止本地预览
        RtcEngine.StopPreview();
        // 停止渲染
        LocalView.SetEnable(false);
    }

    public void changeCameraState()
    {
        //if (CameraButtonText == null)
        //{
        //    // 自动获取按钮子对象的 TMP_Text 组件
        //    CameraButtonText = changeCamera.GetComponentInChildren<TMP_Text>();
        //}

        //// 测试：打印当前按钮文本
        //Debug.Log("按钮文本：" + CameraButtonText.text);

        //if (CameraButtonText.text == "Share Camera")
        //{
        //    PublishVideo();
        //}
        //else if (CameraButtonText.text == "Stop Sharing Camera")
        //{
        //    UnpublishVideo();
        //}
        isActive = !isActive; // 切换状态
        if(isActive == true)
        {
            PublishVideo();
        }
        else
        {
            UnpublishVideo();
        }
    }

    public void changeCameraStateHost()
    {
        //if (CameraButtonText == null)
        //{
        //    // 自动获取按钮子对象的 TMP_Text 组件
        //    CameraButtonText = changeCamera.GetComponentInChildren<TMP_Text>();
        //}

        //// 测试：打印当前按钮文本
        //Debug.Log("按钮文本：" + CameraButtonText.text);

        //if (CameraButtonText.text == "Share Camera")
        //{
        //    PublishVideo();
        //}
        //else if (CameraButtonText.text == "Stop Sharing Camera")
        //{
        //    UnpublishVideo();
        //}
        isActive = !isActive; // 切换状态
        if (isActive == true)
        {
            PublishVideo();
        }
        else
        {
            UnpublishVideo();
        }
    }
    // 离开频道
    public void LeaveChannel()
    {
        UnpublishVideo();
        Debug.Log("Leaving channel");
        RtcEngine.StopPreview();
        RtcEngine.LeaveChannel();
        RtcEngine.DisableVideo();
        // 移除所有远端视图
        foreach (var view in remoteViews.Values)
        {
            GameObject.Destroy(view.gameObject);
        }
        remoteViews.Clear();
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
        /*
        // SDK 接收到第一帧远端视频并成功解码时，会触发 OnUserJoined 回调
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            //// 设置远端视频显示
            //_videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            //// 开始视频渲染
            //_videoSample.RemoteView.SetEnable(true);
            //Debug.Log("Remote user joined");

            if (_videoSample.remoteViews.ContainsKey(uid))
                return;  // 防止重复创建
            // 创建远端视频显示的 RawImage
            GameObject newRemoteView = new GameObject($"RemoteView_{uid}");
            newRemoteView.transform.SetParent(GameObject.Find("RemoteContainer").transform);
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
            Debug.Log($"Remote user {uid} joined.");
        }*/

        // 远端用户视频状态变化时触发
        public override void OnUserMuteVideo(RtcConnection connection, uint uid, bool mute)
        {
            if (mute == true)
            {
                // 用户关闭摄像头，移除对应的远端视图
                if (_videoSample.remoteViews.TryGetValue(uid, out VideoSurface videoSurface))
                {
                    GameObject.Destroy(videoSurface.gameObject);
                    _videoSample.remoteViews.Remove(uid);
                    Debug.Log($"Remote user {uid} turned off their video.");
                }
            }
            else
            {
                // 用户开启摄像头，创建远端视频视图
                if (!_videoSample.remoteViews.ContainsKey(uid))
                {
                    GameObject newRemoteView = new GameObject($"RemoteView_{uid}");
                    newRemoteView.transform.SetParent(GameObject.Find("RemoteContainer").transform);
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

                    Debug.Log($"Remote user {uid} turned on their video.");
                }
            }
        }

        // 远端用户离开当前频道时会触发该回调
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            // 用户关闭摄像头，移除对应的远端视图
            if (_videoSample.remoteViews.TryGetValue(uid, out VideoSurface videoSurface))
            {
                GameObject.Destroy(videoSurface.gameObject);
                _videoSample.remoteViews.Remove(uid);
                Debug.Log($"Remote user {uid} turned off their video.");
            }
        }
    }
}
