using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 需要新输入系统

public class RemoveAllMark : MonoBehaviour
{
    private Renderer objectRenderer;
    public RenderTexture targetRenderTexture; // 要清除的 RenderTexture

    [SerializeField] private InputActionReference m_ToggleMenuAction; // 监听 ToggleMenuAction

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();

        // 监听 ToggleMenu 事件
        if (m_ToggleMenuAction != null)
        {
            m_ToggleMenuAction.action.performed += OnToggleMenu;
        }
        else
        {
            Debug.LogWarning("ToggleMenu action is not assigned in RemoveAllMark!");
        }
    }

    private void OnToggleMenu(InputAction.CallbackContext context)
    {
        Debug.Log("ToggleMenu action triggered! Resetting texture...");
        targetRenderTexture.Release();  // 释放 RenderTexture
        Debug.Log("finish reset");
    }

    void OnDestroy()
    {
        // 取消监听 ToggleMenu 事件，防止内存泄漏
        if (m_ToggleMenuAction != null)
        {
            m_ToggleMenuAction.action.performed -= OnToggleMenu;
        }
    }
}
