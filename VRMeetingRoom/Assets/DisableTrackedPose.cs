using UnityEngine;
using UnityEngine.InputSystem.XR; // Needed to reference TrackedPoseDriver

public class DisableTrackedPoseDriver : MonoBehaviour
{
    void Start()
    {
        // Find the TrackedPoseDriver component on this GameObject
        TrackedPoseDriver trackedPoseDriver = GetComponent<TrackedPoseDriver>();

        // Check if it's running on Windows
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        {
            if (trackedPoseDriver != null)
            {
                Debug.Log("Disabling TrackedPoseDriver on Windows");
                trackedPoseDriver.enabled = false; //Disable it externally!
            }
        }
    }
}
