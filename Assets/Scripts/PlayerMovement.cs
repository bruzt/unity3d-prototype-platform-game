using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] public class CollisionPlayer {
    public float right;
    public float left;
}

[System.Serializable] public class Walking {
    public float moveForce = 1;
    public float maxSpeed = 1;
    public float jumpForce = 1;
    public float horizontalJumpForce = 1;
    public float maxJumpSpeed = 1;
    public float slideDownSpeed = 1;
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
    private GameObject ropeSegment;

    public Transform playerModelTransform;
    public bool isLookingRight = true;
    public bool isInGround = true;
    public bool isSliding = false;
    public bool isSwimming = false;
    public bool isCollidingRight = false;
    public bool isCollidingLeft = false;
    public int totalJumps = 2;
    public int jumpsMade = 0;
    public CollisionPlayer collisionDistance;
    public Walking walking;
    public Swimming swimming;

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
        //DetectHorizontalCollision();
    }

    void OnTriggerEnter(Collider collider){
        if(collider.tag.Contains("Ground")) OnTriggerEnterGround();
        if(collider.tag.Contains("Water")) OnTriggerEnterWater();
        if(collider.name.Contains("Rope")) rigidBody.useGravity = false;
    }

    void OnTriggerStay(Collider collider){
        if(collider.tag == "Ground") OnTriggerStayGround();
    }

    void OnTriggerExit(Collider collider){
        if(collider.tag.Contains("Ground")) OnTriggerExitGround();
        if(collider.tag.Contains("Water")) OnTriggerExitWater();
        if(collider.name.Contains("Rope")) SetLeaveRope();
    }

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
        
        if(ropeSegment != null){

            Rigidbody ropeSegmentRigidbody = ropeSegment.GetComponent<Rigidbody>();

            ropeSegmentRigidbody.AddForce(new Vector3(input.x, 0, 0) * walking.moveForce, ForceMode.Acceleration);

            Vector3 maxVelocity = ropeSegmentRigidbody.velocity;

            if(isInGround){
                maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed * -input.x, walking.maxSpeed * input.x);
            } else {
                maxVelocity.x = Mathf.Clamp(maxVelocity.x, -walking.maxSpeed, walking.maxSpeed);
            }
            
            maxVelocity.y = 0;
            ropeSegmentRigidbody.velocity = maxVelocity;
        }

        RotatePlayerModel(input);
    }

    void OnTriggerEnterGround(){
        isInGround = true;
        jumpsMade = 0;
    }

    void OnTriggerStayGround(){
        isInGround = true;
            
        if(rigidBody.velocity.y < 0) {
            if(isCollidingRight){
                RotatePlayerModel(new Vector2(-1,0));
                
            } else if(isCollidingLeft){
                RotatePlayerModel(new Vector2(1,0));
            }   

            isSliding = true;
            rigidBody.velocity = new Vector3(0, -walking.slideDownSpeed, 0);

        } else {
            isSliding = false;
        }
    }

    void OnTriggerExitGround(){
        isInGround = false;
        isCollidingRight = false;
        isCollidingLeft = false;
        isSliding = false;
        playerInteraction.lastWallTouched = null;
        playerInteraction.wallJumped = null;
    }

    private void RotatePlayerModel(Vector2 input){
        if(input.x > 0){
            isLookingRight = true;
            playerModelTransform.rotation = Quaternion.Euler(0, 0, 0);
        } else if(input.x < 0) {
            isLookingRight = false;
            playerModelTransform.rotation = Quaternion.Euler(0, -180, 0);
        }
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
                (playerInteraction.lastWallTouched != playerInteraction.wallJumped) ||
                (playerInteraction.lastWallTouched == null)
            )
        ){
            if(isSliding) rigidBody.velocity = Vector3.zero;

            /*if(isSliding && isCollidingRight){
                //transform.Translate(0, 0, 0);

                //rigidBody.AddForce(-walking.horizontalJumpForce, walking.jumpForce, 0);
                RotatePlayerModel(new Vector2(-1,0));
                
            } else if(isSliding && isCollidingLeft){
                //transform.Translate(0, 0, 0);

                //rigidBody.AddForce(walking.horizontalJumpForce, walking.jumpForce, 0);
                RotatePlayerModel(new Vector2(1,0));
            } */
                
            JumpUp(walking.jumpForce);

            jumpsMade++;
        }
    }

    public IEnumerator JumpInRope(){

        if(ropeSegment != null){

            SetLeaveRope();

            SphereCollider[] sphereColliders = ropeSegment.transform.parent.GetComponentsInChildren<SphereCollider>();

            foreach(SphereCollider sphereCollider in sphereColliders){
                sphereCollider.enabled = false;
            }

            GetComponentInParent<PlayerController>().SetIsRopeSwinging(false);
        
            SetIsCollidingWithRopeLeft(false);
            SetIsCollidingWithRopeRight(false);

            JumpUp(walking.jumpForce);

            yield return new WaitForSeconds(1);

            foreach(SphereCollider sphereCollider in sphereColliders){
                sphereCollider.enabled = true;
            }

            yield return null;
        }
    }

    public void JumpUp(float jumpForce){
        playerInteraction.wallJumped = playerInteraction.lastWallTouched;

        rigidBody.AddForce(0, jumpForce, 0/*, ForceMode.Acceleration*/);

        Vector3 jumpVelocity = rigidBody.velocity;
        jumpVelocity.y = Mathf.Clamp(jumpVelocity.y, -walking.maxJumpSpeed, walking.maxJumpSpeed);
        jumpVelocity.x = Mathf.Clamp(jumpVelocity.x, -walking.maxJumpSpeed, walking.maxJumpSpeed);
        rigidBody.velocity = jumpVelocity;
    }

    private void DetectHorizontalCollision(){
        /*Vector3 upPosition = new Vector3(transform.position.x, transform.position.y + 0.9f, transform.position.z);
        Vector3 downPosition = new Vector3(transform.position.x, transform.position.y - 0.9f, transform.position.z);

        Debug.DrawRay(transform.position, Vector3.right * 0.80f, Color.red);
        Debug.DrawRay(upPosition, Vector3.right * 0.80f, Color.red);
        Debug.DrawRay(downPosition, Vector3.right * 0.80f, Color.red);

        if (Physics.Raycast(transform.position, Vector3.right, 0.4569f) ||
            Physics.Raycast(upPosition, Vector3.right, 0.4569f) ||
            Physics.Raycast(downPosition, Vector3.right, 0.4569f)
        ){
            isCollidingRight = true;
        }

        if (Physics.Raycast(transform.position, Vector3.left, 0.4569f) ||
            Physics.Raycast(upPosition, Vector3.left, 0.4569f) ||
            Physics.Raycast(downPosition, Vector3.left, 0.4569f)
        ){
            isCollidingLeft = true;
        }*/

        RaycastHit hit;
        Ray rightRay = new Ray(transform.position, Vector3.right);
        Ray leftRay = new Ray(transform.position, Vector3.left);

        //Debug.DrawRay(transform.position, Vector3.right);
        //Debug.DrawRay(transform.position, Vector3.left);

        if(Physics.Raycast(rightRay, out hit, collisionDistance.right)){
            //if(hit.collider.tag == "Ground"){
                isCollidingRight = true;
            //}
        
        } 
        
        if(Physics.Raycast(leftRay, out hit, collisionDistance.left)){
            //if(hit.collider.tag == "Ground"){
                isCollidingLeft = true;
            //}
        }
    }

    void OnTriggerEnterWater(){
        isSwimming = true;
        jumpsMade = 0;

        rigidBody.useGravity = false;
        rigidBody.velocity = new Vector3(0 ,swimming.gravity ,0);
    }   

    void OnTriggerExitWater(){
        isSwimming = false;
        rigidBody.useGravity = true;
    }  

    public void SetIsCollidingWithRopeLeft(bool value, GameObject ropeSegmentGameObject = null){
        isCollidingLeft = value;

        if(value) {
            //rigidBody.AddForce(-100,0,0);
            //transform.SetParent(ropeSegmentGameObject.transform);
            ropeSegment = ropeSegmentGameObject;
        }
    }

    public void SetIsCollidingWithRopeRight(bool value, GameObject ropeSegmentGameObject = null){
        isCollidingRight = value;

        if(value) {
            //rigidBody.AddForce(100,0,0);
            //transform.SetParent(ropeSegmentGameObject.transform);
            ropeSegment = ropeSegmentGameObject;
        }
    }

    void SetLeaveRope(){
        rigidBody.useGravity = true;
        GetComponentInParent<PlayerController>().SetIsRopeSwinging(false);
        SetIsCollidingWithRopeLeft(false);
        SetIsCollidingWithRopeRight(false);
    }
}
