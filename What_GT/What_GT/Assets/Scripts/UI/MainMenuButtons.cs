using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR

using UnityEditor;

#endif

public class MainMenuButtons : MonoBehaviour
{

    public Text Text;

    public void UIQuit()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif

        PlayerPrefs.SetString("who", null);
    }

    private void Start()
    {
        Text.text = PlayerPrefs.GetString("who");
    }

    public void UIStart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            UIStart();
    }
}