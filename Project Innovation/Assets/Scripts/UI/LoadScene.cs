using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    public void LoadSceneByName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void LoadSceneAsync(string name)
    {
         SceneManager.LoadSceneAsync(name);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
