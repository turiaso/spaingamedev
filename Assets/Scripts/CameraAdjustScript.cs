using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAdjustScript : MonoBehaviour
{
    public GameObject rope;
    public Camera mainCamera;

    public float zoomVelocity = 1;

    public int zoomOutMin = 1;
    public int zoomOutMax = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        bool allVisibles = true;
        foreach (Transform child in rope.transform)
        {
            Vector3 screenPos = mainCamera.WorldToScreenPoint(child.position);
            allVisibles = allVisibles && screenPos.x - 700 > 0f && screenPos.x + 700 < Screen.width && screenPos.y-700 > 0f && screenPos.y+700 < Screen.height; ;
            if (!allVisibles)
            {                
                break;
            }
        }
        if (allVisibles)
        {
            zoom(zoomVelocity * Time.deltaTime);
        }
        else
        {
            zoom(-zoomVelocity * Time.deltaTime);
        }
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y,rope.transform.GetChild(0).position.z);
    }

    void zoom(float increment)
    {

        if (mainCamera.orthographic)
        {
            mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize - increment, zoomOutMin, zoomOutMax);
        }
        else
        {
            mainCamera.fieldOfView = Mathf.Clamp(mainCamera.fieldOfView - increment, zoomOutMin, zoomOutMax);
        }
    }
}
