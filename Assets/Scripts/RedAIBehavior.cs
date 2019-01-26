using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedAIBehavior : MonoBehaviour
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

    // check if the player has been tagged or not.
    float wander_timer_refresh = 0.0f;

    public enum Actions {WANDER, ARRIVE, FLEE, PURSUE, TAGGED};
    public Actions current_action;
    public bool enemy_area = false;
    public bool has_flag = false;

    void Start() {
        // target_rotation = transform.rotation;
        // rb = GetComponent<Rigidbody>();
    }
    void FixedUpdate() {
        if(has_flag) {
            Debug.Log(gameObject.tag + " team has the flag!");
        }
        if(current_action == Actions.WANDER) {
            Wander();
        } else if(current_action == Actions.ARRIVE) {
            Arrive();
        } else if(current_action == Actions.FLEE) {
            Flee();
        } else if(current_action == Actions.PURSUE) {
            Pursue();
        } else if(current_action == Actions.TAGGED) {
            transform.Find("Tagged").GetComponent<MeshRenderer>().enabled = true;
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
        if(distance_from_target > minimum_distance) {
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

    Vector3 WanderCirclePoint() {
        Vector3 wander_circle_center = transform.position + (Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * wander_offset);
        Vector3 wander_circle_point = wander_radius * (new Vector3(Mathf.Cos(Random.Range(max_wander_variance, Mathf.PI - max_wander_variance)), 0.0f, Mathf.Sin(Random.Range(max_wander_variance, Mathf.PI - max_wander_variance))));
        return (wander_circle_point + wander_circle_center);
    }

    void Pursue() {
        distance_from_target = (target.transform.position - transform.position).magnitude;
        transform.LookAt(target.transform);
        GetComponent<Rigidbody>().velocity = ((target.transform.position - transform.position).normalized * speed + target.GetComponent<Rigidbody>().velocity);
    }
    void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Blue" && enemy_area || has_flag) {
            has_flag = false;
            current_action = Actions.TAGGED;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        } else if(other.gameObject.tag == "Blue") {
           current_action = Actions.FLEE;
            target = other.gameObject;
        } else {
            return;
        }
    }

    public int currentAction() {
        return (int) current_action;
    }
    public void setActions(int action) {
        if(action == 1) {
            current_action = Actions.ARRIVE;
        } else if(action == 2) {
            current_action = Actions.FLEE;
        } else if(action == 3) {
            current_action = Actions.PURSUE;            
        } else if(action == 4) {
            current_action = Actions.TAGGED;
        } else {
            current_action = Actions.WANDER;
        }
    }
}
