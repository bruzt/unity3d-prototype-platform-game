using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Walking {
    public float moveForce = 1;
    public float maxSpeed = 1;
    public float jumpForce = 1;
    public float horizontalJumpForce = 1;
    public float maxJumpSpeed = 1;
}

[System.Serializable] public class Swimming {
    public float gravity = -1;
    public float jumpForce = 1;
    public float maxJumpSpeed = 1;
    public float swimForce = 1;
    public float maxSwimSpeed = 1;
}

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigidBody; 
    private PlayerInteraction playerInteraction;
    private PlayerAttack playerAttack;
    private float inRopeY;

    [SerializeField] private bool isLookingRight = true;
    
    [SerializeField] private int totalJumps = 2;
    [SerializeField] private int jumpsMade = 0;
    [SerializeField] private Walking walking;
    [SerializeField] private Swimming swimming;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInteraction = GetComponent<PlayerInteraction>(); 
        playerAttack = GetComponent<PlayerAttack>();
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        
    }

    void OnTriggerStay(Collider collider){

    }

    void OnTriggerExit(Collider collider){
        
    }

    ///////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////

    public void Move(Vector2 input){
        if(playerInteraction.GetIsSwimming()){
            rigidBody.AddForce(new Vector3(input.x, input.y, 0) * swimming.swimForce);

            if(rigidBody.position.y > -0.66 /*&& input.y == 0*/) input.y = -0.5f;
            else if(input.y > 0 && rigidBody.position.y > -1) input.y = 0;

            Vector3 maxVelocity = rigidBody.velocity;
            maxVelocity.x = Mathf.Clamp(maxVelocity.x, -swimming.maxSwimSpeed * -input.x, swimming.maxSwimSpeed * input.x);
            maxVelocity.y = Mathf.Clamp(maxVelocity.y, -swimming.maxSwimSpeed * -input.y, swimming.maxSwimSpeed * input.y);
           
            rigidBody.velocity = maxVelocity;

            RotatePlayerModel(input);

        } else {
            input.y = 0;

            rigidBody.AddForce(new Vector3(input.x, input.y, 0) * walking.moveForce);

            if(playerAttack.GetIsAttacking() == false){
                Vector3 maxVelocity = rigidBody.velocity;

                if(playerInteraction.GetIsInGround()){
                    maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed * -input.x, walking.maxSpeed * input.x);
                } else {
                    maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed, walking.maxSpeed);
                }
                
                rigidBody.velocity = maxVelocity;
            }

            RotatePlayerModel(input);
        }
    }

    public void MoveInRope(Vector2 input){
        
        RotatePlayerModel(input);

        transform.position = playerInteraction.GetRopeNode().transform.position;

        Rigidbody ropeNodeRigidbody = playerInteraction.GetRopeNode().GetComponent<Rigidbody>();

        ropeNodeRigidbody.AddForce(new Vector3(input.x, 0, 0) * walking.moveForce/*, ForceMode.Acceleration*/);

        Vector3 maxVelocity = ropeNodeRigidbody.velocity;

        maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed * -input.x, walking.maxSpeed * input.x);
        
        ropeNodeRigidbody.velocity = maxVelocity;

        MoveInRopeY(input.y);
    }

    void MoveInRopeY(float inputY){
        Transform[] nodes = playerInteraction.GetRopeNode().transform.parent.GetComponentsInChildren<Transform>();

        int index = 0;
        int nodesLength = nodes.Length;

        for(int i=0; i < nodesLength; i++){
            if(nodes[i].name == playerInteraction.GetRopeNode().name){
                index = i;
                break;
            } 
        }

        if(inputY > 0 && index > 1 && inRopeY == 0) playerInteraction.SetRopeNode(nodes[index - 1].gameObject);
        else if(inputY < 0 && index < nodesLength - 1 && inRopeY == 0) playerInteraction.SetRopeNode(nodes[index + 1].gameObject);

        inRopeY = inputY;
    }
    
    public void Jump(){
        if(playerInteraction.GetIsSwimming()){
            if(rigidBody.position.y >= -1){
                playerInteraction.SetIsSwimming(false);
                JumpUp(swimming.jumpForce);
            }

        } else if(
            (jumpsMade < totalJumps) && 
            (
                (playerInteraction.GetLastWallTouched() == null) ||
                (playerInteraction.GetLastWallTouched() != playerInteraction.GetLastlastWallJumped())
            )
        ){
            if(playerInteraction.GetIsSwimming()) rigidBody.velocity = Vector3.zero;

            /*if(isSliding && playerInteraction.GetIsCollidingRight()){
                //transform.Translate(0, 0, 0);

                rigidBody.AddForce(-walking.horizontalJumpForce, walking.jumpForce, 0);
                RotatePlayerModel(new Vector2(-1,0));
                
            } else if(isSliding && playerInteraction.GetIsCollidingLeft()){
                //transform.Translate(0, 0, 0);

                rigidBody.AddForce(walking.horizontalJumpForce, walking.jumpForce, 0);
                RotatePlayerModel(new Vector2(1,0));
            }*/
                
            JumpUp();

            jumpsMade++;
        }
    }

    public IEnumerator JumpInRope(){

        if(playerInteraction.GetRopeNode() != null){

            Collider[] colliders = playerInteraction.GetRopeNode().transform.parent.GetComponentsInChildren<Collider>();

            playerInteraction.SetIsInRope(false);

            JumpUp();

            yield return new WaitForSeconds(1);

            foreach(Collider collider in colliders){
                collider.enabled = true;
            }

            yield return null;
        }
    }

    public void JumpUp(){
        JumpUp(walking.jumpForce);
    }

    public void JumpUp(float jumpForce){
        playerInteraction.SetLastWallJumped();

        rigidBody.AddForce(0, jumpForce, 0/*, ForceMode.Acceleration*/);

        Vector3 jumpVelocity = rigidBody.velocity;
        jumpVelocity.y = Mathf.Clamp(jumpVelocity.y, -walking.maxJumpSpeed, walking.maxJumpSpeed);
        jumpVelocity.x = Mathf.Clamp(jumpVelocity.x, -walking.maxJumpSpeed, walking.maxJumpSpeed);
        rigidBody.velocity = jumpVelocity;
    }

    public void RotatePlayerModel(Vector2 input){
        if(input.x > 0){
            isLookingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if(input.x < 0) {
            isLookingRight = false;
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }

    ////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public void SetJumpsMade(int value){
        jumpsMade = value;
    }

    public Swimming GetSwimmingMovement(){
        return swimming;
    }
}
