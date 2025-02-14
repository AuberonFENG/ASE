using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using TMPro;

public class Host_Desktop_UI_ChangeText_Test
{
    private GameObject testObject;
    private ChangeText changeText;
    private List<Button> buttons;
    private List<TextMeshProUGUI> textDisplays;

    [SetUp]
    public void Setup()
    {
        // Create a new GameObject and add ChangeText component
        testObject = new GameObject("TestObject");
        changeText = testObject.AddComponent<ChangeText>();

        // Initialize buttons and texts
        buttons = new List<Button>();
        textDisplays = new List<TextMeshProUGUI>();

        for (int i = 0; i < 3; i++) // Create 3 buttons for testing
        {
            GameObject buttonObj = new GameObject($"Button{i}");
            Button button = buttonObj.AddComponent<Button>();
            TextMeshProUGUI textDisplay = buttonObj.AddComponent<TextMeshProUGUI>();

            buttons.Add(button);
            textDisplays.Add(textDisplay);
        }

        // Assign to ChangeText component
        changeText.buttons = buttons;
        changeText.texts1 = new List<string> { "File", "Mute", "Share Screen" };
        changeText.texts2 = new List<string> { "File", "Unmute", "Stop Sharing Screen" };
    }

    [UnityTest]
    public IEnumerator Host_Desktop_UI_ChangeText_TestWithEnumeratorPasses()
    {
        // Simulate Start method manually
        //changeText.Start();

        // Verify initial state
        for (int i = 0; i < buttons.Count; i++)
        {
            Assert.AreEqual(changeText.texts1[i], textDisplays[i].text);
        }

        // Click the second button
        buttons[1].onClick.Invoke();
        yield return null;

        // Verify text has changed
        Assert.AreEqual("Unmute", textDisplays[1].text);
    }

    [TearDown]
    public void Teardown()
    {
        GameObject.Destroy(testObject);
    }
}
