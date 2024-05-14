using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilsClass //it's static so we can access it anywhere
{
    private static Camera mainCamera;
    
    //This method returns the position of the mouse in the screen based on what the camera sees
    public static Vector3 GetMouseWorldPosition()
    {
        if(mainCamera == null) mainCamera = Camera.main;

        //Vector3 mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        //mouseWorldPosition.z = 0f;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if(Physics.Raycast(ray, out RaycastHit raycastHit))
        {
            //Debug.Log(raycastHit.point.ToString());
            return raycastHit.point;

        }
        else
        {
            //Debug.Log("No encontre collision con el mouse");
            return Vector3.zero;
        }
        
    }

    public static Vector3 GetRandomDir()
    {
        return new Vector3(Random.Range(-1f, 1f), Random.Range(-1f,1f)).normalized;
    }

    //Static method to calculate the rotation of an object. Ex. the Arrow
    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y,vector.x);
        float degrees = radians * Mathf.Rad2Deg; 
        return degrees;
    }
}
