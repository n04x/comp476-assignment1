using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueAIBehavior : MonoBehaviour
{
    // List of all game object needed for AI Behavior.
    private GameController game_controller;
    public GameObject target;
    public GameObject tagged_aura;

    // List of all quaternion needed for AI Behavior.
    public Quaternion target_rotation;
    
    // variable used to calculate the distance.
    float distance_from_target;

    // list of variables used for Arrive().
    public float time2target = 1.0f;
    float near_speed = 1.0f;
    float near_radius = 5.0f;
    float arrival_radius = 0.5f;
    
    // list of variables used for Flee().
    public float minimum_distance = 0.5f;
    float flee_time = 1.0f;

    // list of variables used for Wander().
    float max_wander_variance = 0.0f;
    float wander_offset =  200.0f;
    float wander_radius = 45.0f;
    Vector3 current_random_point;
    float wander_timer_refresh = 0.0f;

    // float maximum_distance = 10.0f;


    public float speed = 2.0f;

    // Enumerator used for actions
    public enum Actions {WANDER, ARRIVE, FLEE, PURSUE, TAGGED};
    public Actions current_action;

    public bool enemy_area = false;
    public bool has_flag = false;


    void Start() {

        GameObject game_controller_object = GameObject.FindWithTag("GameController");
        if(game_controller_object != null) {
			game_controller = game_controller_object.GetComponent<GameController>();
		}	
		if(game_controller == null) {
			Debug.Log("cant find 'GameController' script");
		}
    }

    void FixedUpdate() {
        if(has_flag) {
            target = GameObject.FindWithTag("BlueBase");
            current_action = Actions.ARRIVE;
        }
        if(current_action == Actions.WANDER) {
            Wander();
        } else if(current_action == Actions.ARRIVE) {
            Arrive();
        } else if(current_action == Actions.FLEE) {
            flee_time -= Time.deltaTime;
            if(flee_time > 0) {
                Flee();            
            } else {
                current_action = Actions.WANDER;
                flee_time = 1.0f;
            }
        } else if(current_action == Actions.PURSUE) {
            Pursue();
        } else if(current_action == Actions.TAGGED) {
            tagged_aura.GetComponent<MeshRenderer>().enabled = true;
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
            GetComponent<Rigidbody>().velocity = (((target.transform.position - transform.position).normalized * near_speed)/time2target);
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
        if(other.gameObject.tag == "Red" && enemy_area) {
            if(has_flag) {
                has_flag = false;
            }
            current_action = Actions.TAGGED;
            game_controller.blue_attacker = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        } else if(other.gameObject.tag == "Red" && has_flag) {
            has_flag = false;
            current_action = Actions.TAGGED;
            game_controller.blue_attacker = false;
            GetComponent<Rigidbody>().velocity = Vector3.zero;
        } else if(other.gameObject.tag == "Red") {
            current_action = Actions.FLEE;
            target = other.gameObject;
            game_controller.red_defender = false;
            game_controller.red_attacker = false;
        } else if(other.gameObject.tag == "Blue" && current_action == Actions.TAGGED) {
            target = GameObject.FindWithTag("RedBase");
            current_action = Actions.FLEE;
            other.gameObject.GetComponent<BlueAIBehavior>().setActions(2);
            tagged_aura.GetComponent<MeshRenderer>().enabled = false;
            game_controller.blue_rescue = false;
        }else {
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
        }
    }
}
