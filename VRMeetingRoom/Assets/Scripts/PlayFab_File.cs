#if !DISABLE_PLAYFABENTITY_API && !DISABLE_PLAYFABCLIENT_API

using PlayFab;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class PlayFab_File : MonoBehaviour
{
    public string entityId; // Id representing the logged in player
    public string entityType; // entityType representing the logged in player
    private readonly Dictionary<string, string> _entityFileJson = new Dictionary<string, string>();
    private readonly Dictionary<string, byte[]> _entityFileData = new Dictionary<string, byte[]>();
    private readonly Dictionary<string, string> _tempUpdates = new Dictionary<string, string>();
    public string ActiveUploadFileName;
    public string NewFileName;
    public int GlobalFileLock = 0; // Kind of cheap and simple way to handle this kind of lock

    private string selectedFilePath; // Path to the selected file
    private byte[] selectedFileData; // Byte data of the selected file

    void OnSharedFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
        GlobalFileLock -= 1;
    }

    void OnGUI()
    {
        // 获取屏幕宽度和高度
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // 按钮尺寸
        float buttonWidth = 300f;
        float buttonHeight = 80f;
        float margin = 20f; // 按钮之间的间隔

        // 中心位置
        float centerX = screenWidth / 2f - buttonWidth / 2f;
        float startY = screenHeight / 2f - 350f; // Login 按钮起始位置（更靠近顶部）

        // 定义大号文字样式
        GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
        {
            fontSize = 24,
            alignment = TextAnchor.MiddleCenter
        };

        // Login 和 Logout 按钮
        if (!PlayFabClientAPI.IsClientLoggedIn() && GUI.Button(new Rect(centerX, startY, buttonWidth, buttonHeight), "Login", buttonStyle))
        {
            Login();
        }
        else if (PlayFabClientAPI.IsClientLoggedIn() && GUI.Button(new Rect(centerX, startY, buttonWidth, buttonHeight), "LogOut", buttonStyle))
        {
            PlayFabClientAPI.ForgetAllCredentials();
        }

        // Reload Files 按钮
        if (PlayFabClientAPI.IsClientLoggedIn() && GUI.Button(new Rect(centerX, startY + buttonHeight + margin, buttonWidth, buttonHeight), "(re)Load Files", buttonStyle))
        {
            LoadAllFiles();
        }

        // 文件显示和操作区域
        if (PlayFabClientAPI.IsClientLoggedIn())
        {
            _tempUpdates.Clear();

            float startX = margin; // 文件内容从屏幕左边开始
            float fileStartY = startY + 2 * (buttonHeight + margin) + 50; // 文件显示区起始Y位置
            float columnWidth = buttonWidth + margin; // 每列的宽度
            float rowHeight = buttonHeight + margin;  // 每行的高度

            // 根据屏幕宽度动态计算每行文件数量
            int filesPerRow = Mathf.FloorToInt((screenWidth - margin) / columnWidth);

            int index = 0;
            float totalFileHeight = fileStartY; // 用于计算文件列表的总高度

            foreach (var each in _entityFileJson)
            {
                // 动态计算每个文件的显示位置
                float offsetX = startX + (index % filesPerRow) * columnWidth; // 根据动态计算的列数排列
                float offsetY = fileStartY + (index / filesPerRow) * rowHeight * 4; // 每个文件占 4 行高度

                // 文件名标签
                GUI.Label(new Rect(offsetX, offsetY, buttonWidth, buttonHeight), each.Key, buttonStyle);

                // 文本框用于显示和编辑文件内容
                var tempInput = _entityFileJson[each.Key];
                var tempOutput = GUI.TextField(new Rect(offsetX, offsetY + rowHeight, buttonWidth, buttonHeight), tempInput);

                // 检查文本是否有更改
                if (tempInput != tempOutput)
                    _tempUpdates[each.Key] = tempOutput;

                // Delete 按钮
                if (GUI.Button(new Rect(offsetX, offsetY + 2 * rowHeight, buttonWidth, buttonHeight), "Delete " + each.Key, buttonStyle))
                {
                    DeleteFile(each.Key);
                    continue; // 删除后跳过本循环，避免显示不需要的内容
                }

                // Save to Desktop 按钮
                if (GUI.Button(new Rect(offsetX, offsetY + 3 * rowHeight, buttonWidth, buttonHeight), "Save to Desktop", buttonStyle))
                {
                    SaveFileToDesktop(each.Key);
                }

                index++;
                totalFileHeight = offsetY + 4 * rowHeight; // 记录文件列表的总高度
            }

            // 应用用户的更改
            foreach (var each in _tempUpdates)
            {
                _entityFileJson[each.Key] = each.Value;
            }

            // Select File 和 Upload 按钮居中显示
            float uploadSectionStartY = totalFileHeight + margin; // 紧跟文件列表之后

            if (GUI.Button(new Rect(centerX, uploadSectionStartY, buttonWidth, buttonHeight), "Select File to Upload", buttonStyle))
            {
                selectedFilePath = SelectLocalFile();
                if (!string.IsNullOrEmpty(selectedFilePath))
                {
                    NewFileName = Path.GetFileName(selectedFilePath);
                    selectedFileData = File.ReadAllBytes(selectedFilePath);
                }
            }

            GUI.Label(new Rect(centerX, uploadSectionStartY + buttonHeight + margin, buttonWidth, buttonHeight), selectedFilePath, buttonStyle);

            if (!string.IsNullOrEmpty(selectedFilePath) && GUI.Button(new Rect(centerX, uploadSectionStartY + 2 * (buttonHeight + margin), buttonWidth, buttonHeight), "Upload " + NewFileName, buttonStyle))
            {
                UploadFile(NewFileName, selectedFileData);
            }
        }
    }




    string SelectLocalFile()
    {
        // This would be replaced with a file picker library or platform-specific API.
        // On Unity Editor, you can use EditorUtility.OpenFilePanel, but it won't work in runtime builds.
#if UNITY_EDITOR
        return UnityEditor.EditorUtility.OpenFilePanel("Select File to Upload", "", "");
#else
        Debug.LogError("File selection is not implemented for this platform.");
        return null;
#endif
    }

    void Login()
    {
        var request = new PlayFab.ClientModels.LoginWithCustomIDRequest
        {
            CustomId = SystemInfo.deviceUniqueIdentifier, //根据设备唯一标识符创建ID
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLogin, OnSharedFailure);
    }

    void OnLogin(PlayFab.ClientModels.LoginResult result)
    {
        if (result.EntityToken != null)
        {
            entityId = result.EntityToken.Entity.Id;
            entityType = result.EntityToken.Entity.Type;
            Debug.Log($"Login successful! Entity ID: {entityId}");
        }
        else
        {
            Debug.LogError("EntityToken is null. Check PlayFab title settings.");
        }
    }

    void LoadAllFiles()
    {
        if (GlobalFileLock != 0)
        {
            Debug.LogWarning("File operation in progress. Please wait.");
            return; // 提示用户等待
        }

        GlobalFileLock += 1; // Start GetFiles
        var request = new PlayFab.DataModels.GetFilesRequest { Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType } };
        PlayFabDataAPI.GetFiles(request, OnGetFileMeta, OnSharedFailure);
    }

    void OnGetFileMeta(PlayFab.DataModels.GetFilesResponse result)
    {
        Debug.Log("Loading " + result.Metadata.Count + " files");

        _entityFileJson.Clear();
        foreach (var eachFilePair in result.Metadata)
        {
            _entityFileJson.Add(eachFilePair.Key, null);
            GetActualFile(eachFilePair.Value);
        }
        GlobalFileLock -= 1; // Finish GetFiles
    }

    void GetActualFile(PlayFab.DataModels.GetFileMetadata fileData)
    {
        GlobalFileLock += 1; // Start Each SimpleGetCall
        PlayFabHttp.SimpleGetCall(fileData.DownloadUrl,
            result =>
            {
                // 将文件内容转换为字符串，并存储到内存字典

                _entityFileData[fileData.FileName] = result;
                Debug.Log($"File loaded: {fileData.FileName}");

                GlobalFileLock -= 1; // Finish Each SimpleGetCall
            },
            error =>
            {
                Debug.LogError(error);
                GlobalFileLock -= 1; // Ensure lock is released on error
            }
        );
    }

    void SaveFileToDesktop(string fileName)
    {
        if (!_entityFileData.ContainsKey(fileName) || _entityFileData[fileName] == null)
        {
            Debug.LogError($"File content is empty or not loaded: {fileName}");
            return;
        }

        try
        {
            // 获取桌面路径
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string localPath = Path.Combine(desktopPath, fileName);

            // 直接保存字节数组到文件
            File.WriteAllBytes(localPath, _entityFileData[fileName]);
            Debug.Log($"File saved to desktop: {localPath}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Failed to save file to desktop: {ex.Message}");
        }
    }

    void DeleteFile(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
        {
            Debug.LogError("Invalid file name.");
            return;
        }

        if (GlobalFileLock != 0)
        {
            Debug.LogWarning("File operation in progress. Please wait.");
            return;
        }

        GlobalFileLock += 1; // Lock to prevent concurrent operations

        var request = new PlayFab.DataModels.DeleteFilesRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { fileName }
        };

        PlayFabDataAPI.DeleteFiles(request,
            result =>
            {
                Debug.Log($"File deleted successfully: {fileName}");
                _entityFileJson.Remove(fileName); // 从字典中移除该文件
                GlobalFileLock -= 1; // Release lock
            },
            error =>
            {
                Debug.LogError($"Failed to delete file: {fileName}, Error: {error.GenerateErrorReport()}");
                GlobalFileLock -= 1; // Release lock
            }
        );
    }


    void UploadFile(string fileName, byte[] fileData)
    {
        if (GlobalFileLock != 0)
        {
            Debug.LogWarning("File operation in progress. Please wait.");
            return;
        }

        ActiveUploadFileName = fileName;

        GlobalFileLock += 1; // Start InitiateFileUploads
        var request = new PlayFab.DataModels.InitiateFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        PlayFabDataAPI.InitiateFileUploads(request, response =>
        {
            PlayFabHttp.SimplePutCall(response.UploadDetails[0].UploadUrl,
                fileData,
                result =>
                {
                    FinalizeUpload();
                    GlobalFileLock -= 1; // release lock in success
                },
                error =>
                {
                    Debug.LogError($"SimplePutCall failed with error: {error}");
                    GlobalFileLock -= 1; // release lock in fail
                }
            );
            //GlobalFileLock -= 1; // Finish InitiateFileUploads
        }, OnInitFailed);
    }

    void OnInitFailed(PlayFabError error)
    {
        if (error.Error == PlayFabErrorCode.EntityFileOperationPending)
        {
            GlobalFileLock += 1; // Start AbortFileUploads
            var request = new PlayFab.DataModels.AbortFileUploadsRequest
            {
                Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
                FileNames = new List<string> { ActiveUploadFileName },
            };
            PlayFabDataAPI.AbortFileUploads(request, (result) => { GlobalFileLock -= 1; UploadFile(ActiveUploadFileName, selectedFileData); }, OnSharedFailure);
            GlobalFileLock -= 1; // Finish AbortFileUploads
        }
        else
            OnSharedFailure(error);
    }

    void FinalizeUpload()
    {
        GlobalFileLock += 1; // Start FinalizeFileUploads
        var request = new PlayFab.DataModels.FinalizeFileUploadsRequest
        {
            Entity = new PlayFab.DataModels.EntityKey { Id = entityId, Type = entityType },
            FileNames = new List<string> { ActiveUploadFileName },
        };
        PlayFabDataAPI.FinalizeFileUploads(request, result =>
        {
            Debug.Log("File upload success: " + ActiveUploadFileName);
            GlobalFileLock -= 1; // Finish FinalizeFileUploads
        }, OnSharedFailure);
    }
}
#endif