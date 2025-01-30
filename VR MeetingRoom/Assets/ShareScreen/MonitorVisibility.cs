using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;


public class MonitorVisibility : NetworkBehaviour
{
    private Renderer monitorRenderer; // Reference to the monitor's renderer

    // NetworkVariable to synchronize visibility across the network
    private NetworkVariable<bool> isVisible = new NetworkVariable<bool>(
        true, // Default value
        NetworkVariableReadPermission.Everyone, // Everyone can read
        NetworkVariableWritePermission.Owner    // Only the owner can write
    );

    private void Awake()
    {
        monitorRenderer = GetComponent<Renderer>(); // Get the renderer component
    }

    private void Start()
    {
        // Update visibility when the value changes
        isVisible.OnValueChanged += OnVisibilityChanged;

        // Apply initial visibility state
        UpdateVisibility(isVisible.Value);
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        // Unsubscribe from the event
        isVisible.OnValueChanged -= OnVisibilityChanged;
    }

    private void Update()
    {
        // Allow the owner to toggle visibility with the "T" key
        if (IsOwner && Keyboard.current.tKey.wasPressedThisFrame)
        {
            ToggleVisibility();
        }
    }

    private void ToggleVisibility()
    {
        if (IsOwner) // Ensure only the owner can toggle visibility
        {
            isVisible.Value = !isVisible.Value; // Toggle the NetworkVariable value
        }
    }

    private void OnVisibilityChanged(bool oldValue, bool newValue)
    {
        UpdateVisibility(newValue); // Update the visibility state
    }

    private void UpdateVisibility(bool visible)
    {
        if (monitorRenderer != null)
        {
            monitorRenderer.enabled = visible; // Enable or disable the monitor's renderer
        }
    }
}
