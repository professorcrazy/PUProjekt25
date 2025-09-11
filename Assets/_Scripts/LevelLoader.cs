using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public int loadLevelId = 0;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            LoadNextLevel();
        }
    }

    public void LoadLevelByName(string name)
    {
        SceneManager.LoadScene(name);
    }
    public void LoadLevelByID(int id)
    {
        SceneManager.LoadScene(id);
    }
    public void LoadNextLevel()
    {
        LoadLevelByID((SceneManager.GetActiveScene().buildIndex + 1) % SceneManager.sceneCountInBuildSettings);
    }
}
