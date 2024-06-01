using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class MainMenuEvents : MonoBehaviour
{
    //private void PlayGame()
    //{
      //  SceneManager.LoadSceneAsync(2);

   // }
    private UIDocument _document;

    private Button _button;

    private void Awake()
    {
        _document = GetComponent<UIDocument>();

        _button = _document.rootVisualElement.Q("StartGameButton") as Button;
        _button.RegisterCallback<ClickEvent>(onPlayGameClick);
    }
    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(onPlayGameClick);
    }
    private void onPlayGameClick(ClickEvent evt)
    {
       Debug.Log("You Pressed the Start Button");
       SceneManager.LoadSceneAsync(1);
    }
}