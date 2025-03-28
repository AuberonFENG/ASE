using System;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Agora.Rtc;
using UnityEngine.Serialization;
using io.agora.rtc.demo;
using TMPro;

namespace Agora_RTC_Plugin.ScreenShareV2
{

    public class ScreenShareV2 : MonoBehaviour
    {
        //button text listener
        public Button button;
        public Button button2;
        public GameObject VideoCanvas;

        [FormerlySerializedAs("appIdInput")]
        [SerializeField]
        private AppIdInput _appIdInput;

        [Header("_____________Basic Configuration_____________")]
        [FormerlySerializedAs("APP_ID")]
        [SerializeField]
        private string _appID = "";

        [FormerlySerializedAs("TOKEN")]
        [SerializeField]
        private string _token = "";

        [FormerlySerializedAs("CHANNEL_NAME")]
        [SerializeField]
        private string _channelName = "";

        private string _channelNameScreen = "";

        public Text LogText;
        public Logger Log;
        internal IRtcEngineEx RtcEngine = null;
        private ScreenCaptureSourceInfo[] _screenCaptureSourceInfos;

        public Dropdown WinIdSelect;
        public RawImage IconImage;
        public RawImage ThumbImage;

        private Rect _originThumRect = new Rect(0, 0, 500, 260);
        private Rect _originIconRect = new Rect(0, 0, 50, 50);

        public uint localUid;
        public uint localUidEx = 111;

        private bool isSharing = false;

        // Use this for initialization
        private void Start()
        {

            LoadAssetData();
            if (true)
            {
                InitEngine();
                SetBasicConfiguration();
#if UNITY_ANDROID || UNITY_IPHONE
                WinIdSelect.gameObject.SetActive(false);
                IconImage.gameObject.SetActive(false);
                ThumbImage.gameObject.SetActive(false);
#else

#endif
            }
            button.onClick.AddListener(ExecuteFunction);
            button2.onClick.AddListener(ExecuteFunction);
        }

        void ExecuteFunction()
        {
            if (isSharing == false)
            {

                PrepareScreenCapture();
                VideoCanvas.SetActive(true);
                isSharing = true;
            }
            else if (isSharing == true)
            {
                StopShare();
                VideoCanvas.SetActive(false);
                isSharing = false;
            }
        }

        //Show data in AgoraBasicProfile
        [ContextMenu("ShowAgoraBasicProfileData")]
        private void LoadAssetData()
        {
            if (_appIdInput == null) return;
            _appID = _appIdInput.appID;
            _token = _appIdInput.token;
            _channelName = _appIdInput.channelName;
            _channelNameScreen = _channelName;

            //TODO uid ex should base on the callback from joinChannel, instead of randomly generating one.
            System.Random random = new System.Random();
            localUidEx = (uint)random.Next(1, int.MaxValue); // Ensure min is at least 1
        }

        private void InitEngine()
        {
            RtcEngine = Agora.Rtc.RtcEngine.CreateAgoraRtcEngineEx();
            UserEventHandler handler = new UserEventHandler(this);
            RtcEngineContext context = new RtcEngineContext();
            context.appId = _appID;
            context.channelProfile = CHANNEL_PROFILE_TYPE.CHANNEL_PROFILE_LIVE_BROADCASTING;
            context.audioScenario = AUDIO_SCENARIO_TYPE.AUDIO_SCENARIO_DEFAULT;
            context.areaCode = AREA_CODE.AREA_CODE_GLOB;
            RtcEngine.Initialize(context);
            //RtcEngine.InitEventHandler(handler);
        }

