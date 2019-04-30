using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GM_Iso_RTS : GameManager {

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!isPaused) Pause(true);
            else Pause(false);
        }
    }
}
