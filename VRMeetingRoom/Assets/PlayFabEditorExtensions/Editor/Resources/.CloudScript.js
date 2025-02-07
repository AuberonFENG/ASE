handlers.createMeetingForUser = function (args, context) {
    var targetUserId = args.PlayFabId;
    var meetingData = {
        MeetingID: args.MeetingID,
        MeetingPassword: args.MeetingPassword
    };

    var updateRequest = server.UpdateUserData({
        PlayFabId: targetUserId,
        Data: meetingData,
        Permission: "Public"  // 或者 "Private"
    });

    return { success: true, meetingID: args.MeetingID };
};
