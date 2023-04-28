using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public float zoomSpeed = 500f;
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
        }

    }
}
