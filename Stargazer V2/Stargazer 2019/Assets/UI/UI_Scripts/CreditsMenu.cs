using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CreditsMenu : MonoBehaviour
{
    public GameObject CreditMenu;
    public GameObject MainMenu;
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu.SetActive(true);
            CreditMenu.SetActive(false);
        }
    }
}
