using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private PlayerMovement playerMovement;
    private PlayerInteraction playerInteraction;
    private PlayerLife playerLife;
    private Vector2 inputDirection;
    private bool inputJump;

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerLife = GetComponent<PlayerLife>();
    }

    // Update is called once per frame
    void Update()
    {
        if(playerLife.GetIsAlive()){

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
}
