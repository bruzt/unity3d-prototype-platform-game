using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehavior : MonoBehaviour
{
    private int currentHitPoints;

    [SerializeField] private List<string> takesDamageOf;
    [SerializeField] private int totalHitPoints = 1;
    [SerializeField] private float timeToDestroy;
    [SerializeField] private int ForceUpOnHit = 10000;

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
            if(other.name.Contains(colliderName) && currentHitPoints > 0) {
                otherRigidbody.velocity = new Vector3(otherRigidbody.velocity.x, 0, 0);
                if(colliderName.Contains("Foot")) otherRigidbody.AddForce(0, ForceUpOnHit,0);
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
        GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezePosition;
        GetComponentInChildren<Collider>().enabled = false;
        Destroy(gameObject, timeToDestroy);
    }
}
