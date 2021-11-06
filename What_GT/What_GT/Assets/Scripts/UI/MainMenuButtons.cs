using UnityEngine.SceneManagement;
using UnityEngine;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class MainMenuButtons : MonoBehaviour
{
    public void UIQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void UIStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}