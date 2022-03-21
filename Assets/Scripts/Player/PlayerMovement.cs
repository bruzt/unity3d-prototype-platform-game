using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class Walking {
    public float moveForce = 1;
    public float maxSpeed = 1;
    public float runMultiplier = 1.5f;
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
    private Rigidbody playerRigidbody; 
    private PlayerInteraction playerInteraction;
    private PlayerAttack playerAttack;
    private float inRopeY;
    private bool isLookingRight = true;

    [SerializeField] private int totalJumps = 1;
    [SerializeField] private int jumpsMade = 0;
    [SerializeField] private Walking walking;
    [SerializeField] private Swimming swimming;

    // Start is called before the first frame update
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody>();
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

    public void Move(Vector2 input, bool isRunning = false){
        if(playerInteraction.GetIsSwimming()){
            playerRigidbody.AddForce(new Vector3(input.x, input.y, 0) * swimming.swimForce);

            if(playerRigidbody.position.y > -0.66 /*&& input.y == 0*/) input.y = -0.5f;
            else if(input.y > 0 && playerRigidbody.position.y > -1) input.y = 0;

            Vector3 maxVelocity = playerRigidbody.velocity;
            maxVelocity.x = Mathf.Clamp(maxVelocity.x, -swimming.maxSwimSpeed * -input.x, swimming.maxSwimSpeed * input.x);
            maxVelocity.y = Mathf.Clamp(maxVelocity.y, -swimming.maxSwimSpeed * -input.y, swimming.maxSwimSpeed * input.y);
           
            playerRigidbody.velocity = maxVelocity;

            RotatePlayerModel(input);

        } else {
            input.y = 0;

            playerRigidbody.AddForce(new Vector3(input.x, input.y, 0) * walking.moveForce);

            if(playerAttack.GetIsDashing() == false){
                float finalmaxSpeed = (isRunning) ? walking.maxSpeed * walking.runMultiplier : walking.maxSpeed;
                Vector3 maxVelocity = playerRigidbody.velocity;

                if(playerInteraction.GetIsInGround()){
                    maxVelocity.x = Mathf.Clamp(maxVelocity.x, -finalmaxSpeed * -input.x, finalmaxSpeed * input.x);
                } else {
                    maxVelocity.x = Mathf.Clamp(maxVelocity.x, -finalmaxSpeed, finalmaxSpeed);
                }
                
                playerRigidbody.velocity = maxVelocity;
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
            if(playerRigidbody.position.y >= -1){
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
            if(playerRigidbody.velocity.y < 0) playerRigidbody.velocity = new Vector3(playerRigidbody.velocity.x, 0, 0);

            /*if(isSliding && playerInteraction.GetIsCollidingRight()){
                //transform.Translate(0, 0, 0);

                playerRigidbody.AddForce(-walking.horizontalJumpForce, walking.jumpForce, 0);
                RotatePlayerModel(new Vector2(-1,0));
                
            } else if(isSliding && playerInteraction.GetIsCollidingLeft()){
                //transform.Translate(0, 0, 0);

                playerRigidbody.AddForce(walking.horizontalJumpForce, walking.jumpForce, 0);
                RotatePlayerModel(new Vector2(1,0));
            }*/
                
            JumpUp();

            jumpsMade++;
        }
    }

    public void JumpInRope(){

        playerInteraction.DetachFromRope();
        
        JumpUp();
    }

    public void JumpUp(){
        JumpUp(walking.jumpForce);
    }

    public void JumpUp(float jumpForce){
        playerInteraction.SetLastWallJumped();

        playerRigidbody.AddForce(0, jumpForce, 0/*, ForceMode.Acceleration*/);

        Vector3 jumpVelocity = playerRigidbody.velocity;
        jumpVelocity.y = Mathf.Clamp(jumpVelocity.y, -walking.maxJumpSpeed, walking.maxJumpSpeed);
        jumpVelocity.x = Mathf.Clamp(jumpVelocity.x, -walking.maxJumpSpeed, walking.maxJumpSpeed);
        playerRigidbody.velocity = jumpVelocity;
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

    public bool GetIsWalking(){
        return playerRigidbody.velocity.x != 0 && playerInteraction.GetIsInGround();
    }

    public bool GetIsRising(){
        return playerRigidbody.velocity.y > 0 && playerInteraction.GetIsInGround() == false;
    }

    public bool GetIsFalling(){
        return playerRigidbody.velocity.y < 0 && playerInteraction.GetIsInGround() == false;
    }

    public bool GetIsJumping(){
        return GetIsRising() || GetIsFalling();
    }

    public void SetTotalJumps(int jumps){
        totalJumps = jumps;
    }

    public bool GetIsLookingRight(){
        return isLookingRight;
    }
}
