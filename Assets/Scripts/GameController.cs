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
        GameObject enemy_target = null;
        foreach (GameObject player in red_team) {
            if(player.GetComponent<RedAIBehavior>().currentAction() == (int) Movement.PURSUE) {
                red_defender = true;
            }
        }
        // foreach (GameObject player in blue_team) {
        //     if(player.GetComponent<BlueAIBehavior>().currentAction() == (int) Movement.PURSUE) {
        //         blue_defender = true;
        //     }
        // }
        foreach (GameObject target in blue_team) {
            if(target.GetComponent<BlueAIBehavior>().enemy_area) {
                enemy_target = target;
            }
        }
        if(!red_defender && enemy_target != null) {
            Debug.Log("enemy target in the area!");
            int index = Random.Range(0,3);
            red_script = red_team[index].GetComponent<RedAIBehavior>();
            if(red_script.currentAction() == (int) Movement.WANDER) {
                red_script.setActions(3);
                red_script.target = enemy_target;
            }
        }
    }
}
