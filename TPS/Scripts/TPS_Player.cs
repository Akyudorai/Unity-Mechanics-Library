using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPS_Player : MonoBehaviour {

    // Game Manager Reference
    public GameManager gm;
    public TPS_Controller activeController;

    [SerializeField] private TPSC_FreeCam defaultController;

    private void Start()
    {
        if (activeController != null)
        {
            activeController.Possess(this);
        }

        else
        {
            activeController = defaultController.Possess(this);
        }
    }

    private void Update()
    {
        if (activeController != null && !gm.isPaused)
            activeController.Tick();

        if (Input.GetKeyDown(KeyCode.Mouse4))
        {
            //Debug.Log("Attempting to Possess");

            Vector3 rayOrigin = activeController.cam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
            RaycastHit hit;

            // Check if there's an interactable target in range.
            if (Physics.Raycast(rayOrigin, activeController.cam.transform.forward, out hit, 5.0f))
            {
                // Declare Target;
                GameObject targetHit = hit.transform.gameObject;
                //Debug.Log(targetHit.name);

                if (targetHit.GetComponentInParent<TPS_Controller>() != null)
                {
                    activeController.Eject();
                    activeController = targetHit.GetComponentInParent<TPS_Controller>().Possess(this);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse3))
        {
            activeController.Eject();
            activeController = defaultController.Possess(this);
        }
    }


}
