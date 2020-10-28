using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private Transform barrier;
    
    private void OnTriggerEnter(Collider other)
    {
       
        barrier = other.gameObject.GetComponent<Transform>();
        other.enabled = false;

        if (gameObject.CompareTag("X Barrier"))
        {
            barrier.position = new Vector3(-barrier.position.x + 1, barrier.position.y, barrier.position.z);
            
        }else if (gameObject.CompareTag("-X Barrier"))
        { 
            barrier.position = new Vector3(-barrier.position.x - 1, barrier.position.y, barrier.position.z);
            
        }else if (gameObject.CompareTag("Z Barrier"))
        {
            barrier.position = new Vector3(barrier.position.x, barrier.position.y, -barrier.position.z + 1);
        }else if (gameObject.CompareTag("-Z Barrier"))
        {
            barrier.position = new Vector3(barrier.position.x, barrier.position.y, - barrier.position.z - 1);
        }
        other.enabled = true;
    }
}
