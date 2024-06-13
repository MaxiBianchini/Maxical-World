using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private TMP_Text respawnCountText;
    [SerializeField] private int respawnTime = 5;
    [SerializeField] private float tutorialShowingTime = 5f;
    [SerializeField] private float imageFadeOutTime = 2f;
    [SerializeField] private Image attackTutorialImg;
    [SerializeField] private Image buildTutorialImg;

    private PlayerController player;
    
    private bool startCount = false;
    private float remainingTime;
    

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        player = FindObjectOfType<PlayerController>();

        //respawnCountText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        StartCoroutine(ShowTutorialMessagesRoutine());
        remainingTime = respawnTime;
    }

    private IEnumerator ShowTutorialMessagesRoutine()
    {
        attackTutorialImg.enabled = true;
        yield return new WaitForSeconds(tutorialShowingTime);
        StartCoroutine(FadeOutImage(attackTutorialImg));
        yield return new WaitForSeconds(imageFadeOutTime);

        buildTutorialImg.enabled = true;
        yield return new WaitForSeconds(tutorialShowingTime);
        StartCoroutine(FadeOutImage(buildTutorialImg));



    }
    
    public IEnumerator FadeOutImage(Image imageToFade)
    {
        // Obtén el color inicial de la imagen
        Color originalColor = imageToFade.color;
        float elapsedTime = 0f;

        while (elapsedTime < imageFadeOutTime)
        {
            elapsedTime += Time.deltaTime;
            // Calcula el nuevo valor alfa
            float alpha = Mathf.Lerp(1, 0, elapsedTime / imageFadeOutTime);
            imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Asegúrate de que la imagen sea completamente transparente al final
        imageToFade.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0);
    }

    private void OnEnable()
    {
        player.onPlayerDeath += RespawnPlayer;
    }


    private void RespawnPlayer(object sender, EventArgs e)
    {
        PlayerController player = sender as PlayerController;
        StartCoroutine(RespawnRoutine(player));
    }

    private IEnumerator RespawnRoutine(PlayerController player)
    {
        //seteo la posicion del player
        Vector3 respawnPoint = FindObjectOfType<RespawnPoint>().transform.position;
        if (respawnPoint == null) yield return null;
        player.transform.position = respawnPoint;
        
        //Hago aparecer el texto y el contador
        SetTextVisibility(respawnCountText, true);
        startCount = true;
        yield return new WaitForSeconds(respawnTime);
        
        //Hago aparecer el player y desactivo el contador
        player.gameObject.SetActive(true);
        player.ResetHealthValue();
        startCount = false;
        remainingTime = respawnTime;
        SetTextVisibility(respawnCountText, false);
    }

    private void Update()
    {
        if (startCount)
        {
           remainingTime -= Time.deltaTime;
           int seconds = Mathf.FloorToInt(remainingTime);
            respawnCountText.text = $"Respawning in {seconds.ToString("D1")} secs";
        }
    }

    private void SetTextVisibility(String texto, TMP_Text text, bool isVisible)
    {
        Color color = text.color;
        color.a = isVisible ? 1f : 0f;
        text.color = color;
        text.text = texto;
    }

    private void SetTextVisibility(TMP_Text text, bool isVisible)
    {
        Color color = text.color;
        color.a = isVisible ? 1f : 0f;
        text.color = color;
    }
}
