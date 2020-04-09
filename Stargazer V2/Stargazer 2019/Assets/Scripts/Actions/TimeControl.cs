using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float slowdownFactor = 0.2f;
    public float slowdownLength = 3f;

    void Update()
    {
        if (!PauseMenu.IsGamePaused())
        {
            if (Input.GetMouseButtonDown(1))
            {
                ActivateSlowMotion();
            }
            else
            {
                Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
                Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
            }
        }
    }

    public void ActivateSlowMotion()
    {
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    }

}
