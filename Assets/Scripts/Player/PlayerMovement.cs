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
    private GameObject ropeNodeGameObject;
    private float inRopeY;

    [SerializeField] private bool isLookingRight = true;
    [SerializeField] private bool isInGround = true;
    [SerializeField] private bool isSliding = false;
    [SerializeField] private bool isSwimming = false;
    
    [SerializeField] private int totalJumps = 2;
    [SerializeField] private int jumpsMade = 0;
    [SerializeField] private Walking walking;
    [SerializeField] private Swimming swimming;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        playerInteraction = GetComponent<PlayerInteraction>(); 
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    void FixedUpdate()
    {
        
    }

    void OnTriggerEnter(Collider collider){
        if(collider.tag.Contains("Water")) EnterWater();
    }

    void OnTriggerStay(Collider collider){

    }

    void OnTriggerExit(Collider collider){
        if(collider.tag.Contains("Water")) ExitWater();
    }

    ///////////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////////

    public void Move(Vector2 input){
        if(isSwimming){
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

            Vector3 maxVelocity = rigidBody.velocity;

            if(isInGround){
                maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed * -input.x, walking.maxSpeed * input.x);
            } else {
                maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed, walking.maxSpeed);
            }
            
            rigidBody.velocity = maxVelocity;

            RotatePlayerModel(input);
        }
    }

    public void MoveInRope(Vector2 input){
        
        RotatePlayerModel(input);

        transform.position = ropeNodeGameObject.transform.position;

        Rigidbody ropeNodeRigidbody = ropeNodeGameObject.GetComponent<Rigidbody>();

        ropeNodeRigidbody.AddForce(new Vector3(input.x, 0, 0) * walking.moveForce/*, ForceMode.Acceleration*/);

        Vector3 maxVelocity = ropeNodeRigidbody.velocity;

        maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed * -input.x, walking.maxSpeed * input.x);
        
        ropeNodeRigidbody.velocity = maxVelocity;

        MoveInRopeY(input.y);
    }

    void MoveInRopeY(float inputY){
        Transform[] nodes = ropeNodeGameObject.transform.parent.GetComponentsInChildren<Transform>();

        int index = 0;
        int nodesLength = nodes.Length;

        for(int i=0; i < nodesLength; i++){
            if(nodes[i].name == ropeNodeGameObject.name){
                index = i;
                break;
            } 
        }

        if(inputY > 0 && index > 1 && inRopeY == 0) ropeNodeGameObject = nodes[index - 1].gameObject;
        else if(inputY < 0 && index < nodesLength - 1 && inRopeY == 0) ropeNodeGameObject = nodes[index + 1].gameObject;

        inRopeY = inputY;
    }
    
    public void Jump(){
        if(isSwimming){
            if(rigidBody.position.y >= -1){
                isSwimming = false;
                JumpUp(swimming.jumpForce);
            }

        } else if(
            (jumpsMade < totalJumps) && 
            (
                (playerInteraction.GetLastWallTouched() == null) ||
                (playerInteraction.GetLastWallTouched() != playerInteraction.GetLastlastWallJumped())
            )
        ){
            if(isSliding) rigidBody.velocity = Vector3.zero;

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

        if(ropeNodeGameObject != null){

            Collider[] colliders = ropeNodeGameObject.transform.parent.GetComponentsInChildren<Collider>();

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

    private void RotatePlayerModel(Vector2 input){
        if(input.x > 0){
            isLookingRight = true;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        } else if(input.x < 0) {
            isLookingRight = false;
            transform.rotation = Quaternion.Euler(0, -180, 0);
        }
    }

    public void EnterGround(){
        isInGround = true;
        jumpsMade = 0;
    }

    public void StaySideGround(Collider collider){
        isInGround = true;

        ObstacleBehavior obstacleBehavior = collider.transform.parent.GetComponent<ObstacleBehavior>();
            
        if(rigidBody.velocity.y < 0) {
            isSliding = true;

            if(playerInteraction.GetIsCollidingRight()){
                RotatePlayerModel(new Vector2(-1,0));
                
            } else if(playerInteraction.GetIsCollidingLeft()){
                RotatePlayerModel(new Vector2(1,0));
            }   

            isSliding = true;
            rigidBody.velocity = new Vector3(0, -obstacleBehavior.slideDownSpeed, 0);

        } else {
            isSliding = false;
        }
    }

    void EnterWater(){
        isSwimming = true;
        jumpsMade = 0;

        rigidBody.useGravity = false;
        rigidBody.velocity = new Vector3(0, swimming.gravity ,0);
    }   

    void ExitWater(){
        isSwimming = false;
        rigidBody.useGravity = true;
    }  

    ////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////

    public bool GetIsSliding(){
        return isSliding;
    }

    public void SetIsSliding(bool value){
        isSliding = value;
    }

    public void SetIsInGround(bool value){
        isInGround = value;
    }

    public void SetRopeNode(GameObject node){
        ropeNodeGameObject = node;
    }

    public bool GetIsInGround(){
        return isInGround;
    }

    public void SetJumpsMade(int value){
        jumpsMade = value;
    }
}
