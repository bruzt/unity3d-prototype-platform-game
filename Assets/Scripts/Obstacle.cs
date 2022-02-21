using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        PlayerInteraction playerInteraction;
        playerInteraction = collider.gameObject.GetComponentInParent<PlayerInteraction>();

        if(playerInteraction != null){
            if(collider.name == "RightCollider") playerInteraction.SetCollidingRight(this.gameObject);
            else if(collider.name == "LeftCollider") playerInteraction.SetCollidingLeft(this.gameObject);
        }
    }

    void OnTriggerStay(Collider collider){
        
    }

    void OnTriggerExit(Collider collider){
        
    }
}
