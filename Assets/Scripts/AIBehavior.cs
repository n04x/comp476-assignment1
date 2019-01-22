using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBehavior : MonoBehaviour
{
    private Rigidbody rb;
    public Transform Target;
    public Quaternion target_rotation;
    public Vector3 velocity;
    public float statisfaction_radius = 0.1f;
    public float time2target = 1.0f;
    public float rotation_speed = 0.5f;
    public float maximum_speed = 5.0f;
    public float speed_cutoff = 0.5f;
    public float minimum_distance = 0.5f;
    float maximum_distance = 10.0f;

    void Start() {
        target_rotation = transform.rotation;    
        rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate() {
        KinematicArrive();    
        // KineamticFlee();
    }

    void KinematicArrive() {
        velocity = Target.position - transform.position;
        if(velocity.magnitude < statisfaction_radius) {
            velocity = Vector3.zero;
        }
        velocity /= time2target;

        if (velocity.magnitude > maximum_speed)
        {
            velocity.Normalize();
            velocity *= maximum_speed;
        }
        Debug.Log("velocity: " + velocity.magnitude);
        target_rotation.SetLookRotation(Target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, rotation_speed * Time.deltaTime);
        transform.position = (transform.position + velocity * Time.deltaTime);
   }
   void KineamticFlee() {
       if(Vector3.Distance(transform.position, Target.position) < maximum_distance) {
           velocity = transform.position - Target.position;
           velocity.Normalize();
           velocity *= maximum_speed;
           if(Vector3.Distance(transform.position, Target.position) >= minimum_distance) {
               transform.LookAt(Target);
               transform.Rotate(0, 180, 0);
           }
           transform.position = (transform.position + velocity * Time.deltaTime);
       }
   }
}
