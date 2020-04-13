using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreditsMenu : MonoBehaviour
{
    public GameObject CreditMenu;
    public GameObject MainMenu;
    void Update()
    {
        if (Input.anyKey && CreditMenu.activeSelf)
        {
            MainMenu.SetActive(true);
            CreditMenu.SetActive(false);
        }

        if (GameManager.gameEnded)
        {
            MainMenu.SetActive(false);
            print("hi");
            CreditMenu.SetActive(true);
            GameManager.gameEnded = false;
        }
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.None;
    }
}
