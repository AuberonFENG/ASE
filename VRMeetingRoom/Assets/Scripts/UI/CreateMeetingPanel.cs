using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateMeetingPanel : MonoBehaviour
{
    public PlayFabCreateMeetingRequest createMeetingRequest;
    public TMP_InputField meetingPassword;
    public Button createMeetingButton;

    // Start is called before the first frame update
    void Start()
    {
        meetingPassword.text = "";
        createMeetingButton.onClick.AddListener(OnCreateMeetingClick);
    }

    private void OnCreateMeetingClick()
    {
        Debug.Log("meetingPassword: " + meetingPassword.text);
        if (meetingPassword.text == "")
        {
            Debug.Log("Error: meetingPassword is empty! ");
            return;
        }
        createMeetingRequest.GetMeetingInfo(meetingPassword.text);

        //
    }

}
