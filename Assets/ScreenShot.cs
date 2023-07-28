using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
public class ScreenShot : MonoBehaviour
{
    private static ScreenShot Instance;
    public GameObject renderCamera;
    private Camera camera;

    public void Awake()
    {
        camera = renderCamera.GetComponent<Camera>();
    }

    public void createCameraRenderTexture()
    {

        if(camera.targetTexture == null)
        {
            camera.targetTexture = RenderTexture.GetTemporary(1024, 768, 16);        
        }

    }

    public Texture2D takeSnapshot(RenderTexture renderTexture)
    {
            Debug.Log("taking snapshot:START");
            Texture2D resultTexture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
            Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);

            RenderTexture.active = renderTexture;
            resultTexture2D.ReadPixels(rect, 0, 0); // read pixels based on rectangle info from targetCamera.targetTexture
            resultTexture2D.Apply();
            Debug.Log("taking snapshot:DONE");
            return resultTexture2D;
    }

    public void saveSnapshot(RenderTexture renderTexture, string filepath)
    {
            Texture2D resultTexture2D = takeSnapshot(renderTexture);
            byte[] byteArray = resultTexture2D.EncodeToPNG();
            System.IO.File.WriteAllBytes(filepath, byteArray);
    }

    public IEnumerator saveSnapshot(string outputFilePath)
    {
        renderCamera.SetActive(true);
        createCameraRenderTexture();
        Debug.Log("activate renderCamera");
        yield return new WaitForSeconds(2f);
        saveSnapshot(camera.targetTexture, outputFilePath);
        renderCamera.SetActive(false);
        Debug.Log("deactivate renderCamera");
        yield return new WaitForSeconds(2f);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("space is pressed");
            string outputFileName = Application.dataPath + "/SnapShots/" + gameObject.name + ".png";
            StartCoroutine(saveSnapshot(outputFileName));
        }
    }


}
// public class ScreenShot : MonoBehaviour
// {
//     private static ScreenShot Instance;
//     private Camera targetCamera;
//     public Transform target;

//     private bool isScreenShotJustTakenOnNextFrame = false;
//     // Start is called before the first frame update
//     void Start()
//     {
//         Instance = this;
//         targetCamera = target.GetComponent<Camera>();
//     }
    
//     private void TakeScreenshotFromTargetCamera(int width, int height)
//     {
//         Debug.Log("TakeScreenshotFromTargetCamera()");

//         targetCamera.targetTexture = RenderTexture.GetTemporary(width, height, 16);
//         isScreenShotJustTakenOnNextFrame = true;
//     }

//     public void OnPostRender()
//     {
//         Debug.Log("OnPostRender()");

//         if(isScreenShotJustTakenOnNextFrame)
//         {
//             Debug.Log("OnPostRender()- saving snapshot");
//             isScreenShotJustTakenOnNextFrame = false;
//             RenderTexture renderTexture = targetCamera.targetTexture;

//             Texture2D renderResult = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.ARGB32, false);
//             Rect rect = new Rect(0, 0, renderTexture.width, renderTexture.height);
//             renderResult.ReadPixels(rect, 0, 0); // read pixels based on rectangle info from targetCamera.targetTexture


//             byte[] byteArray = renderResult.EncodeToPNG();

//             System.IO.File.WriteAllBytes(Application.dataPath + "/ScreenShots/" + gameObject.name + ".png", byteArray);

//             RenderTexture.ReleaseTemporary(renderTexture);
//             targetCamera.targetTexture = null;
//             Debug.Log("A snapshot is taken - " + gameObject.name);
//         }
//     }

//     // Update is called once per frame
//     void Update()
//     {
//         if(Input.GetKeyDown(KeyCode.Space))
//         {
//             Debug.Log("space is pressed");
            
//             takeScreenShot();
//         }
//     }

//     public static void takeScreenShot()
//     {
//         Instance.TakeScreenshotFromTargetCamera(500,500);
//     }
// }
