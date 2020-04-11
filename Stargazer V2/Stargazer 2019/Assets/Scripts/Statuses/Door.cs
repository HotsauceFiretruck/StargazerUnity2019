using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour
{
    public void Open()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