        private void SetBasicConfiguration()
        {
            RtcEngine.EnableAudio();
            RtcEngine.EnableVideo();
            RtcEngine.SetClientRole(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
        }

        #region -- Button Events ---

        public void JoinChannel()
        {
            //RtcEngine.JoinChannel(_token, _channelName, "", 0);

            // Join an ex channel
            ChannelMediaOptions options = new ChannelMediaOptions();
            options.autoSubscribeAudio.SetValue(false);
            options.autoSubscribeVideo.SetValue(true);
            options.publishCameraTrack.SetValue(false);
            options.publishScreenTrack.SetValue(false);
            options.enableAudioRecordingOrPlayout.SetValue(false);
#if UNITY_ANDROID || UNITY_IPHONE
            options.publishScreenCaptureAudio.SetValue(false);
            options.publishScreenCaptureVideo.SetValue(false);
#endif
            options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            var ret = RtcEngine.JoinChannelEx(_token, new RtcConnection(_channelNameScreen, localUidEx), options);
            Debug.Log("JoinChannel returns: " + ret);
        }

        public void LeaveChannel()
        {
            //TODO change to leavechannelex
            RtcEngine.LeaveChannel();
        }


        public void OnWinSelectChanged()
        {
            ShowThumbnail();
            ShowIcon();
        }

        public void PrepareScreenCapture()
        {
            if (WinIdSelect == null || RtcEngine == null) return;

            WinIdSelect.ClearOptions();

            SIZE t = new SIZE();
            t.width = 1280;
            t.height = 720;
            SIZE s = new SIZE();
            s.width = 640;
            s.height = 640;
            _screenCaptureSourceInfos = RtcEngine.GetScreenCaptureSources(t, s, true);

            WinIdSelect.AddOptions(_screenCaptureSourceInfos.Select(w =>
                    new Dropdown.OptionData(
                        string.Format("{0}: {1}-{2} | {3}", w.type, w.sourceName, w.sourceTitle, w.sourceId)))
                .ToList());
            OnWinSelectChanged();
        }

        public void StopShare()
        {
            DestroyVideoView(0);
            RtcEngine.StopScreenCapture();
            // Join a channel and push the screen sharing stream
            ChannelMediaOptions options = new ChannelMediaOptions();
            options.autoSubscribeAudio.SetValue(false);
            options.autoSubscribeVideo.SetValue(true);
            options.publishCameraTrack.SetValue(false);
            options.publishScreenTrack.SetValue(true);
            options.enableAudioRecordingOrPlayout.SetValue(false);
#if UNITY_ANDROID || UNITY_IPHONE
            options.publishScreenCaptureAudio.SetValue(true);
            options.publishScreenCaptureVideo.SetValue(true);
#endif
            options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            var ret = RtcEngine.UpdateChannelMediaOptionsEx(options, new RtcConnection(_channelNameScreen, localUidEx));
            Debug.Log("UpdateChannelMediaOptions returns: " + ret);
        }



        public void OnConfirmButtonClicked()
        {
            StartShare();
            Publish();
        }


        private void ShowThumbnail()
        {
            if (ThumbImage.texture)
            {
                GameObject.Destroy(ThumbImage.texture);
                ThumbImage.texture = null;
            }
            ThumbImageBuffer thumbImageBuffer = _screenCaptureSourceInfos[WinIdSelect.value].thumbImage;
            if (thumbImageBuffer.buffer.Length == 0) return;
            Texture2D texture = null;
#if UNITY_STANDALONE_OSX
            texture = new Texture2D((int)thumbImageBuffer.width, (int)thumbImageBuffer.height, TextureFormat.RGBA32, false);
#elif UNITY_STANDALONE_WIN
            texture = new Texture2D((int)thumbImageBuffer.width, (int)thumbImageBuffer.height, TextureFormat.BGRA32, false);
#endif
            texture.LoadRawTextureData(thumbImageBuffer.buffer);
            texture.Apply();
            ThumbImage.texture = texture;

            float scale = Math.Min((float)_originThumRect.width / (float)thumbImageBuffer.width, (float)_originThumRect.height / (float)thumbImageBuffer.height);
            ThumbImage.rectTransform.sizeDelta = new Vector2(thumbImageBuffer.width * scale, thumbImageBuffer.height * scale);
        }

        private void ShowIcon()
        {
            if (IconImage.texture)
            {
                GameObject.Destroy(IconImage.texture);
                IconImage.texture = null;
            }
            ThumbImageBuffer iconImageBuffer = _screenCaptureSourceInfos[WinIdSelect.value].iconImage;
            if (iconImageBuffer.buffer.Length == 0) return;
            Texture2D texture = null;
#if UNITY_STANDALONE_OSX
            texture = new Texture2D((int)iconImageBuffer.width, (int)iconImageBuffer.height, TextureFormat.RGBA32, false);
#elif UNITY_STANDALONE_WIN
            texture = new Texture2D((int)iconImageBuffer.width, (int)iconImageBuffer.height, TextureFormat.BGRA32, false);
#endif
            texture.LoadRawTextureData(iconImageBuffer.buffer);
            texture.Apply();
            IconImage.texture = texture;


            float scale = Math.Min((float)_originIconRect.width / (float)iconImageBuffer.width, (float)_originIconRect.height / (float)iconImageBuffer.height);
            IconImage.rectTransform.sizeDelta = new Vector2(iconImageBuffer.width * scale, iconImageBuffer.height * scale);

        }

        private void StartShare()
        {
            if (RtcEngine == null) return;


#if UNITY_ANDROID || UNITY_IPHONE
            var parameters2 = new ScreenCaptureParameters2();
            parameters2.captureAudio = true;
            parameters2.captureVideo = true;
            var nRet = RtcEngine.StartScreenCapture(parameters2);
            this.Log.UpdateLog("StartScreenCapture :" + nRet);
#else
            RtcEngine.StopScreenCapture();
            if (WinIdSelect == null) return;
            var option = WinIdSelect.options[WinIdSelect.value].text;
            if (string.IsNullOrEmpty(option)) return;

            if (option.Contains("ScreenCaptureSourceType_Window"))
            {
                var windowId = option.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1];
                //Log.UpdateLog(string.Format(">>>>> Start sharing {0}", windowId));
                var nRet = RtcEngine.StartScreenCaptureByWindowId(ulong.Parse(windowId), default(Rectangle),
                        default(ScreenCaptureParameters));
                //this.Log.UpdateLog("StartScreenCaptureByWindowId:" + nRet);
            }
            else
            {
                var dispId = uint.Parse(option.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries)[1]);
                //Log.UpdateLog(string.Format(">>>>> Start sharing display {0}", dispId));
                var nRet = RtcEngine.StartScreenCaptureByDisplayId(dispId, default(Rectangle),
                    new ScreenCaptureParameters { captureMouseCursor = true, frameRate = 30 });
                //this.Log.UpdateLog("StartScreenCaptureByDisplayId:" + nRet);
            }

#endif
            ScreenShareV2.MakeVideoView(0, "", VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN);
        }

