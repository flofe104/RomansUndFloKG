using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{


    public GameObject activateOnBossDeath;
    public GameObject deactivateOnBossDeath;

    private void Start()
    {
        activateOnBossDeath.SetActive(false);
        deactivateOnBossDeath.SetActive(true);
    }

    public void QuitGame()
    {

#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void PlayAgain()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }

}
