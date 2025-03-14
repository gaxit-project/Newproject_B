using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private ShieldController shieldController;

    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        shieldController = FindObjectOfType<ShieldController>();

        if (playerMovement == null)
            Debug.LogError("PlayerMovement が見つかりません！");
        if (shieldController == null)
            Debug.LogError("ShieldController が見つかりません！");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 0"))
        {
            playerMovement?.StartDodge();
            shieldController?.TriggerReflect();
        }
    }
}
