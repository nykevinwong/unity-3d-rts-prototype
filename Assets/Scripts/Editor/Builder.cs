//place this script in the Editor folder within Assets.
using UnityEditor;


//to be used on the command line:
//$ Unity -quit -batchmode -executeMethod Builder.BuildWebGL
// "C:\Program Files\Unity\Hub\Editor\2021.3.3f1\Editor\Unity.exe" -batchmode -executeMethod Builder.BuildWebGL

class Builder {
    static void BuildWebGL() {
        string[] scenes = {"Assets/SampleScene.unity"};
        BuildPipeline.BuildPlayer(scenes, "dist-webgl", BuildTarget.WebGL, BuildOptions.None);
    }
}