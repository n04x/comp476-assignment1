using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinningCondition : MonoBehaviour
{
    public enum Team {RED, BLUE}
    public Team team;
    private GameController game_controller;

    void Start() {
        GameObject game_controller_object = GameObject.FindWithTag("GameController");
        if(game_controller_object != null) {
			game_controller = game_controller_object.GetComponent<GameController>();
		}	
		if(game_controller == null) {
			Debug.Log("cant find 'GameController' script");
		}
    }

    private void OnTriggerEnter(Collider other) {
        if(team == Team.BLUE) {
            if(other.gameObject.tag == "Blue" && other.gameObject.GetComponent<BlueAIBehavior>().has_flag) {
                Debug.Log("blue win");
                game_controller.game_over = true;
                game_controller.blue_win = true;
            } else {
                return;
            }
        } else if(team == Team.RED) {
            if(other.gameObject.tag == "Red" && other.gameObject.GetComponent<RedAIBehavior>().has_flag) {
                Debug.Log("red win!");
                game_controller.game_over = true;
                game_controller.red_win = true;
            } else {
                return;
            }
        }
    }
}
