using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 inputDirection;
    private PlayerMovement movement;

    [SerializeField] private bool isMoving = false;
    [SerializeField] private bool isRopeSwinging = false;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
        isMoving = true;
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection.y = Input.GetAxis("Vertical");
        inputDirection.x = Input.GetAxis("Horizontal");

        if(isMoving){

            if(Input.GetButtonDown("Jump")) movement.Jump();
        
            movement.Move(inputDirection);

        } else if(isRopeSwinging){

            if(Input.GetButtonDown("Jump")) StartCoroutine(movement.JumpInRope());

            movement.MoveInRope(inputDirection);
        }
    }

    public bool GetIsMoving(){
        return isMoving;
    }

    public void SetIsMoving(bool value){
        isMoving = value;
        isRopeSwinging = !value;
    }

    public void SetIsRopeSwinging(bool value){
        isRopeSwinging = value;
        isMoving = !value;
    }

    public bool GetIsRopeSwinging(){
        return isRopeSwinging;
    }
}
