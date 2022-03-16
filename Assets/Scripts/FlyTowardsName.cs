using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyTowardsName : MonoBehaviour
{
    private bool shouldFlyTowardsName = false;
    private string nameToFlyTowards;
    [SerializeField] private float flySpeed = 5;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FlyTowards();
    }

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    private void FlyTowards(){
        if(shouldFlyTowardsName){
            transform.position = Vector3.MoveTowards(transform.position, GameObject.Find(nameToFlyTowards).transform.position, flySpeed * Time.deltaTime);
        }
    }

    ///////////////////////////////////////////////////////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////////////

    public void SetShouldFlyTowardsName(bool value){
        shouldFlyTowardsName = value;
    }

    public void SetNameToFlyTowards(string value){
        nameToFlyTowards = value;
    }

    public void SetFlySpeed(float value){
        flySpeed = value;
    }
}
