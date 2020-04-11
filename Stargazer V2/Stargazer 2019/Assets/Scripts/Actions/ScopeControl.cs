using UnityEngine;

public class ScopeControl : MonoBehaviour
{
    bool isScoped = false;

    public float scopedFOV = 30f;
    private float normalFOV = 95f;

    Camera view;

    void Start()
    {
        view = GetComponentInChildren<Camera>();
    }

    void Scope()
    {
        if (view != null)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                isScoped = !isScoped;

                if (isScoped)
                {
                    view.fieldOfView = scopedFOV;
                }
                else
                {
                    view.fieldOfView = normalFOV;
                }
            }
        }
    }

    public void Reset()
    {
        if (view != null)
        {
            view.fieldOfView = normalFOV;
        }
    }

    void Update()
    {
        Scope();
    }
}
