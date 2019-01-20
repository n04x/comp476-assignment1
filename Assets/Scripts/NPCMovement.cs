using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform Target;   // Reference the target position to chase.
    public float speed;
    float minimum_distance = 5.0f;
    
    void FixedUpdate() {
        // This is the the movement configuration for A.
        if(Vector3.Distance(transform.position, Target.position) > minimum_distance) {
            transform.LookAt(Target);
        }
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, Target.position, step);

        
   }
}
