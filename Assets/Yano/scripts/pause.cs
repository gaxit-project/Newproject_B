using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class test : MonoBehaviour
{
    public GameObject pause;
    public GameObject soundsetting;
    public GameObject backtitle;
    bool pause_status;

    public Button sound;
    public Button pausemenu;
    public Button titleback;



    // Start is called before the first frame update
    void Start()
    {
        sound = sound.GetComponent<Button>();
        pausemenu = pausemenu.GetComponent<Button>();
        if (!(SceneManager.GetActiveScene().name == "Start_Scene"))
        {
            titleback = titleback.GetComponent<Button>();
        }

        pause.SetActive(false);
        soundsetting.SetActive(false);
        if (!(SceneManager.GetActiveScene().name == "Start_Scene"))
        {
            backtitle.SetActive(false);
        }

    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) || Input.GetKeyDown("joystick button 6"))
        {
            Debug.Log("押されました");
            pause.SetActive(!pause.activeSelf);

            if (Time.timeScale == 0.0f)
            {
                Time.timeScale = 1.0f;
                pause.SetActive(false);
                soundsetting.SetActive(false);
                backtitle.SetActive(false);

            }
            else
            {
                Time.timeScale = 0.0f;
                pausemenu.Select();
            }

        }
    }

    public void EP()
    {
        pause.SetActive(false);
        //Debug.Log("ゲーム中です");
        Time.timeScale = 1.0f;
    }
    public void setsound()
    {
        soundsetting.SetActive(!soundsetting.activeSelf);
        pause.SetActive(!pause.activeSelf);
        if (soundsetting.activeSelf == true)
        {
            sound.Select();
        }
        if (pause.activeSelf == true)
        {
            pausemenu.Select();
        }
    }
    public void title()
    {
        pause.SetActive(!pause.activeSelf);
        backtitle.SetActive(!backtitle.activeSelf);
        if (backtitle.activeSelf == true)
        {
            titleback.Select();
        }
        if (pause.activeSelf == true)
        {
            pausemenu.Select();
        }
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
     Application.Quit();
#endif
    }
    public void Title()
    {
        Time.timeScale = 1.0f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("title");
    }
}
