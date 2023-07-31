using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGhost : MonoBehaviour
{
    public static ObjectGhost Instance { get; private set; }
    // Start is called before the first frame update
    public Transform targetPrefab;
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
                
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
