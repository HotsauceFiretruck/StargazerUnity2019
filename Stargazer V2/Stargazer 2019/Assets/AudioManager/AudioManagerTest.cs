using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerTest : MonoBehaviour
{
    [SerializeField] private AudioClip buttonClickSFX;
    [SerializeField] private AudioClip music1;
    [SerializeField] private AudioClip music2;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            AudioManager.Instance.PlaySFX(buttonClickSFX, 1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            AudioManager.Instance.PlayMusic(music1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            AudioManager.Instance.PlayMusic(music2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            AudioManager.Instance.PlayMusicWithFade(music1);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            AudioManager.Instance.PlayMusicWithFade(music2);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            AudioManager.Instance.PlayMusicWithFadeWithCrossFade(music1, 3.0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            AudioManager.Instance.PlayMusicWithFadeWithCrossFade(music2, 3.0f);
        }
    }
}
