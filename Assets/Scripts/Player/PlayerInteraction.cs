using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Rigidbody rigidBody;
    private PlayerMovement playerMovement;
    private GameObject ropeNodeGameObject;
    private GameObject lastCoinCollected;
    
    [SerializeField] private GameObject lastWallTouched;
    [SerializeField] private GameObject lastWallJumped;
    [SerializeField] private bool isCollidingRight = false;
    [SerializeField] private bool isCollidingLeft = false;
    [SerializeField] private bool isInRope = false;
    [SerializeField] private bool isInGround = true;
    [SerializeField] private bool isSwimming = false;
    [SerializeField] private bool isSliding = false;

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
        if(collider.tag.Contains("Water")) EnterWater();
        if(collider.name.Contains("Trampoline")) EnterTrampoline(collider);
        if(collider.name.Contains("Ground")) EnterGround();
        if(collider.name.Contains("Ground") && collider.name.Contains("Side") == false) {
            SetCleanTouchedJumpedWall();
        }
        if(collider.name.Contains("Coin")) EnterCoin(collider.gameObject);
        if(collider.name.Contains("RopeNode")) EnterRopeNode(collider);
    }

    void OnTriggerStay(Collider collider){
        if(collider.name.Contains("Ground")) isInGround = true;
        if(collider.name.Contains("Side")) StaySideGround(collider);
        if(collider.name.Contains("RightSide")) SetIsCollidingLeft(true, collider.gameObject);
        if(collider.name.Contains("LeftSide")) SetIsCollidingRight(true, collider.gameObject);
    }

    void OnTriggerExit(Collider collider){
        if(collider.tag.Contains("Water")) ExitWater();
        if(collider.name.Contains("Ground")) isInGround = false;
        if(collider.name.Contains("Side")) ExitGroundSide();
    }

    ////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

    void EnterTrampoline(Collider collider){
        TrampolineController trampoline = collider.GetComponent<TrampolineController>();

        SetCleanTouchedJumpedWall();

        if(
            trampoline != null && 
            isInGround == false
        ){
            playerMovement.SetJumpsMade(1);
            rigidBody.velocity = Vector3.zero;
            playerMovement.JumpUp(trampoline.jumpForce);
        }
    }

    void EnterCoin(GameObject coin){
        if(coin != lastCoinCollected){
            if(coin.name.Contains("One")) GameController.AddCoin(1);
            if(coin.name.Contains("Ten")) GameController.AddCoin(10);
            
            lastCoinCollected = coin;
            GameObject.Destroy(coin);
        }
    }

    void EnterGround(){
        isInGround = true;
        playerMovement.SetJumpsMade(0);
    }

    void ExitGroundSide(){
        SetIsCollidingRight(false);
        SetIsCollidingLeft(false);
        SetIsSliding(false);
    }

    void EnterWater(){
        isSwimming = true;
        playerMovement.SetJumpsMade(0);

        rigidBody.useGravity = false;
        rigidBody.velocity = new Vector3(0, playerMovement.GetSwimmingMovement().gravity ,0);
    }   

    void ExitWater(){
        isSwimming = false;
        rigidBody.useGravity = true;
    }  

    void StaySideGround(Collider collider){
        isInGround = true;

        ObstacleBehavior obstacleBehavior = collider.transform.parent.GetComponent<ObstacleBehavior>();
            
        if(rigidBody.velocity.y < 0) {
            isSliding = true;

            if(GetIsCollidingRight()){
                playerMovement.RotatePlayerModel(new Vector2(-1,0));
                
            } else if(GetIsCollidingLeft()){
                playerMovement.RotatePlayerModel(new Vector2(1,0));
            }   

            isSliding = true;
            rigidBody.velocity = new Vector3(0, -obstacleBehavior.slideDownSpeed, 0);

        } else {
            isSliding = false;
        }
    }

    void EnterRopeNode(Collider ropeNodeCollider){

        isInRope = true;

        Collider[] colliders = ropeNodeCollider.transform.parent.GetComponentsInChildren<Collider>();

        foreach(Collider collider in colliders){
            collider.enabled = false;
        }

        ropeNodeGameObject = ropeNodeCollider.gameObject;
        SetCleanTouchedJumpedWall();
        playerMovement.SetJumpsMade(1);

        //transform.SetParent(ropeNodeCollider.transform);
    }

    ////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

    public void SetRigibdodyVelocity(Vector3 velocity){
        rigidBody.velocity = velocity;
    }

    public bool GetIsCollidingRight(){
        return isCollidingRight;
    }

    public void SetIsCollidingRight(bool value, GameObject gameObject = null){
        isCollidingRight = value;

        if(value) SetLastWallTouched(gameObject);
    }

    public bool GetIsCollidingLeft(){
        return isCollidingLeft;
    }

    public void SetIsCollidingLeft(bool value,  GameObject gameObject = null){
        isCollidingLeft = value;

        if(value) SetLastWallTouched(gameObject);
    }

    public GameObject GetLastWallTouched(){
        return lastWallTouched;
    }

    public void SetLastWallTouched(GameObject gameObject){
        lastWallTouched = gameObject.transform.parent.gameObject;
    } 

    public GameObject GetLastlastWallJumped(){
        return lastWallJumped;
    }

    public void SetLastWallJumped(){
        lastWallJumped = lastWallTouched;
    }

    public void SetCleanTouchedJumpedWall(){
        lastWallJumped = null;
        lastWallTouched = null;
    }

    public bool GetIsInRope(){
        return isInRope;
    }

    public void SetIsInRope(bool value){
        isInRope = value;
    }

    public bool GetIsSliding(){
        return isSliding;
    }

    public void SetIsSliding(bool value){
        isSliding = value;
    }

    public bool GetIsInGround(){
        return isInGround;
    }

    public void SetIsInGround(bool value){
        isInGround = value;
    }

    public bool GetIsSwimming(){
        return isSwimming;
    }

    public void SetIsSwimming(bool value){
        isSwimming = value;
    }

    public GameObject GetRopeNode(){
        return ropeNodeGameObject;
    }

    public void SetRopeNode(GameObject newRopeNode){
        ropeNodeGameObject = newRopeNode;
    }
}
