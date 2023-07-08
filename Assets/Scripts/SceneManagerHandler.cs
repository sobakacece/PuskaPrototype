using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class SceneManagerHandler : MonoBehaviour
{
    SceneManagerInput input;

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);

    }
    // Start is called before the first frame update
    void OnEnable()
    {
        if (input == null)
        {
            input = new SceneManagerInput();
        }
        input.Enable();

        input.Main.Exit.performed += Exit;
        input.Main.Reload.performed += Restart;

    }
    void OnDisable()
    {
        input.Main.Exit.performed -= Exit;
        input.Main.Reload.performed -= Restart;
    }
    private void Exit(InputAction.CallbackContext ctx)
    {
        Application.Quit();
    }
    private void Restart(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
