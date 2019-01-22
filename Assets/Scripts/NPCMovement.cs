using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    public Transform Target;   // Reference the target position to chase.
    public int AIType;
    public float speed;
    float minimum_distance = 5.0f;
    float maximum_distance = 30.0f;
    
    void FixedUpdate() {
        // This is the movement configuration for A.
        if(AIType == 0) {
            MovingTowardTarget();
        }

        // This is the movement configuration for C.
        if(AIType == 2) {
            RunningAwayFromTarget();
        }
   }

   void MovingTowardTarget() {
       float step = speed * Time.deltaTime;
       if(Vector3.Distance(transform.position, Target.position) > minimum_distance) {
           transform.LookAt(Target);
        //    transform.Translate(Vector3.forward * Time.deltaTime * speed);
       }
       transform.position = Vector3.MoveTowards(transform.position, Target.position, step);
   }
   void RunningAwayFromTarget() {
        int direction = -1;
        if(Vector3.Distance(transform.position, Target.position) >= minimum_distance) {
            transform.LookAt(Target);
            transform.Rotate(0,180,0);
        } 
        float step = speed * Time.deltaTime * direction;
        transform.position = Vector3.MoveTowards(transform.position, Target.position, step);
//    float step = speed * Time.deltaTime;
    //    transform.position = direction * Vector3.MoveTowards(transform.position, Target.position, step);
   }
}
