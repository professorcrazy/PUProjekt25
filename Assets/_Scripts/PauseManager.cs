using UnityEngine;

public class PauseManager : MonoBehaviour
{
    KeyCode pauseKey = KeyCode.Escape;
    public GameObject pauseMenu;
    bool isPaused = false;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(pauseKey))
        {
            PauseSystem();
        }
    }

    void PauseSystem()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true);
            }
            Time.timeScale = 0f;
            
        }
        else
        {
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
            Time.timeScale = 1f;
        }
    }
}
