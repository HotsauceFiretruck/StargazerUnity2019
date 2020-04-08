using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartButton : MonoBehaviour {

    public Button startButton;
    void Start ()
    {
        startButton= GetComponent<Button>();
		startButton.onClick.AddListener(TaskOnClick);
        
    }

    void TaskOnClick()
    {
        SceneManager.LoadScene("Game");
    }

    // public void QuitGame ()
    // {
    //     Debug.Log("QUIT");
    //     Application.Quit();
    // }

}
