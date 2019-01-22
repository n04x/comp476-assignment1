using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIRespawn : MonoBehaviour
{
    public bool x_axis;
    public float offset;
    void OnTriggerEnter(Collider other) {
        if(x_axis) {
            other.transform.position = new Vector3(-other.transform.position.x + offset, other.transform.position.y, other.transform.position.z);
        } else
        {
            other.transform.position = new Vector3(other.transform.position.x, other.transform.position.y, -other.transform.position.z + offset);
            
        }
    }
}
