using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTowardsTag : MonoBehaviour
{
    private bool shouldFlyTowardsTag = false;
    private string tagToFlyTowards;
    [SerializeField] private float flySpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FlyTowardsPlayer();
    }

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    private void FlyTowardsPlayer(){
        if(shouldFlyTowardsTag){
            transform.position = Vector3.MoveTowards(transform.position, GameObject.FindGameObjectWithTag(tagToFlyTowards).transform.position, flySpeed * Time.deltaTime);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    public void SetShouldFlyTowardsTag(bool value){
        shouldFlyTowardsTag = value;
    }

    public void SetTag(string value){
        tagToFlyTowards = value;
    }

    public void SetFlySpeed(float value){
        flySpeed = value;
    }
}
