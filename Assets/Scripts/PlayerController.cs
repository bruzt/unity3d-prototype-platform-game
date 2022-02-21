using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector2 inputDirection;
    private PlayerMovement movement;

    // Start is called before the first frame update
    void Start()
    {
        movement = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection.y = Input.GetAxis("Vertical");
        inputDirection.x = Input.GetAxis("Horizontal");

        //Debug.Log(inputDirection);

        if(Input.GetButtonDown("Jump")){
            movement.Jump();
        }
        
        movement.Move(inputDirection);
    }
}
