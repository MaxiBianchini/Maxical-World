using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    [SerializeField] private TMP_Text respawnCountText;
    [SerializeField] private int respawnTime = 5;

    private PlayerController player;
    
    private bool startCount = false;
    private float remainingTime;
    

    private void Awake()
    {
        player = FindObjectOfType<PlayerController>();

        //respawnCountText = GetComponentInChildren<TMP_Text>();
    }

    private void Start()
    {
        remainingTime = respawnTime;
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
