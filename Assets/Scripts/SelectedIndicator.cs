using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedIndicator : MonoBehaviour
{
    private Renderer renderer;

    void Awake()
    {
        renderer = GetComponent<Renderer>();
    }

    public void Selected()
    {
        renderer.material.color = Color.red;
    }

    public void Unselected()
    {
        renderer.material.color = Color.white;
    }
    // Update is called once per frame
    // public virtual void OnDestroy()
    // {
    //     renderer.material.color = Color.white;
    // }
}
