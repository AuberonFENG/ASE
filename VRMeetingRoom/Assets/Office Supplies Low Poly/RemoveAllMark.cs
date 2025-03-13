using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 需要新输入系统

public class RemoveAllMark : MonoBehaviour
{
    public RenderTexture originalTexture;  // 存储初始纹理
    private Renderer objectRenderer;

    [SerializeField] private InputActionReference m_ToggleMenuAction; // 监听 ToggleMenuAction

    void Awake()
    {
        objectRenderer = GetComponent<Renderer>();
        Debug.Log("Test Type: " + objectRenderer.material.HasProperty("_CamTexture"));
        // 存储初始纹理（确保对象有材质）
        if (objectRenderer != null && objectRenderer.material != null)
        {
            originalTexture = (RenderTexture)objectRenderer.material.mainTexture;
        }
        else
        {
            Debug.LogWarning("No valid material or texture found on this object!");
        }

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
        ResetTexture();
    }

    void ResetTexture()
    {
        if (objectRenderer != null && originalTexture != null)
        {
            Debug.Log("Test Type: " + objectRenderer.material.mainTexture.GetType().Name);
            objectRenderer.material.mainTexture = originalTexture;
            Debug.Log("Texture has been reset.");
        }
        else
        {
            Debug.LogWarning("Original texture is not set! Please assign a texture before using this component.");
        }
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
