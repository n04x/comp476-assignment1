using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAIBehavior : MonoBehaviour
{
    private Rigidbody rb;
    public GameObject target;
    public Quaternion target_rotation;
    public Vector3 velocity;
    public float statisfaction_radius = 0.1f;
    public float time2target = 1.0f;
    public float rotation_speed = 0.5f;
    public float maximum_speed = 5.0f;
    public float speed_cutoff = 0.5f;
    public float minimum_distance = 0.5f;
    float maximum_distance = 10.0f;

    float speed = 2.0f;
    float near_speed = 1.0f;
    float near_radius = 5.0f;
    float arrival_radius = 0.5f;
    float distance_from_target;
    float max_wander_variance = 0.0f;
    float wander_offset =  200.0f;
    float wander_radius = 100.0f;
    Vector3 current_random_point;

    int action = 0;
    // check if the player has been tagged or not.
    bool tagged = false;
    float wander_timer_refresh = 0.0f;

    void Start() {
        // target_rotation = transform.rotation;    
        // rb = GetComponent<Rigidbody>();
        action = 0;

    }
    void FixedUpdate() {
        if(!tagged) {
            if(action == 0) {
            Wander();
            } else if(action == 1) {
                Arrive();
            } else
            {
                Flee();
            }
        } else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
            transform.position = transform.position;
        }
    }

    void Arrive() {
        distance_from_target = (target.transform.position - transform.position).magnitude;
        if(distance_from_target > near_radius) {
            transform.LookAt(target.transform);
            GetComponent<Rigidbody>().velocity = ((target.transform.position - transform.position).normalized * speed);
        } else if(distance_from_target > arrival_radius) {
            GetComponent<Rigidbody>().velocity = ((target.transform.position - transform.position).normalized * near_speed);
        } else
        {
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        }
    }

    void Flee() {
        distance_from_target = (target.transform.position - transform.position).magnitude;
        if(distance_from_target > arrival_radius) {
            transform.rotation = Quaternion.LookRotation(transform.position - target.transform.position);
            GetComponent<Rigidbody>().velocity = -((target.transform.position - transform.position).normalized * speed);         
        } else {
            GetComponent<Rigidbody>().velocity = -((target.transform.position - transform.position).normalized * speed);         
        }
    }

    void Wander() {
        wander_timer_refresh -= Time.deltaTime;
        if(wander_timer_refresh < 0) {
            current_random_point = WanderCirclePoint();
            transform.LookAt(current_random_point);
            Vector3 move_direction = (current_random_point - transform.position).normalized;
            GetComponent<Rigidbody>().velocity = (move_direction * speed);
            wander_timer_refresh = 3.0f;
        }
    }

    void Pursue() {
        distance_from_target = (target.transform.position - transform.position).magnitude;
        transform.LookAt(target.transform);
        GetComponent<Rigidbody>().velocity = ((target.transform.position - transform.position).normalized * speed + target.GetComponent<Rigidbody>().velocity);
    }

    Vector3 WanderCirclePoint() {
        Vector3 wander_circle_center = transform.position + (Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * wander_offset);
        Vector3 wander_circle_point = wander_radius * (new Vector3(Mathf.Cos(Random.Range(max_wander_variance, Mathf.PI - max_wander_variance)), 0.0f, Mathf.Sin(Random.Range(max_wander_variance, Mathf.PI - max_wander_variance))));
        return (wander_circle_point + wander_circle_center);
    }

    void OnCollisionEnter(Collision other) {
        Debug.Log("Collided");
        if(other.gameObject.tag == "Red" && transform.position.x > 0) {
            tagged = true;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        } else if(other.gameObject.tag == "Red") {
            action = 2;
            target = other.gameObject;
        } else {
            return;
        }
    }
}
