using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Zoom with Mouse Wheel")]
    public float zoomSpeed = 500f;

    [Header("Move Camera with Mouse Panning")]
    public float cameraPanSpeed = 0.05f;
    private Vector3 lastPanningMousePos;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.platform);        
    }

    // Update is called once per frame
    void Update()
    {
        // ToDO: apply strategy design pettern?
         if (Application.platform == RuntimePlatform.WebGLPlayer ||
             Application.platform == RuntimePlatform.WindowsPlayer ||
             Application.platform == RuntimePlatform.OSXPlayer ||
             Application.platform == RuntimePlatform.LinuxPlayer ||
             Application.platform == RuntimePlatform.WindowsEditor
             )
         {

            // Zoom in and out based on mouse wheel
            Camera.main.orthographicSize  = Mathf.Clamp(Camera.main.orthographicSize + Input.GetAxis("Mouse ScrollWheel") * zoomSpeed * Time.deltaTime, 0.5f, 40f);

            // mouse panning with middle/wheel mouse button            
            if (Input.GetMouseButtonDown(2))
            {
                lastPanningMousePos = Input.mousePosition;
            }
 
            if (Input.GetMouseButton(2))
            {
                Vector3 delta = Input.mousePosition - lastPanningMousePos;
                Camera.main.transform.Translate(delta.x * cameraPanSpeed * Time.deltaTime, delta.y * cameraPanSpeed * Time.deltaTime, 0);
                lastPanningMousePos = Input.mousePosition;
            }
            
        }

    }
}
