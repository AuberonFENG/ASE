using UnityEngine;
using UnityEngine.InputSystem.XR; // Needed to reference TrackedPoseDriver

public class DisableTrackedPoseDriver : MonoBehaviour
{
    [SerializeField] private GameObject leftController;
    void Start()
    {
        // Find the TrackedPoseDriver component on this GameObject
        TrackedPoseDriver trackedPoseDriver = GetComponent<TrackedPoseDriver>();

        // Check
        if (!leftController.activeInHierarchy)
        {
            if (trackedPoseDriver != null)
            {
                trackedPoseDriver.enabled = false;
            }
        }
    }
}
