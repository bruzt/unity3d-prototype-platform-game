using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RopeSegmentController : MonoBehaviour
{
    private Rigidbody rigidBody;

    public float finalMaxXVelocity = 50;
    public float maxXVelocity = 1;
    public float finalMaxZRotationDegree = 80;
    public float maxZRotationDegree = 1;

    // Start is called before the first frame update
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        //LimitXMaxVelocity();
        //LimitZRotationDegree();
        //LimitXPosition();
    }

    void OnCollisionStay(Collision collision){
        if(collision.gameObject.name.Contains("Player")) OnCollisionStayPlayer(collision);
    }

    void OnCollisionExit(Collision collision){
        if(collision.gameObject.name.Contains("Player")) OnCollisionExitPlayer(collision);
    }

    void LimitXMaxVelocity(){
        Vector3 velocity = rigidBody.velocity;
        velocity.x = Mathf.Clamp(velocity.x, -maxXVelocity, maxXVelocity);
        rigidBody.velocity = velocity;
    }

    void LimitZRotationDegree(){
        Quaternion rotation = transform.rotation;
        rotation.z = Mathf.Clamp(rotation.z, -maxZRotationDegree, maxZRotationDegree);
        transform.rotation = rotation;
    }


    void LimitXPosition(){
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, position.x-0.1f, position.x+0.1f);
        transform.position = position;
    }

    public void IncreaseMaxXVelocity(int size, int position){

        float percent = (float)position/(float)size;

        float velocity = finalMaxXVelocity*percent;

        maxXVelocity = velocity;
    }

    public void DecreaseMaxZRotationDegree(int size, int position){
        
        float percent = (float)position/(float)size;

        float degree = finalMaxZRotationDegree*percent;

        maxZRotationDegree = degree;
    }

    private void OnCollisionStayPlayer(Collision collision){
        collision.gameObject.GetComponentInParent<PlayerController>().SetIsRopeSwinging(true);
    }

    private void OnCollisionExitPlayer(Collision collision){
        collision.gameObject.GetComponentInParent<PlayerController>().SetIsRopeSwinging(false);
    }
}
