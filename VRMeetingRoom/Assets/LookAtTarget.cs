using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    public Transform target; // 目标物体
    public bool lockYAxis = false; // 是否锁定 Y 轴（防止上下倾斜）
    void Start()
    {
        if (target == null)
        {
            GameObject leftController = GameObject.Find("Left Controller"); // 按名称查找
            if (leftController != null)
            {
                target = leftController.transform;
            }
            else
            {
                Debug.LogWarning("找不到 Left Controller！");
            }
        }
    }
    void Update()
    {
        if (target == null)
        {
            Debug.LogWarning("目标未设置！");
            return;
        }

        Vector3 direction = target.position - transform.position;

        if (lockYAxis)
        {
            direction.y = 0; // 锁定 Y 轴，防止上下倾斜
        }

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}

