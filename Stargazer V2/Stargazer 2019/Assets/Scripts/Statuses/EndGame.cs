using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    public Transform orientation;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (Vector3.Distance(orientation.position, transform.position) <= 10)
            {
                if (Physics.Raycast(orientation.position, orientation.forward, out RaycastHit hitInfo, 5.0f))
                {
                    if (hitInfo.transform.CompareTag("End Game"))
                    {
                        GameManager.gameEnded = true;
                        SceneManager.LoadScene(0);
                    }
                }
            }
        }
    }
}
