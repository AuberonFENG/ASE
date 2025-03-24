using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayFabCreateMeetingRequest : MonoBehaviour
{
    public PlayFabManager playerFabManager;
    public void GetMeetingInfo(string meetingPassword)
    {
        // 生成 meeting_id ---跳转到的scene关联
        string meetingID = Guid.NewGuid().ToString();
        playerFabManager.CreateMeeting(meetingID, meetingPassword);
    }
}
