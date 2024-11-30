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
    // ������� app ID
    private string _appID = "5dbad5013ad24129a070b83a994f98f6";
    // �������Ƶ����
    private string _channelName = "channel";
    // ���� Token
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
            // ���� IRtcEngine
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
        // ������Ƶģ��
        RtcEngine.EnableVideo();
        // ����������ƵԤ��
        RtcEngine.StartPreview();
        // ���ñ�����Ƶ��ʾ
        LocalView.SetForUser(0, "");
        // ��Ⱦ��Ƶ
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
        // ���� IRtcEngine ʵ��
        RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngine();
        RtcEngineContext context = new RtcEngineContext();
        context.appId = _appID;
        context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
        context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
        // ��ʼ�� IRtcEngine
        RtcEngine.Initialize(context);
    }

    // �����û��ص���ʵ���������ûص�
    private void InitEventHandler()
    {
        UserEventHandler handler = new UserEventHandler(this);
        RtcEngine.InitEventHandler(handler);
    }

    public void Join()
    {
        // ����Ƶ��ý��ѡ��
        ChannelMediaOptions options = new ChannelMediaOptions();
        // ��ʼ��Ƶ��Ⱦ
        LocalView.SetEnable(true);
        // ������˷�ɼ�����Ƶ��
        options.publishMicrophoneTrack.SetValue(true);
        // ��������ͷ�ɼ�����Ƶ��
        options.publishCameraTrack.SetValue(true);
        // �Զ�����������Ƶ��
        options.autoSubscribeAudio.SetValue(true);
        // �Զ�����������Ƶ��
        options.autoSubscribeVideo.SetValue(true);
        // ��Ƶ��������Ϊֱ��
        options.channelProfile.SetValue(CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING);
        // ���û���ɫ��Ϊ����
        options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        // ����Ƶ��
        RtcEngine.JoinChannel(_token, _channelName, 0, options);
    }

    public void Leave()
    {
        Debug.Log("Leaving _channelName");
        // �ر���Ƶģ��
        RtcEngine.StopPreview();
        // �뿪Ƶ��
        RtcEngine.LeaveChannel();
        // ֹͣԶ����Ƶ��Ⱦ
        RemoteView.SetEnable(false);
    }

    // ʵ�����Լ��Ļص��࣬���Լ̳� IRtcEngineEventHandler �ӿ���ʵ��
    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly JoinChannelVideo _videoSample;

        internal UserEventHandler(JoinChannelVideo videoSample)
        {
            _videoSample = videoSample;
        }

        // ��������ص�
        public override void OnError(int err, string msg)
        {
        }

        // �����û��ɹ�����Ƶ��ʱ���ᴥ���ûص�
        public override void OnJoinChannelSuccess(RtcConnection connection, int elapsed)
        {
        }

        // SDK ���յ���һ֡Զ����Ƶ���ɹ�����ʱ���ᴥ�� OnUserJoined �ص�
        public override void OnUserJoined(RtcConnection connection, uint uid, int elapsed)
        {
            // ����Զ����Ƶ��ʾ
            _videoSample.RemoteView.SetForUser(uid, connection.channelId, VIDEO_SOURCE_TYPE.VIDEO_SOURCE_REMOTE);
            // ��ʼ��Ƶ��Ⱦ
            _videoSample.RemoteView.SetEnable(true);
            Debug.Log("Remote user joined");
        }

        // Զ���û��뿪��ǰƵ��ʱ�ᴥ���ûص�
        public override void OnUserOffline(RtcConnection connection, uint uid, USER_OFFLINE_REASON_TYPE reason)
        {
            _videoSample.RemoteView.SetEnable(false);
            Debug.Log("Remote user offline");
        }
    }
}
