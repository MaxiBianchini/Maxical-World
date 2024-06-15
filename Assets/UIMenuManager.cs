using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenuManager : MonoBehaviour
{
   public void LoadGameScene()
    {
        SceneManager.LoadScene(1);
    }
}
