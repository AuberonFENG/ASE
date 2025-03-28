using UnityEngine;
using Agora_RTC_Plugin.JoinChannelVideo;

public class ShareCamera : MonoBehaviour
{
    private JoinChannelVideo JoinChannelVideo_;
    public GameObject showLocalView, leaveChannel, joinChannel, changeCamera, changeCameraHost;

    // Start is called before the first frame update
    void Start()
    {
        JoinChannelVideo_ = GetComponent<JoinChannelVideo>(); // 获取挂在同一 GameObject 上的 Script
        if (JoinChannelVideo_ != null)
        {
            SetupUItoJoinChannelVideo();
            JoinChannelVideo_.Setup();
            
        }
        else
        {
            Debug.LogError("JoinChannelVideo_ not found!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        JoinChannelVideo_.CheckPermissions();
    }

    void OnApplicationQuit()
    {
        JoinChannelVideo_.Quit();
    }


    private void SetupUItoJoinChannelVideo()
    {
        JoinChannelVideo_.showLocalView = showLocalView;
        JoinChannelVideo_.leaveChannel = leaveChannel;
        JoinChannelVideo_.joinChannel = joinChannel;
        JoinChannelVideo_.changeCamera = changeCamera;
        JoinChannelVideo_.changeCameraHost = changeCameraHost;
    }
}
