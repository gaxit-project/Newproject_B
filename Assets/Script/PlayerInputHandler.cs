using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private ShieldController shieldController;
    private BossScript bossScript;
    void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        shieldController = FindObjectOfType<ShieldController>();
        bossScript = FindObjectOfType<BossScript>();

        if (playerMovement == null)
            Debug.LogError("PlayerMovement が見つかりませんで！");
        if (shieldController == null)
            Debug.LogError("ShieldController が見つかりませんで！");
        if (bossScript == null)
            Debug.LogError("BossScript が見つかりませんで！");
    }

    void Update()
    {
        if (Time.timeScale == 0 || (bossScript != null && bossScript.GetBossHP() <= 0)) return; // タイムスケールが 0 または BossのHPが0以下のときは入力を受け付けない

        if ((Input.GetKeyDown(KeyCode.E) || Input.GetKeyDown("joystick button 0")) && !shieldController.IsReflecting())
        {
            playerMovement?.StartDodge();
            shieldController?.TriggerReflect();
        }
    }
}
