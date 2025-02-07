using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update
    public Button ExitBtn;
    public GameObject ExitPage;
    public GameObject NextPage;
    void Start()
    {
        ExitBtn.onClick.AddListener(OnExitButtonClick);
    }
    void OnExitButtonClick()
    {
        Debug.Log("OnLoginButtonClick");
        NextPage.SetActive(true);
        ExitPage.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
