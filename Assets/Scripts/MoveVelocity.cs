using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveVelocity : MonoBehaviour
{
    Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetMoveVelocity(Vector3 velocity)
    {
        this.velocity = velocity;
    }


    // Update is called once per frame
    void Update()
    {
        if(this.velocity!=null) this.transform.position  += velocity * Time.deltaTime;          
    }
}
