using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI; // To handle Button interactions



public class ChangeText : MonoBehaviour
{
    public List<Button> buttons; // Reference to all buttons in the container
    public List<string> texts1;  // First set of texts
    public List<string> texts2;  // Second set of texts
    private List<bool> isText1 = new List<bool> { true, true, true, true}; // Tracks which set of texts is active


    void Start()
    {

        // Ensure the number of texts matches the buttons
        if (buttons.Count != texts1.Count || buttons.Count != texts2.Count)
        {
            Debug.LogError("The number of buttons and texts do not match!");
            return;
        }

        // Initialize texts
        //texts1 = new List<string> { "File", "Mute", "Share Screen", "Share Camera", "Chat", "Exit Meeting" }; // Initial button texts
        //texts2 = new List<string> { "File", "Unmute", "Stop Sharing Screen", "Stop Sharing Camera", "Chat", "Exit Meeting" }; // Text when clicked



        // Initialize button texts
        for (int i = 0; i < buttons.Count; i++)
        {
            TextMeshProUGUI textDisplay = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (textDisplay != null)
            {
                textDisplay.text = texts1[i]; // Set initial text
            }            else
            {
                Debug.LogError($"Button {i} does not have a TextMeshProUGUI component!");
            }

            // Add a click listener for each button (optional behavior per button)
            int index = i; // Capture the current index for use in the lambda
            buttons[i].onClick.AddListener(() => SwitchAllTexts(index));
        }
    }

    public void SwitchAllTexts(int index)
    {

        Debug.Log($"Buttons Count: {buttons.Count}, Texts1 Count: {texts1.Count}, Texts2 Count: {texts2.Count}");
        Debug.Log($"Button content:{buttons}");
        TextMeshProUGUI textDisplay = buttons[index].GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log($"Button text:{textDisplay.text}");
        textDisplay.text = isText1[index] ? texts2[index] : texts1[index];
        isText1[index] = !isText1[index];
        //// Toggle texts for all buttons
        //for (int i = 0; i < buttons.Count; i++)
        //{
        //    TextMeshProUGUI textDisplay = buttons[i].GetComponentInChildren<TextMeshProUGUI>();
        //    if (textDisplay != null)
        //    {
        //        textDisplay.text = isText1 ? texts2[i] : texts1[i];
        //    }
        //}
        //isText1 = !isText1; // Flip the state
    }

    //private void OnButtonClick(int index)
    //{
    //    // Example: Perform an action specific to the clicked button
    //    Debug.Log($"Button {index} clicked!");
    //    TextMeshProUGUI textDisplay = buttons[index].GetComponentInChildren<TextMeshProUGUI>();
    //    if (textDisplay != null)
    //    {
    //        textDisplay.text = isText1[index] ? texts2[index] : texts1[index];
    //    }
    //}
}
