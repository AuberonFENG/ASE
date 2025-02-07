using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateMeetingPanel : MonoBehaviour
{
    public CreateMeeting createMeetingRequest;
    public TMP_InputField password;
    public Button createMeetingButton;
    private void Start()
    {
        password.text = "";
        createMeetingButton.onClick.AddListener(OnCreateMeetingClick);
    }
    private void OnCreateMeetingClick()
    {
        if (password.text == "")
        {
            Debug.Log("");
            return;
        }
        createMeetingRequest.GetUserInfo(password.text);
    }
    
}
