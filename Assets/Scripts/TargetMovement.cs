using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetMovement : MonoBehaviour
{
    public GameObject targetGameObject;
    // Start is called before the first frame update
    MoveVelocity moveVelocityScript;

    public void SetTargetGameObject(GameObject targetGameObject)
    {
//        SelectedIndicator indicator = GetComponent<SelectedIndicator<>();

         // you can change target only when the entity is currently selected.
            this.targetGameObject = targetGameObject;
        
    }
    
    void Start()
    {
        moveVelocityScript = this.GetComponent<MoveVelocity>();
    }

    // Update is called once per frame
    void Update()
    {
        if(targetGameObject==null || targetGameObject.activeSelf==false) return;

        Vector3 moveDir = (targetGameObject.transform.position - transform.position).normalized;
        moveVelocityScript.SetMoveVelocity(moveDir);
        //moveVelocityScript.SetMoveVelocity(new Vect)        
        if(moveDir.x==0 && moveDir.z==0) 
        {
            Debug.Log($"TargetMovement: { this.GetInstanceID() } has arrived the target position");
       
        }

    }
}
