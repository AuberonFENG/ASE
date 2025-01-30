using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Chage_Button_Text : EditorWindow
{
    [MenuItem("Window/UI Toolkit/Chage_Button_Text")]
    public static void ShowExample()
    {
        Chage_Button_Text wnd = GetWindow<Chage_Button_Text>();
        wnd.titleContent = new GUIContent("Chage_Button_Text");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        //Import UXML created manually.
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Editor/SimpleCustomEditor_uxml.uxml");
        VisualElement labelFromUXML_uxml = visualTree.Instantiate(); 
        root.Add(labelFromUXML_uxml);

    }
}
