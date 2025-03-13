using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // 需要新输入系统

public class RemoveAllMark : MonoBehaviour
{
    public Material originalMaterial;  // 由外部传入的初始材质
    private Renderer objectRenderer;

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
        Debug.Log("ToggleMenu action triggered! Resetting material...");
        ResetMaterial();
    }

    void ResetMaterial()
    {
        if (objectRenderer != null && originalMaterial != null)
        {
            
            /* here is wronggggggggggggggggggggg!*/
            // renderer defination?? 
            objectRenderer.material = originalMaterial;
            Debug.Log("Original Material Type: " + originalMaterial.GetType().Name);

            Debug.Log("Material has been reset.");
        }
        else
        {
            Debug.LogWarning("Original material is not set! Please call Initialize(Material material) before using this component.");
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
