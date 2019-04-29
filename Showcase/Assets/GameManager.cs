using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameManager : MonoBehaviour {

    public bool isPaused = false;
    public Canvas pauseMenu;
    
    public void Pause(bool state)
    {
        isPaused = state;

        if (pauseMenu != null)
            pauseMenu.enabled = state;

        // Cursor
        switch (state)
        {
            case true:
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                break;

            case false:
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                break;

             default: break;
        }
        
    }

}
