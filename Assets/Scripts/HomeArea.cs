using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// =================================================================
// This class handle the home area of each team which is selected
// in the Scene view. It will handle any character that enter enemy
// area and exit enemy area but flipping the boolean variable.
// =================================================================
public class HomeArea : MonoBehaviour
{
    private BlueAIBehavior BlueAIScript;
    private RedAIBehavior RedAIScript;
    public enum Team {RED, BLUE};
    public Team team;
    // =================================================================
    // OnTriggerEnter for when a character enters the enemy area
    // =================================================================
    void OnTriggerEnter(Collider other) {
        if(team == Team.RED) {
            if(other.gameObject.tag == "Blue") {
                BlueAIScript = other.GetComponent<BlueAIBehavior>();
                BlueAIScript.enemy_area = true;
            }
        } else if(team == Team.BLUE) {
            if(other.gameObject.tag == "Red") {
                RedAIScript = other.GetComponent<RedAIBehavior>();
                RedAIScript.enemy_area = true;
            }
        } else {
            return;
        }
    }
    // =================================================================
    // OnTriggerExit for when a character exits the enemy area.
    // =================================================================
    void OnTriggerExit(Collider other) {
        if(team ==Team.RED) {
            if(other.gameObject.tag == "Blue") {
                BlueAIScript = other.GetComponent<BlueAIBehavior>();
                if(BlueAIScript.has_flag == false) {
                    BlueAIScript.enemy_area = false;
                }
            } 
        } else if(team == Team.BLUE) {
            if(other.gameObject.tag == "Red") {
                RedAIScript = other.GetComponent<RedAIBehavior>();
                if(RedAIScript.has_flag == false) {
                    RedAIScript.enemy_area = false;                
                }
            }
        } else {
            return;
        }    
    }
}
