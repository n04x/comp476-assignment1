using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    
    bool red_tagged = false;
    bool blue_tagged = false;
    
    // variable used for Attacker().
    public bool red_attacker = false;
    public bool blue_attacker = false;

    // public bool red_rescue = false;
    // public bool blue_rescue = false;

    // variables used for Defender().
    public bool red_defender = false;
    public bool blue_defender = false;

    public bool red_flag_captured = false;
    public bool blue_flag_captured = false;

    // variable used for GameOver().
    public Text game_over_text;
    float restart_timer = 5.0f;
    public bool game_over = false;
    public bool blue_win = false;
    public bool red_win = false;
    // Update is called once per frame
    void Update()
    {
        Attacker();
        Defender();
        if(game_over) {
            GameOver();
        }
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
            if(target.GetComponent<BlueAIBehavior>().enemy_area && target.GetComponent<BlueAIBehavior>().currentAction() == (int) Movement.ARRIVE) {
                blue_enemy_target = target;
            }
        }
        // Check if there is a RED player in the BLUE home area
        foreach (GameObject target in red_team) {
            if(target.GetComponent<RedAIBehavior>().enemy_area && target.GetComponent<BlueAIBehavior>().currentAction() == (int) Movement.ARRIVE) {
                red_enemy_target = target;
            }
        }

        // Set the Pursue action to the RED player randomly selected
        if(!red_defender && blue_enemy_target != null) {
            int index = Random.Range(0,3);
            red_script = red_team[index].GetComponent<RedAIBehavior>();
            if(red_script.currentAction() == (int) Movement.WANDER && !red_script.has_flag) {
                red_script.setActions(3);
                red_script.target = blue_enemy_target;
            }
        }
        // Set the Pursue action to the BLUE player randomly selected
        if(!blue_defender && red_enemy_target != null) {
            int index = Random.Range(0,3);
            blue_script = blue_team[index].GetComponent<BlueAIBehavior>();
            if(blue_script.currentAction() == (int) Movement.WANDER && !blue_script.has_flag) {
                blue_script.setActions(3);
                blue_script.target = red_enemy_target;
            }
        }
    }

    void Rescue() {
        int tagged_counter = 0;
        List<GameObject> tagged_red_player = new List<GameObject>();
        GameObject rescue_target = null;
        foreach (GameObject player in red_team) {
            if(player.GetComponent<RedAIBehavior>().currentAction() == (int) Movement.TAGGED) {
                tagged_counter++;
                tagged_red_player.Add(player);
            }
        }
        if(tagged_counter > 0 && tagged_counter < 3) {
            red_tagged = true;
        }
        if(red_tagged) {
            int index = Random.Range(0,3);
            float closest_distance = float.PositiveInfinity;
            red_script = red_team[index].GetComponent<RedAIBehavior>();
            if(red_script.currentAction() == (int) Movement.WANDER) {
                foreach (GameObject tagged_player in tagged_red_player) {
                    if((tagged_player.transform.position - red_team[index].transform.position).magnitude < closest_distance) {
                        closest_distance = (tagged_player.transform.position - red_team[index].transform.position).magnitude;
                        rescue_target = tagged_player;
                    }
                }
                red_script.setActions(1);
                red_script.target = rescue_target;
            }
        }
    }
    // team 1 = blue and team 2 = red.
    public void GameOver() {
        restart_timer -= Time.deltaTime;
        if(restart_timer < 0) {
            Application.LoadLevel(Application.loadedLevel);
        }
        if(blue_win) {
            // blue team win
            game_over_text.text = "Blue team win! game restart in: " + (int) restart_timer;
        } else if(red_win) {
            // red team win
            game_over_text.text = "Red team win! game restart in: " + (int) restart_timer;

        }
    }
}
