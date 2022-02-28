using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleBehavior : MonoBehaviour
{
    private int hitPoints;

    public string[] destructedByCollidersName;
    public int totalHitPoints = 100;
    public float timeToDestroy;
    public int ForceUpOnHit = 10000;

    // Start is called before the first frame update
    void Start()
    {
        hitPoints = totalHitPoints;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other){
        foreach(string colliderName in destructedByCollidersName){
            if(other.name.Contains(colliderName)) {
                other.GetComponentInParent<Rigidbody>().AddForce(0, ForceUpOnHit,0);
                ApplyDamage(1);
            }
        }
    }

    ////////////////////////////////////

    void ApplyDamage(int damage){
        hitPoints -= damage;

        if(hitPoints <= 0) Destroy();
    }

    void Destroy(){
        GetComponent<Rigidbody>().constraints =  RigidbodyConstraints.FreezePositionY;
        GetComponentInChildren<Collider>().enabled = false;
        Destroy(gameObject, timeToDestroy);
    }
}
