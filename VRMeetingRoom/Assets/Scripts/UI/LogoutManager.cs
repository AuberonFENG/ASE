using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LogoutManager : MonoBehaviour
{
    // Start is called before the first frame update
    public Button LogoutBtn;
    void Start()
    {
        LogoutBtn.onClick.AddListener(OnLogoutButtonClick);
    }
    void OnLogoutButtonClick()
    {
        Debug.Log("OnButtonClick");
        SceneManager.LoadScene("UI1");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
