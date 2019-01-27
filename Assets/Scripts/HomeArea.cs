using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeArea : MonoBehaviour
{
    private BlueAIBehavior BlueAIScript;
    private RedAIBehavior RedAIScript;
    public enum Team {RED, BLUE};
    public Team team;

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
