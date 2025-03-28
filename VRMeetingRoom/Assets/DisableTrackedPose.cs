using UnityEngine;
using UnityEngine.InputSystem.XR; // Needed to reference TrackedPoseDriver

public class DisableTrackedPoseDriver : MonoBehaviour
{
    [SerializeField] private GameObject leftController;
    private TrackedPoseDriver trackedPoseDriver;
    void Start()
    {

        // Find the TrackedPoseDriver component on this GameObject
        trackedPoseDriver = GetComponent<TrackedPoseDriver>();
    }

    void Update()
    {

        // Check
        if (leftController.activeInHierarchy)
        {
            if (trackedPoseDriver != null)
            {
                trackedPoseDriver.enabled = true;
            }
        }
    }
}