        private void Publish()
        {
            // Join a channel and push the screen sharing stream
            ChannelMediaOptions options = new ChannelMediaOptions();
            options.autoSubscribeAudio.SetValue(false);
            options.autoSubscribeVideo.SetValue(true);
            options.publishCameraTrack.SetValue(false);
            options.publishScreenTrack.SetValue(true);
            options.enableAudioRecordingOrPlayout.SetValue(false);
#if UNITY_ANDROID || UNITY_IPHONE
            options.publishScreenCaptureAudio.SetValue(true);
            options.publishScreenCaptureVideo.SetValue(true);
#endif
            options.clientRoleType.SetValue(CLIENT_ROLE_TYPE.CLIENT_ROLE_BROADCASTER);
            RtcEngine.UpdateChannelMediaOptionsEx(options, new RtcConnection(_channelNameScreen, localUidEx));

        }

        #endregion

        private void OnDestroy()
        {
            Debug.Log("OnDestroy");
            if (RtcEngine == null) return;
            RtcEngine.InitEventHandler(null);
            RtcEngine.LeaveChannel();
            RtcEngine.Dispose();
        }

        internal string GetChannelName()
        {
            return _channelName;
        }

        #region -- Video Render UI Logic ---

        internal static void MakeVideoView(uint uid, string channelId = "", VIDEO_SOURCE_TYPE videoSourceType = VIDEO_SOURCE_TYPE.VIDEO_SOURCE_CAMERA)
        {
            var go = GameObject.Find(uid.ToString());
            if (!ReferenceEquals(go, null))
            {
                return; // reuse
            }

            // create a GameObject and assign to this new user
            var videoSurface = MakePlaneSurface(uid.ToString());
            if (ReferenceEquals(videoSurface, null)) return;
            // configure videoSurface
            videoSurface.SetForUser(uid, channelId, videoSourceType);
            videoSurface.SetEnable(true);

            videoSurface.OnTextureSizeModify += (int width, int height) =>
            {
                var transform = videoSurface.GetComponent<RectTransform>();
                if (transform)
                {
                    //If render in RawImage. just set rawImage size.
                    transform.sizeDelta = new Vector2(width / 2, height / 2);
                    transform.localScale = videoSourceType == VIDEO_SOURCE_TYPE.VIDEO_SOURCE_SCREEN ? new Vector3(-1, 1, 1) : Vector3.one;
                }
                else
                {
                    //If render in MeshRenderer, just set localSize with MeshRenderer
                    float scale = (float)height / (float)width;
                    videoSurface.transform.localScale = new Vector3(-1, 1, scale);
                }
                Debug.Log("OnTextureSizeModify: " + width + "  " + height);
            };
        }

