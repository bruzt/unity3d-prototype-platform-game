using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineController : MonoBehaviour
{
    [SerializeField] private float jumpForce = 20000;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /////////////////////////////////////////////////////////
    /////////////////////////////////////////////////////////

    public float GetJumpForce(){
        return jumpForce;
    }
}
