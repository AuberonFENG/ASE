using UnityEngine;

public class UIFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // 主相机
    public float distanceFromCamera = 1.0f; // UI 距离相机的距离

    public void Start()
    {
        if (mainCamera == null)
        {
            // 如果未指定相机，默认使用主相机
            mainCamera = Camera.main;
        }
    }

    public void Update()
    {
        // 将 Canvas 设置为相机前方的指定位置
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;

        // 使 Canvas 始终面向相机
        transform.rotation = Quaternion.LookRotation(transform.position - mainCamera.transform.position);
    }
}
