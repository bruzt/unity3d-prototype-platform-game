using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    private Rigidbody rigidBody;
    private PlayerMovement playerMovement;
    [SerializeField] private GameObject lastWallTouched;
    [SerializeField] private GameObject lastWallJumped;
    [SerializeField] private bool isCollidingRight = false;
    [SerializeField] private bool isCollidingLeft = false;

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
        if(collider.name.Contains("Trampoline")) EnterTrampoline(collider);
        if(collider.name.Contains("Ground")) playerMovement.EnterGround();
        if(collider.name.Contains("Ground") && collider.name.Contains("Side") == false) {
            SetCleanTouchedJumpedWall();
        }
        if(collider.name.Contains("Coin")) CoinCollision(collider.gameObject);
    }

    void OnTriggerStay(Collider collider){
        if(collider.name.Contains("Side")) playerMovement.StaySideGround(collider);
        if(collider.name.Contains("RightSide")) SetIsCollidingLeft(true, collider.gameObject);
        if(collider.name.Contains("LeftSide")) SetIsCollidingRight(true, collider.gameObject);
    }

    void OnTriggerExit(Collider collider){
        if(collider.name.Contains("Ground")) playerMovement.SetIsInGround(false);
        if(collider.name.Contains("Side")) ExitGroundSide();
    }

    ////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

    void EnterTrampoline(Collider collider){
        TrampolineController trampoline = collider.GetComponent<TrampolineController>();

        if(
            trampoline != null && 
            playerMovement.isInGround == false
        ){
            rigidBody.velocity = Vector3.zero;
            playerMovement.JumpUp(trampoline.jumpForce);
        }
    }

    public void CoinCollision(GameObject coin){
        if(coin.name.Contains("One")) GameController.AddCoin(1);
        if(coin.name.Contains("Ten")) GameController.AddCoin(10);
        
        GameObject.Destroy(coin);
    }

    void ExitGroundSide(){
        isCollidingRight = false;
        isCollidingLeft = false;
        playerMovement.SetIsSliding(false);
        //playerInteraction.SetCleanTouchedJumpedWall();
    }

    ////////////////////////////////////////////////////////////////////////////////////
    ////////////////////////////////////////////////////////////////////////////////////

    public void SetRigibdodyVelocity(Vector3 velocity){
        rigidBody.velocity = velocity;
    }

    public bool GetIsCollidingRight(){
        return isCollidingRight;
    }

    public void SetIsCollidingRight(bool value, GameObject gameObject){
        isCollidingRight = value;

        if(value) SetLastWallTouched(gameObject);
    }

    public bool GetIsCollidingLeft(){
        return isCollidingLeft;
    }

    public void SetIsCollidingLeft(bool value,  GameObject gameObject){
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
}
