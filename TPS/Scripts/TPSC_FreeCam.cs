using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TPSC_FreeCam : TPS_Controller {

    // #########################################################
    // # --------------- CONTROLLER VARIABLES ---------------- #
    // #########################################################

    [Header("Motion Settings")] // m_
    [SerializeField] [Range(0.1f, 3.0f)] float m_Speed = 0.15f;
    private float m_xForce, m_yForce;
    private Vector2 ForceVector;

    // #########################################################
    // -------------------- INITIALIZATION ------------------- #
    // #########################################################

    protected override void Awake()
    {
        base.Awake();

    }

    // #########################################################
    // # ---------------------- UPDATE ----------------------- #
    // #########################################################

    public override void Tick()
    {
        if (owner == null) return;

        base.Tick();

        Motion();
    }

    // #########################################################
    // # --------------------- CONTROLS ---------------------- #
    // #########################################################

    // Player Movement Controls
    private void Motion()
    {

        // X/Y Motion
        m_xForce = Input.GetAxis("Horizontal") * m_Speed;
        m_yForce = Input.GetAxis("Vertical") * m_Speed;

        // Calculate Motion
        Vector3 motion = new Vector3(m_xForce, 0, m_yForce);
        motion = cam.transform.rotation * motion;
        transform.position += motion;

    }

    public override TPS_Controller Possess(TPS_Player player)
    {
        // Cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        return base.Possess(player);
    }
}
