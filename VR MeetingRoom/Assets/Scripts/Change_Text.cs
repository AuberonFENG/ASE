using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // 导入 TextMeshPro 命名空间

public class Change_Text : MonoBehaviour
{
    
    public TextMeshProUGUI textDisplay; // 用于引用 TextMeshProUGUI 组件
    public string text1 = "Hello World!"; // 第一条文本
    public string text2 = "Welcome to Unity!"; // 第二条文本
    private bool isText1 = true; // 用于跟踪当前显示的文本

    void Start()
    {
        // 确保 TextMeshProUGUI 组件已经正确引用
        if (textDisplay == null)
            textDisplay = GetComponentInChildren<TextMeshProUGUI>();

        // 初始显示第一条文本
        if (textDisplay != null)
        {
            textDisplay.text = text1;
        }
        else
        {
            Debug.LogError("TextMeshProUGUI component not assigned or found!");
        }
    }

    public void SwitchText()
    {
        // 切换文本内容
        if (textDisplay != null)
        {
            if (isText1)
            {
                textDisplay.text = text2;
                isText1 = false;
            }
            else
            {
                textDisplay.text = text1;
                isText1 = true;
            }
        }
    }

    public void ShowFile_UI()
    {

    }

}
