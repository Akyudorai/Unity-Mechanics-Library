using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class MENU : MonoBehaviour {

    public GameManager game;

    public void UNPAUSE()
    {
        if (game != null) game.Pause(false);
    }

    public void NAVIGATE(int index)
    {
        SceneManager.LoadScene(index);
    }

}
