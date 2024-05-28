using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    private Camera mainCamera;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Asegúrate de que el TextMesh esté siempre mirando hacia la cámara
        if (mainCamera != null)
        {
            transform.LookAt(mainCamera.transform);
            transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);
        }
    }
}

