using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayLightSystem : MonoBehaviour
{
    [SerializeField] private float dayNightSpeed = 1f;

    List<GameObject> torches = new List<GameObject>();
    float xRotation;

    private void Start()
    {
        xRotation = 60f;
        torches.AddRange(GameObject.FindGameObjectsWithTag("Lights"));
    }

    void Update()
    {
        xRotation += dayNightSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(xRotation, transform.rotation.y, transform.rotation.z);

        float xEuler = transform.rotation.eulerAngles.x;
        if (xEuler >= 0f && xEuler <= 150f)
        {
            OnAndOffTorches(false);
        }
        else
        {
            OnAndOffTorches(true);
        }
     

    }

    private void OnAndOffTorches(bool value)
    {
        foreach (GameObject go in torches)
        {
            if(go != null)
            {
                go.SetActive(value);
            }
        }
    }
}
