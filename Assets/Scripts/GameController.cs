using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject[] red_team;
    public GameObject[] blue_team;
    public GameObject blue_flag;
    public GameObject red_flag;
    private RedAIBehavior red_script;
    private BlueAIBehavior blue_script;

    enum Movement {WANDER, ARRIVE, FLEE, PURSUE, TAGGED}
    public GameObject red_tester;
    public GameObject blue_tester;

    bool red_attacker = false;
    bool blue_attacker = false;

    bool red_defender = false;
    bool blue_defender = false;

    public bool red_flag_captured = false;
    public bool blue_flag_captured = false;
    // Update is called once per frame
    void Update()
    {
        Attacker();
        Defender();
    }
    // =================================================================
    // Function to select an attacker and to see if there is already one.
    // =================================================================
    void Attacker() {
        foreach (GameObject player in red_team) {
            if(player.GetComponent<RedAIBehavior>().currentAction() == (int) Movement.ARRIVE) {
                red_attacker = true;
            }
        }
        foreach (GameObject player in blue_team) {
            if(player.GetComponent<BlueAIBehavior>().currentAction() == (int) Movement.ARRIVE) {
                blue_attacker = true;
            }
        }
        // Select a player from RED team to attack
        if(!red_attacker) {
             int index = Random.Range(0,3);
            red_script = red_team[index].GetComponent<RedAIBehavior>();
            if(red_script.currentAction() == (int) Movement.WANDER) {
                red_script.setActions(1);
                red_script.target = blue_flag;
            }
        }
        // Select a player from BLUE team to attack
        if(!blue_attacker) {
            int index = Random.Range(0,3);
            blue_script = blue_team[index].GetComponent<BlueAIBehavior>();
            if(blue_script.currentAction() == (int) Movement.WANDER) {
                blue_script.setActions(1);
                blue_script.target = red_flag;
            }
        }
    }

    // =================================================================
    // Function to select a defender who IS NOT an attacker already.
    // =================================================================
    void Defender() {
        GameObject red_enemy_target = null;
        GameObject blue_enemy_target = null;
        // Check if there is already a pursuer for RED team
        foreach (GameObject player in red_team) {
            if(player.GetComponent<RedAIBehavior>().currentAction() == (int) Movement.PURSUE) {
                red_defender = true;
            }
        }

        // Check if there is already a pursuer for BLUE team
        foreach (GameObject player in blue_team) {
            if(player.GetComponent<BlueAIBehavior>().currentAction() == (int) Movement.PURSUE) {
                blue_defender = true;
            }
        }
        // Check if there is a BLUE player in the RED home area
        foreach (GameObject target in blue_team) {
            if(target.GetComponent<BlueAIBehavior>().enemy_area) {
                blue_enemy_target = target;
            }
        }
        // Check if there is a RED player in the BLUE home area
        foreach (GameObject target in red_team) {
            if(target.GetComponent<RedAIBehavior>().enemy_area) {
                red_enemy_target = target;
            }
        }

        // Set the Pursue action to the RED player randomly selected
        if(!red_defender && blue_enemy_target != null) {
            int index = Random.Range(0,3);
            red_script = red_team[index].GetComponent<RedAIBehavior>();
            if(red_script.currentAction() == (int) Movement.WANDER) {
                red_script.setActions(3);
                red_script.target = blue_enemy_target;
            }
        }
        // Set the Pursue action to the BLUE player randomly selected
        if(!blue_defender && red_enemy_target != null) {
            int index = Random.Range(0,3);
            blue_script = blue_team[index].GetComponent<BlueAIBehavior>();
            if(blue_script.currentAction() == (int) Movement.WANDER) {
                blue_script.setActions(3);
                blue_script.target = red_enemy_target;
            }
        }
    }
}
