using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehavior : MonoBehaviour
{
    private int currentHitPoints;

    public List<string> takesDamageOf;
    public int totalHitPoints = 1;
    public float timeToDestroy;
    public int ForceUpOnHit = 10000;

    // Start is called before the first frame update
    void Start()
    {
        currentHitPoints = totalHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        Rigidbody otherRigidbody = other.GetComponentInParent<Rigidbody>();

        foreach(string colliderName in takesDamageOf){
            if(other.name.Contains(colliderName)) {
                otherRigidbody.velocity = new Vector3(otherRigidbody.velocity.x, 0, 0);
                otherRigidbody.AddForce(0, ForceUpOnHit,0);
                ApplyDamage(1);
            }
        }
    }

    ////////////////////////////////////

    void ApplyDamage(int damage){
        currentHitPoints -= damage;

        transform.localScale = new Vector3(1, 0.5f, 1);

        if(currentHitPoints <= 0) Destroy();
    }

    void Destroy(){
        GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezePositionY;
        GetComponentInChildren<Collider>().enabled = false;
        Destroy(gameObject, timeToDestroy);
    }
}
