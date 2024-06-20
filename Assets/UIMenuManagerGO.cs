using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManagerGO : MonoBehaviour
{
    public void LoadGameScene()
    {
        AudioManager.Instance.musicSource.Stop();
        AudioManager.Instance.PlayMusic("Menu");
        SceneManager.LoadScene(0);

    }
}
