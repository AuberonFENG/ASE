using UnityEngine;

public class TextureSwitcher : MonoBehaviour
{
    [Header("Assign Textures Here")]
    public Texture texture1; // First texture
    public Texture texture2; // Second texture

    private Renderer objectRenderer;
    private bool isTexture1Active = true; // Track which texture is active

    void Start()
    {
        // Get the Renderer component of the GameObject
        objectRenderer = GetComponent<Renderer>();

        // Initialize the texture to the first one, if assigned
        if (texture1 != null)
        {
            objectRenderer.material.mainTexture = texture1;
        }
    }

    void Update()
    {
        // Check if 'K' key is pressed
        if (Input.GetKeyDown(KeyCode.K))
        {
            // Switch the texture
            SwitchTexture();
        }
    }

    private void SwitchTexture()
    {
        if (isTexture1Active)
        {
            if (texture2 != null) // Ensure texture2 is assigned
            {
                objectRenderer.material.mainTexture = texture2;
                isTexture1Active = false;
            }
        }
        else
        {
            if (texture1 != null) // Ensure texture1 is assigned
            {
                objectRenderer.material.mainTexture = texture1;
                isTexture1Active = true;
            }
        }
    }
}