        // VIDEO TYPE 1: 3D Object
        private static VideoSurface MakePlaneSurface(string goName)
        {
            var go = GameObject.CreatePrimitive(PrimitiveType.Plane);

            if (go == null)
            {
                return null;
            }

            go.name = goName;
            var mesh = go.GetComponent<MeshRenderer>();
            if (mesh != null)
            {
                Debug.LogWarning("VideoSureface update shader");
                mesh.material = new Material(Shader.Find("Unlit/Texture"));
            }
            // set up transform
            go.transform.rotation = Quaternion.Euler(270.0f, 90.0f, 0.0f);
            go.transform.position = new Vector3(7.39f, 3.24f, 0.76f);
            go.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

            // configure videoSurface
            var videoSurface = go.AddComponent<VideoSurface>();
            return videoSurface;
        }

        // Video TYPE 2: RawImage
        private static VideoSurface MakeImageSurface(string goName)
        {
            var go = new GameObject();

            if (go == null)
            {
                return null;
            }

            go.name = goName;
            // to be renderered onto
            go.AddComponent<RawImage>();
            // make the object draggable
            //go.AddComponent<UIElementDrag>();
            var canvas = GameObject.Find("VideoCanvas");
            if (canvas != null)
            {
                go.transform.parent = canvas.transform;
                Debug.Log("add video view");
            }
            else
            {
                Debug.Log("Canvas is null video view");
            }

            // set up transform
            go.transform.Rotate(0f, 0.0f, 180.0f);
            go.transform.localPosition = Vector3.zero;
            go.transform.localScale = new Vector3(3f, 4f, 1f);

            // configure videoSurface
            var videoSurface = go.AddComponent<VideoSurface>();
            return videoSurface;
        }

        internal static void DestroyVideoView(uint uid)
        {
            var go = GameObject.Find(uid.ToString());
            if (!ReferenceEquals(go, null))
            {
                Destroy(go);
            }
        }

        #endregion
    }

    #region -- Agora Event ---

    internal class UserEventHandler : IRtcEngineEventHandler
    {
        private readonly ScreenShareV2 _desktopScreenShare;

        internal UserEventHandler(ScreenShareV2 desktopScreenShare)
        {
            _desktopScreenShare = desktopScreenShare;
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
        }
    }

    #endregion
}