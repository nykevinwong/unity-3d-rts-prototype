using UnityEditor;
using UnityEngine;
using System;
using System.IO;

public class ModelCustomImportSteps : AssetPostprocessor
{
    // https://medium.com/codex/hackn-slash-interlude-1-automating-your-unity-imports-cd2ae594bf5c

    // convert fbx into prefab
    // https://blog.csdn.net/yuxikuo_1/article/details/54983667
    void OnPreprocessModel()
    {
            Debug.Log("OnPreprocessModel:"+assetPath);

        if (assetPath.EndsWith(".FBX", StringComparison.OrdinalIgnoreCase))  // @-sign in the name triggers this step
        {
            ModelImporter modelImporter = assetImporter as ModelImporter;
            modelImporter.materialImportMode = ModelImporterMaterialImportMode.None;

            if(assetPath.Contains("STRUCT_", StringComparison.OrdinalIgnoreCase))
            {
                modelImporter.useFileScale = true;
                modelImporter.globalScale = 0.2f;
            }

            modelImporter.SaveAndReimport();

        }
    }

    static void ProcessFBX(string assetPath)
    {  
            if (!Directory.Exists("Assets/Prefabs"))
            { AssetDatabase.CreateFolder("Assets", "Prefabs"); }

            GameObject modelGameObject = AssetDatabase.LoadMainAssetAtPath(assetPath) as GameObject; ;
            GameObject instanceRoot = (GameObject)PrefabUtility.InstantiatePrefab(modelGameObject);
            Debug.Log("modelGameObject:" + modelGameObject.name);
            Debug.Log("instanceRoot:" + instanceRoot.name);
            
            String localPath = "Assets/Prefabs/fbx/" + Path.GetFileName(assetPath).Replace(".FBX",".prefab");
            localPath = AssetDatabase.GenerateUniqueAssetPath(localPath);
            
        
//            bool prefabSuccess;
            PrefabUtility.SaveAsPrefabAssetAndConnect(instanceRoot, localPath, InteractionMode.UserAction, out bool prefabSuccess);
              
            UnityEngine.Object.DestroyImmediate(instanceRoot); 

            if (prefabSuccess == true)
                Debug.Log("Prefab was saved successfully");
            else
                Debug.Log("Prefab failed to save" + prefabSuccess);

    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths, bool didDomainReload)
    {
        foreach(string assetPath in importedAssets)
        {
            Debug.Log("OnPostprocessAllAssets:"+assetPath);
             if (assetPath.EndsWith(".FBX", StringComparison.OrdinalIgnoreCase))
             {
               ProcessFBX(assetPath);
             }
        }
    }
}