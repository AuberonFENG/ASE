using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingChoicePanel : MonoBehaviour
{
    public GameObject createMeetingPanel;
    public GameObject meetingChoicePanel;
    public Button createMeetingBtn;

    void Start()
    {
        createMeetingBtn.onClick.AddListener(OnClickCreateMeeting);
    }

    void OnClickCreateMeeting()
    {
        createMeetingPanel.SetActive(true);
        meetingChoicePanel.SetActive(false);
    }
    
}
