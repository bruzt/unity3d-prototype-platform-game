using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Rigidbody rigidBody;
    private PlayerMovement playerMovement;

    public GameObject lastWallTouched;
    public GameObject wallJumped;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if(collider.tag == "Trampoline") OnTriggerEnterTrampoline(collider);
    }

    void OnTriggerStay(Collider collider){
        
    }

    void OnTriggerExit(Collider collider){

    }

    public void SetCollidingRight(GameObject gameObject){
        playerMovement.isCollidingRight = true;
        lastWallTouched = gameObject;
    }

    public void SetCollidingLeft(GameObject gameObject){
        playerMovement.isCollidingLeft = true;
        lastWallTouched = gameObject;
    }

    void OnTriggerEnterTrampoline(Collider collider){
        TrampolineController trampoline = collider.GetComponent<TrampolineController>();

        if(
            trampoline != null && 
            collider.isTrigger && 
            playerMovement.isInGround == false
        ){
            rigidBody.velocity = Vector3.zero;
            playerMovement.JumpUp(trampoline.jumpForce);
        }
    }

    public void CoinCollision(GameObject coin){
        if(coin.tag.Contains("One")) GameController.AddCoin(1);
        
        GameObject.Destroy(coin);
    }

    public void SetRigibdodyVelocity(Vector3 velocity){
        rigidBody.velocity = velocity;
    }
}
