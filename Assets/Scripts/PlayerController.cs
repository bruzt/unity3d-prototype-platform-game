using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 inputDirection;
    private bool inputJump;
    private PlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection.x = Input.GetAxis("Horizontal");
        inputJump = Input.GetButtonDown("Jump");

        if(playerInteraction.GetIsInRope()){

            inputDirection.y = Input.GetAxisRaw("Vertical");
            
            if(inputJump) StartCoroutine(playerMovement.JumpInRope());

            playerMovement.MoveInRope(inputDirection);

        } else {

            inputDirection.y = Input.GetAxis("Vertical");

            if(inputJump) playerMovement.Jump();
        
            playerMovement.Move(inputDirection);
        }
    }
}
